package com.proff.teamcity.roundhouse

import jetbrains.buildServer.agent.AgentBuildRunnerInfo
import jetbrains.buildServer.agent.BuildAgentConfiguration
import jetbrains.buildServer.agent.plugins.beans.PluginDescriptor
import jetbrains.buildServer.agent.runner.CommandLineBuildService
import jetbrains.buildServer.agent.runner.CommandLineBuildServiceFactory

class RoundhouseBuildServiceFactory(private val pluginDescriptor: PluginDescriptor) : CommandLineBuildServiceFactory {
    override fun createService(): CommandLineBuildService {
        return RoundhouseBuildService(pluginDescriptor)
    }

    override fun getBuildRunnerInfo(): AgentBuildRunnerInfo {
        return object : AgentBuildRunnerInfo {
            override fun getType(): String {
                return RoundhouseConstants.RUNNER_TYPE
            }

            override fun canRun(config: BuildAgentConfiguration): Boolean {
                return true
            }
        }
    }
}