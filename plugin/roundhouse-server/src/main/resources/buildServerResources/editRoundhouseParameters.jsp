<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<%@ taglib prefix="bs" tagdir="/WEB-INF/tags" %>
<jsp:useBean id="params" class="com.proff.teamcity.roundhouse.RoundhouseParametersProvider"/>
<tr>
    <th><label for="${params.deployTypeConfigKey}">Deploy type:</label></th>
    <td>
        <div class="posRel">
            <props:selectProperty name="${params.deployTypeConfigKey}" className="mediumField">
                <props:option value="deploy">Deploy</props:option>
                <props:option value="drop">Drop</props:option>
                <props:option value="dropDeploy">Drop and deploy</props:option>
            </props:selectProperty>
        </div>
        <span class="error" id="error_${params.deployTypeConfigKey}"></span>
        <span class="smallNote">Command line parameter: --drop. For "Drop and deploy" will be 2 commands.</span>
    </td>
</tr>
<tr>
    <th><label for="${params.databaseTypeConfigKey}">Database type:</label></th>
    <td>
        <div class="posRel">
            <props:selectProperty name="${params.databaseTypeConfigKey}" className="mediumField">
                <props:option value="sqlserver">SQLServer 2005+</props:option>
                <props:option value="sql2000">SQLServer 2000</props:option>
                <props:option value="oracle">Oracle 9i+</props:option>
                <props:option value="mysql">MySQL</props:option>
                <props:option value="postgres">PostgreSQL</props:option>
            </props:selectProperty>
        </div>
        <span class="error" id="error_${params.databaseTypeConfigKey}"></span>
        <span class="smallNote">Tells RH what type of database it is running on. Command line parameter: --dt</span>
    </td>
</tr>
<tr>
    <th><label for="${params.serverConfigKey}">Server name:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.serverConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.serverConfigKey}"></span>
        <span class="smallNote">The server and instance you would like to run on. (local) and (local)\SQL2008 are both valid values. Defaults to (local). Command line parameter: -s</span>
    </td>
</tr>
<tr>
    <th><label for="${params.loginConfigKey}">Login:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.loginConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.loginConfigKey}"></span>
        <span class="smallNote">Leave empty for Windows authorization. Command line parameter: -c. For Oracle also --csa</span>
    </td>
</tr>
<tr>
    <th><label for="${params.passwordConfigKey}">Password:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.passwordConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.passwordConfigKey}"></span>
        <span class="smallNote">Command line parameter: -c. For Oracle also --csa</span>
    </td>
</tr>
<tr>
    <th><label for="${params.databaseConfigKey}">Database name:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.databaseConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.databaseConfigKey}"></span>
        <span class="smallNote">The database you want to create/migrate. Command line parameter: -d</span>
    </td>
</tr>
<tr>
    <th><label for="${params.folderConfigKey}">Scripts folder:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.folderConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.folderConfigKey}"></span>
        <span class="smallNote">The directory where your SQL scripts are. Command line parameter: -f</span>
    </td>
</tr>
<tr>
    <th><label for="${params.environmentConfigKey}">Environment:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.environmentConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.environmentConfigKey}"></span>
        <span class="smallNote">This allows RH to be environment aware and only run scripts that are in a particular environment based on the namingof the script. LOCAL.something.ENV.sql would only be run in the LOCAL environment. Defaults to LOCAL. Command line parameter: --env</span>
    </td>
</tr>
<tr>
    <th><label for="${params.versionConfigKey}">Database version:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.versionConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.versionConfigKey}"></span>
        <span class="smallNote">Specify the version directly instead of looking in a file. If present, ignores file version options. Command line parameter: -v</span>
    </td>
</tr>
<tr>
    <th><label for="${params.verboseLoggingKey}">If non UTF-8 file found:</label></th>
    <td>
        <props:selectProperty name="${params.failOnNonUTF8ConfigKey}" className="mediumField">
            <props:option value="false">log warning</props:option>
            <props:option value="true">add build problem</props:option>
        </props:selectProperty>
        <span class="smallNote">When a file not in UTF-8 encoding, there may be a problem with encoding on agents with a different default encoding (only files with non ASCII symbols are checked)</span>
    </td>
</tr>
<tr>
    <th><label for="${params.commandConfigKey}">Command line arguments:</label></th>
    <td>
        <div class="posRel">
            <props:textProperty name="${params.commandConfigKey}" className="longField"/>
        </div>
        <span class="error" id="error_${params.commandConfigKey}"></span>
        <span class="smallNote">Additional command line arguments. <a href="https://github.com/chucknorris/roundhouse/wiki/ConfigurationOptions">Help.</a> </span>
    </td>
</tr>
<tr>
    <th><label for="${params.verboseLoggingKey}">Verbose logging:</label></th>
    <td>
        <props:checkboxProperty name="${params.verboseLoggingKey}"/>
    </td>
</tr>
