using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using roundhouse.infrastructure.app;
using roundhouse.infrastructure.logging;

namespace roundhouse
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;

            var deployType = Environment.GetEnvironmentVariable("roundhouse-deploy-type");
            DatabaseType? databaseType = null;
            DatabaseType t;
            if (Enum.TryParse(Environment.GetEnvironmentVariable("roundhouse-database-type"), out t))
                databaseType = t;
            var server = Environment.GetEnvironmentVariable("roundhouse-server");
            var login = Environment.GetEnvironmentVariable("roundhouse-login");
            var password = Environment.GetEnvironmentVariable("roundhouse-password");
            var database = Environment.GetEnvironmentVariable("roundhouse-database");
            var folder = Environment.GetEnvironmentVariable("roundhouse-folder");
            var environment = Environment.GetEnvironmentVariable("roundhouse-environment");
            var version = Environment.GetEnvironmentVariable("roundhouse-version");
            var failonNonUTF8 = Environment.GetEnvironmentVariable("roundhouse-failOnNonUTF8") == "true";
            var verbose = Environment.GetEnvironmentVariable("roundhouse-verbose") == "true";

            DeployType[] types;
            switch (deployType)
            {
                case "deploy":
                    types = new[] {DeployType.Deploy};
                    break;
                case "drop":
                    types = new[] {DeployType.Drop};
                    break;
                case "dropDeploy":
                    types = new[] {DeployType.Drop, DeployType.Deploy};
                    break;
                default:
                    throw new InvalidOperationException("Unknown deploy type");
            }

            var filesChecked = false;
            foreach (var type in types)
            {
                var a = new List<string>(args);

                if (databaseType != null)
                {
                    a.Add("-dt");
                    a.Add(databaseType.ToString());
                }

                if (!string.IsNullOrWhiteSpace(server))
                {
                    a.Add("-s");
                    a.Add(server);
                }

                if (!string.IsNullOrWhiteSpace(database))
                {
                    a.Add("-d");
                    a.Add(database);
                }

                if (!string.IsNullOrWhiteSpace(folder))
                {
                    a.Add("-f");
                    a.Add(folder);
                }

                if (!string.IsNullOrWhiteSpace(environment))
                {
                    a.Add("-env");
                    a.Add(environment);
                }

                if (!string.IsNullOrWhiteSpace(version))
                {
                    a.Add("-v");
                    a.Add(version);
                }

                if (!string.IsNullOrWhiteSpace(login))
                {
                    switch (databaseType)
                    {
                        case DatabaseType.sqlserver:
                        case DatabaseType.sql2000:
                            a.Add("-c");
                            a.Add(
                                $"data source={server};initial catalog={database};Integrated Security=false;User ID={login};Password={password};");
                            break;
                        case DatabaseType.oracle:
                            a.Add("-c");
                            a.Add($"Data Source={server}/{database};User Id={login};Password={password};Integrated Security=no;");
                            a.Add("-csa");
                            a.Add($"Data Source={server};User Id={login};Password={password};Integrated Security=no;");
                            break;
                        case DatabaseType.mysql:
                            a.Add("-c");
                            a.Add($"Server={server};Database={database};Port=3306;Uid={login};Pwd={password};");
                            break;
                        case DatabaseType.postgres:
                            a.Add("-c");
                            a.Add($"Server={server};Database={database};Port=5432;UserId={login};Password={password};");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (type == DeployType.Drop)
                    a.Add("--drop");

                if (args.Length > 0)
                    a.AddRange(args);

                var config = console.Program.set_up_configuration_and_build_the_container(a.ToArray());

                var logger = new TeamCityServiceMessages().CreateWriter();
                config.Silent = true;
                config.Debug = verbose;

                if (deployType != "drop" && !filesChecked)
                {
                    var badFiles = new List<string>();
                    foreach (
                        var file in Directory.GetFiles(config.SqlFilesDirectory, "*.sql", SearchOption.AllDirectories))
                    {
                        var preamble = Encoding.UTF8.GetPreamble();
                        using (var f = new BufferedStream(File.OpenRead(file)))
                        {
                            if (f.Length >= preamble.Length)
                            {
                                var buffer = new byte[preamble.Length];
                                f.Read(buffer, 0, preamble.Length);
                                if (preamble.SequenceEqual(buffer))
                                {
                                    continue;
                                }
                            }
                            f.Position = 0;
                            int b;
                            while ((b = f.ReadByte()) != -1)
                            {
                                if (b > 127)
                                {
                                    badFiles.Add(file);
                                    break;
                                }
                            }
                        }
                    }

                    if (badFiles.Count > 0)
                    {
                        var message = $"Found non UTF-8 files:{Environment.NewLine}{string.Join(Environment.NewLine, badFiles)}";
                        if (failonNonUTF8)
                        {
                            foreach (var badFile in badFiles)
                            {
                                var identity = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(badFile));
                                logger.WriteBuildProblem(Convert.ToBase64String(identity),
                                    $"File {badFile} is not in UTF8 encoding");
                            }
                            return;
                        }
                        logger.WriteWarning(message);
                    }
                }

                using (var l = logger.OpenBlock(type.ToString()))
                {
                    config.Logger = new CustomLogger(l, verbose);
                    WriteStartupParameters(l, config);
                    var migrate = new Migrate();
                    migrate.Set(c => ApplyConfig(config, c));
                    migrate.Run();
                }
            }
        }

        private static void ApplyConfig(ConfigurationPropertyHolder from, ConfigurationPropertyHolder to)
        {
            to.Logger = from.Logger;
            to.ServerName = from.ServerName;
            to.DatabaseName = from.DatabaseName;
            to.ConnectionString = from.ConnectionString;
            to.ConnectionStringAdmin = from.ConnectionStringAdmin;
            to.CommandTimeout = from.CommandTimeout;
            to.CommandTimeoutAdmin = from.CommandTimeoutAdmin;
            to.SqlFilesDirectory = from.SqlFilesDirectory;
            to.RepositoryPath = from.RepositoryPath;
            //to.Version = from.Version;
            to.VersionFile = from.VersionFile;
            to.VersionXPath = from.VersionXPath;
            to.AlterDatabaseFolderName = from.AlterDatabaseFolderName;
            to.RunAfterCreateDatabaseFolderName = from.RunAfterCreateDatabaseFolderName;
            to.RunBeforeUpFolderName = from.RunBeforeUpFolderName;
            to.UpFolderName = from.UpFolderName;
            to.DownFolderName = from.DownFolderName;
            to.RunFirstAfterUpFolderName = from.RunFirstAfterUpFolderName;
            to.FunctionsFolderName = from.FunctionsFolderName;
            to.ViewsFolderName = from.ViewsFolderName;
            to.SprocsFolderName = from.SprocsFolderName;
            to.IndexesFolderName = from.IndexesFolderName;
            to.RunAfterOtherAnyTimeScriptsFolderName = from.RunAfterOtherAnyTimeScriptsFolderName;
            to.PermissionsFolderName = from.PermissionsFolderName;
            /*to.BeforeMigrationFolderName = from.BeforeMigrationFolderName;
            to.AfterMigrationFolderName = from.AfterMigrationFolderName;*/
            to.SchemaName = from.SchemaName;
            to.VersionTableName = from.VersionTableName;
            to.ScriptsRunTableName = from.ScriptsRunTableName;
            to.ScriptsRunErrorsTableName = from.ScriptsRunErrorsTableName;
            to.EnvironmentName = from.EnvironmentName;
            to.Restore = from.Restore;
            to.RestoreFromPath = from.RestoreFromPath;
            to.RestoreCustomOptions = from.RestoreCustomOptions;
            to.RestoreTimeout = from.RestoreTimeout;
            to.CreateDatabaseCustomScript = from.CreateDatabaseCustomScript;
            to.OutputPath = from.OutputPath;
            to.WarnOnOneTimeScriptChanges = from.WarnOnOneTimeScriptChanges;
            //to.WarnAndIgnoreOnOneTimeScriptChanges = from.WarnAndIgnoreOnOneTimeScriptChanges;
            to.Silent = from.Silent;
            to.DatabaseType = from.DatabaseType;
            to.Drop = from.Drop;
            to.DoNotCreateDatabase = from.DoNotCreateDatabase;
            to.WithTransaction = from.WithTransaction;
            to.RecoveryMode = from.RecoveryMode;
            to.Debug = from.Debug;
            to.DryRun = from.DryRun;
            to.Baseline = from.Baseline;
            to.RunAllAnyTimeScripts = from.RunAllAnyTimeScripts;
            to.DisableTokenReplacement = from.DisableTokenReplacement;
            to.SearchAllSubdirectoriesInsteadOfTraverse = from.SearchAllSubdirectoriesInsteadOfTraverse;
            to.DisableOutput = from.DisableOutput;
        }

        private static void WriteStartupParameters(ITeamCityWriter logger, ConfigurationPropertyHolder config)
        {
            using (var l = logger.OpenBlock("startup parameters"))
            {
                l.WriteMessage($"ServerName: {config.ServerName}");
                l.WriteMessage($"DatabaseName: {config.DatabaseName}");
                l.WriteMessage($"ConnectionString: {config.ConnectionString}");
                l.WriteMessage($"ConnectionStringAdmin: {config.ConnectionStringAdmin}");
                l.WriteMessage($"CommandTimeout: {config.CommandTimeout}");
                l.WriteMessage($"CommandTimeoutAdmin: {config.CommandTimeoutAdmin}");
                l.WriteMessage($"SqlFilesDirectory: {config.SqlFilesDirectory}");
                l.WriteMessage($"RepositoryPath: {config.RepositoryPath}");
                //l.WriteMessage($"Version: {config.Version}");
                l.WriteMessage($"VersionFile: {config.VersionFile}");
                l.WriteMessage($"VersionXPath: {config.VersionXPath}");
                l.WriteMessage($"AlterDatabaseFolderName: {config.AlterDatabaseFolderName}");
                l.WriteMessage($"RunAfterCreateDatabaseFolderName: {config.RunAfterCreateDatabaseFolderName}");
                l.WriteMessage($"RunBeforeUpFolderName: {config.RunBeforeUpFolderName}");
                l.WriteMessage($"UpFolderName: {config.UpFolderName}");
                l.WriteMessage($"DownFolderName: {config.DownFolderName}");
                l.WriteMessage($"RunFirstAfterUpFolderName: {config.RunFirstAfterUpFolderName}");
                l.WriteMessage($"FunctionsFolderName: {config.FunctionsFolderName}");
                l.WriteMessage($"ViewsFolderName: {config.ViewsFolderName}");
                l.WriteMessage($"SprocsFolderName: {config.SprocsFolderName}");
                l.WriteMessage($"IndexesFolderName: {config.IndexesFolderName}");
                l.WriteMessage($"RunAfterOtherAnyTimeScriptsFolderName: {config.RunAfterOtherAnyTimeScriptsFolderName}");
                l.WriteMessage($"PermissionsFolderName: {config.PermissionsFolderName}");
                //l.WriteMessage($"BeforeMigrationFolderName: {config.BeforeMigrationFolderName}");
                l.WriteMessage($"SchemaName: {config.SchemaName}");
                l.WriteMessage($"VersionTableName: {config.VersionTableName}");
                l.WriteMessage($"ScriptsRunTableName: {config.ScriptsRunTableName}");
                l.WriteMessage($"ScriptsRunErrorsTableName: {config.ScriptsRunErrorsTableName}");
                l.WriteMessage($"EnvironmentName: {config.EnvironmentName}");
                l.WriteMessage($"Restore: {config.Restore}");
                l.WriteMessage($"RestoreFromPath: {config.RestoreFromPath}");
                l.WriteMessage($"RestoreCustomOptions: {config.RestoreCustomOptions}");
                l.WriteMessage($"RestoreTimeout: {config.RestoreTimeout}");
                l.WriteMessage($"CreateDatabaseCustomScript: {config.CreateDatabaseCustomScript}");
                l.WriteMessage($"OutputPath: {config.OutputPath}");
                l.WriteMessage($"WarnOnOneTimeScriptChanges: {config.WarnOnOneTimeScriptChanges}");
                //l.WriteMessage($"WarnAndIgnoreOnOneTimeScriptChanges: {config.WarnAndIgnoreOnOneTimeScriptChanges}");
                l.WriteMessage($"Silent: {config.Silent}");
                l.WriteMessage($"DatabaseType: {config.DatabaseType}");
                l.WriteMessage($"Drop: {config.Drop}");
                l.WriteMessage($"DoNotCreateDatabase: {config.DoNotCreateDatabase}");
                l.WriteMessage($"WithTransaction: {config.WithTransaction}");
                l.WriteMessage($"RecoveryMode: {config.RecoveryMode}");
                l.WriteMessage($"Debug: {config.Debug}");
                l.WriteMessage($"DryRun: {config.DryRun}");
                l.WriteMessage($"RunAllAnyTimeScripts: {config.RunAllAnyTimeScripts}");
                l.WriteMessage($"DisableTokenReplacement: {config.DisableTokenReplacement}");
                l.WriteMessage(
                    $"SearchAllSubdirectoriesInsteadOfTraverse: {config.SearchAllSubdirectoriesInsteadOfTraverse}");
                l.WriteMessage($"DisableOutput: {config.DisableOutput}");
                l.WriteMessage($"Logger: {config.Logger}");
            }
        }
    }

    internal class CustomLogger : Logger
    {
        private readonly ITeamCityWriter _logger;
        private readonly bool _verbose;

        public CustomLogger(ITeamCityWriter logger, bool verbose)
        {
            _logger = logger;
            _verbose = verbose;
        }

        public void log_a_debug_event_containing(string message, params object[] args)
        {
            if (_verbose)
                _logger.WriteMessage(string.Format(message, args));
        }

        public void log_an_info_event_containing(string message, params object[] args)
        {
            _logger.WriteMessage(string.Format(message, args));
        }

        public void log_a_warning_event_containing(string message, params object[] args)
        {
            _logger.WriteWarning(string.Format(message, args));
        }

        public void log_an_error_event_containing(string message, params object[] args)
        {
            _logger.WriteError(string.Format(message, args));
        }

        public void log_a_fatal_event_containing(string message, params object[] args)
        {
            _logger.WriteError(string.Format(message, args));
        }

        public object underlying_type => null;
    }
}