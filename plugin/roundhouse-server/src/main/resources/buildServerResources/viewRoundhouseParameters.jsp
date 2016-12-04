<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib prefix="props" tagdir="/WEB-INF/tags/props" %>
<jsp:useBean id="propertiesBean" scope="request" type="jetbrains.buildServer.controllers.BasePropertiesBean"/>
<jsp:useBean id="params" class="com.proff.teamcity.configTransformations.TransformParametersProvider"/>
<c:if test="${not empty propertiesBean.properties[params.deployTypeConfigKey]}">
    <div class="parameter">
        Deploy type: <props:displayValue name="${params.deployTypeConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.databaseTypeConfigKey]}">
    <div class="parameter">
        Database type: <props:displayValue name="${params.databaseTypeConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.serverConfigKey]}">
    <div class="parameter">
        Server name: <props:displayValue name="${params.serverConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.loginConfigKey]}">
    <div class="parameter">
        Login: <props:displayValue name="${params.loginConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.passwordConfigKey]}">
    <div class="parameter">
        Password: <props:displayValue name="${params.passwordConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.databaseConfigKey]}">
    <div class="parameter">
        Database name: <props:displayValue name="${params.databaseConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.folderConfigKey]}">
    <div class="parameter">
        Scripts folder: <props:displayValue name="${params.folderConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.environmentConfigKey]}">
    <div class="parameter">
        Environment: <props:displayValue name="${params.environmentConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.versionConfigKey]}">
    <div class="parameter">
        Database version: <props:displayValue name="${params.versionConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.failOnNonUTF8ConfigKey]}">
    <div class="parameter">
        Fail if found non UTF-8 file: <props:displayValue name="${params.failOnNonUTF8ConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.commandConfigKey]}">
    <div class="parameter">
        Command line arguments: <props:displayValue name="${params.commandConfigKey}"/>
    </div>
</c:if>
<c:if test="${not empty propertiesBean.properties[params.verboseLoggingKey]}">
    <div class="parameter">
        Verbose logging: <props:displayValue name="${params.verboseLoggingKey}"/>
    </div>
</c:if>
