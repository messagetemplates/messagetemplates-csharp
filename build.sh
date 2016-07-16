#!/bin/bash
dotnet restore
for path in src/*/project.json; do
    dirname="$(dirname "${path}")"
    dotnet build ${dirname} -c Release
done

for path in test/MessageTemplates.Tests/project.json; do
    dirname="$(dirname "${path}")"
    dotnet build ${dirname} -f netcoreapp1.0 -c Release
    dotnet test ${dirname} -f netcoreapp1.0  -c Release
done

nuget restore test/MessageTemplates.Net40Tests/packages.config -SolutionDirectory .
xbuild test/MessageTemplates.Net40Tests/MessageTemplates.Net40Tests.csproj /p:Configuration=Release
mono packages/xunit.runner.console.2.2.0-beta1-build3239/tools/xunit.console.exe test/MessageTemplates.Net40Tests/bin/Release/MessageTemplates.Net40Tests.dll
