package com.proff.teamcity.roundhouse

import jetbrains.buildServer.agent.plugins.beans.PluginDescriptor
import jetbrains.buildServer.agent.runner.BuildServiceAdapter
import jetbrains.buildServer.agent.runner.ProgramCommandLine
import jetbrains.buildServer.agent.runner.SimpleProgramCommandLine
import java.util.*

class RoundhouseBuildService(private val pluginDescriptor: PluginDescriptor) : BuildServiceAdapter() {
    override fun makeProgramCommandLine(): ProgramCommandLine {
        val env = HashMap(environmentVariables)
        val params = runnerParameters

        if (params[RoundhouseConstants.DEPLOY_TYPE_CONFIG_KEY] != null)
            env[RoundhouseConstants.DEPLOY_TYPE_CONFIG_KEY] = params[RoundhouseConstants.DEPLOY_TYPE_CONFIG_KEY]

        if (params[RoundhouseConstants.DATABASE_TYPE_CONFIG_KEY] != null)
            env[RoundhouseConstants.DATABASE_TYPE_CONFIG_KEY] = params[RoundhouseConstants.DATABASE_TYPE_CONFIG_KEY]

        if (params[RoundhouseConstants.SERVER_CONFIG_KEY] != null)
            env[RoundhouseConstants.SERVER_CONFIG_KEY] = params[RoundhouseConstants.SERVER_CONFIG_KEY]

        if (params[RoundhouseConstants.LOGIN_CONFIG_KEY] != null)
            env[RoundhouseConstants.LOGIN_CONFIG_KEY] = params[RoundhouseConstants.LOGIN_CONFIG_KEY]

        if (params[RoundhouseConstants.PASSWORD_CONFIG_KEY] != null)
            env[RoundhouseConstants.PASSWORD_CONFIG_KEY] = params[RoundhouseConstants.PASSWORD_CONFIG_KEY]

        if (params[RoundhouseConstants.DATABASE_CONFIG_KEY] != null)
            env[RoundhouseConstants.DATABASE_CONFIG_KEY] = params[RoundhouseConstants.DATABASE_CONFIG_KEY]

        if (params[RoundhouseConstants.FOLDER_CONFIG_KEY] != null)
            env[RoundhouseConstants.FOLDER_CONFIG_KEY] = params[RoundhouseConstants.FOLDER_CONFIG_KEY]

        if (params[RoundhouseConstants.ENVIRONMENT_CONFIG_KEY] != null)
            env[RoundhouseConstants.ENVIRONMENT_CONFIG_KEY] = params[RoundhouseConstants.ENVIRONMENT_CONFIG_KEY]

        if (params[RoundhouseConstants.VERSION_CONFIG_KEY] != null)
            env[RoundhouseConstants.VERSION_CONFIG_KEY] = params[RoundhouseConstants.VERSION_CONFIG_KEY]

        if (params[RoundhouseConstants.FAIL_ON_NON_UTF8_CONFIG_KEY] != null)
            env[RoundhouseConstants.FAIL_ON_NON_UTF8_CONFIG_KEY] = params[RoundhouseConstants.FAIL_ON_NON_UTF8_CONFIG_KEY]

        if (params[RoundhouseConstants.VERBOSE_CONFIG_KEY] != null)
            env[RoundhouseConstants.VERBOSE_CONFIG_KEY] = params[RoundhouseConstants.VERBOSE_CONFIG_KEY]

        return SimpleProgramCommandLine(env, workingDirectory.absolutePath, pluginDescriptor.pluginRoot.absolutePath + "/Roundhouse/roundhouse.exe", params[RoundhouseConstants.COMMAND_CONFIG_KEY]?.split(" ") ?: listOf())
    }
}