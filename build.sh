#!/bin/bash

# note: this build script is incomplete (no pack, only build netstandard/netcoreapp/net40 for now)
dotnet restore

for path in src/*/*.csproj; do
    dirname="$(dirname "${path}")"
    dotnet build -f netstandard1.0 ${dirname} -c Release
    dotnet build -f netstandard1.3 ${dirname} -c Release
done

for path in test/MessageTemplates.Tests/*.csproj; do
    dirname="$(dirname "${path}")"
    dotnet build ${dirname} -f netcoreapp1.0 -c Release
    dotnet test ${dirname} -f netcoreapp1.0  -c Release
done

nuget restore test/MessageTemplates.Net40Tests/packages.config -SolutionDirectory .
msbuild /v:m test/MessageTemplates.Net40Tests/MessageTemplates.Net40Tests.csproj /p:Configuration=Release
mono packages/xunit.runner.console.2.2.0-beta1-build3239/tools/xunit.console.exe test/MessageTemplates.Net40Tests/bin/Release/MessageTemplates.Net40Tests.dll
