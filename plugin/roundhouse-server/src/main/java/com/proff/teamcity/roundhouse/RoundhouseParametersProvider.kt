package com.proff.teamcity.roundhouse

class RoundhouseParametersProvider {
    val deployTypeConfigKey: String
        get() = RoundhouseConstants.DEPLOY_TYPE_CONFIG_KEY
    val databaseTypeConfigKey: String
        get() = RoundhouseConstants.DATABASE_TYPE_CONFIG_KEY
    val serverConfigKey: String
        get() = RoundhouseConstants.SERVER_CONFIG_KEY
    val loginConfigKey: String
        get() = RoundhouseConstants.LOGIN_CONFIG_KEY
    val passwordConfigKey: String
        get() = RoundhouseConstants.PASSWORD_CONFIG_KEY
    val databaseConfigKey: String
        get() = RoundhouseConstants.DATABASE_CONFIG_KEY
    val folderConfigKey: String
        get() = RoundhouseConstants.FOLDER_CONFIG_KEY
    val verboseLoggingKey: String
        get() = RoundhouseConstants.VERBOSE_CONFIG_KEY
    val environmentConfigKey: String
        get() = RoundhouseConstants.ENVIRONMENT_CONFIG_KEY
    val failOnNonUTF8ConfigKey: String
        get() = RoundhouseConstants.FAIL_ON_NON_UTF8_CONFIG_KEY
    val versionConfigKey: String
        get() = RoundhouseConstants.VERSION_CONFIG_KEY
    val commandConfigKey: String
        get() = RoundhouseConstants.COMMAND_CONFIG_KEY
}