package com.proff.teamcity.roundhouse

import com.intellij.openapi.diagnostic.Logger
import jetbrains.buildServer.agent.AgentLifeCycleAdapter
import jetbrains.buildServer.agent.AgentLifeCycleListener
import jetbrains.buildServer.agent.BuildAgent
import jetbrains.buildServer.util.EventDispatcher
import jetbrains.buildServer.util.StringUtil
import org.apache.commons.io.FileUtils
import java.io.File
import java.util.regex.Pattern

class AppAgent(dispatcher: EventDispatcher<AgentLifeCycleListener>) : AgentLifeCycleAdapter() {
    private val LOG = Logger.getInstance(FileUtils::class.java.name)

    init {
        dispatcher.addListener(this)
    }

    override fun agentInitialized(agent: BuildAgent) {
        super.agentInitialized(agent)
        val pathVariable = System.getenv("PATH")
        val paths = StringUtil.splitHonorQuotes(pathVariable, File.pathSeparatorChar)
        val oracle = findToolPath(paths, Pattern.compile("^.*\\\\oci\\.dll$"))
        if (oracle != null) {
            agent.configuration.addConfigurationParameter("OracleClientTools", oracle)
        }
    }

    private fun findToolPath(paths: List<String>, pattern: Pattern): String? {
        for (path in paths) {
            val directory = File(path)
            if (!directory.exists()) {
                LOG.debug("Ignoring non existing directory $path")
                continue
            }

            val files = directory.listFiles()
            if (files == null) {
                LOG.debug("Ignoring empty directory $path")
                continue
            }

            files
                    .map { it.absolutePath }
                    .filter { pattern.matcher(it).find() }
                    .forEach { return it }
        }

        return null
    }
}