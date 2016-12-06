# TeamCity Roundhouse Plugin

The TeamCity Roundhouse plugin provides support of deploy database via [roundhouse](https://github.com/chucknorris/roundhouse)

# Compatibility

The plugin is compatible with [TeamCity](https://www.jetbrains.com/teamcity/download/) 9.1.x and greater.

# Requirements
.net framework 4.5 on agent

# Build

1. build visual studio solution *runner/roundhouse.sln*
2. copy builded filesto *plugin/roundhouse-agent/roundhouse*
3. execute *mvn package* in *plugin* folder