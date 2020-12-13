nuget restore
msbuild CoreBot.sln -p:DeployOnBuild=true -p:PublishProfile=.\visbotapp-Web-Deploy.pubxml -p:Password=tF3lq0wiXMCHEDZJHfcPLkmaJW8YE4eAJT3nXBF01ANxnSXrmSxTtdwf0kwx

