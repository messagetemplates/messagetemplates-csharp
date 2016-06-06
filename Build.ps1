Push-Location $PSScriptRoot

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

& dotnet restore

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];

Push-Location src\MessageTemplates

& dotnet pack -c Release -o ..\..\.\artifacts --version-suffix=$revision
if($LASTEXITCODE -ne 0) { exit 1 }    

Pop-Location

Push-Location test\MessageTemplates.Tests

& dotnet test -c Release
if($LASTEXITCODE -ne 0) { exit 2 }

Pop-Location

Push-Location test\MessageTemplates.Net40Tests

&nuget install ..\..\test\MessageTemplates.Net40Tests\packages.config -SolutionDirectory ..\..\..

&msbuild ..\..\test\MessageTemplates.Net40Tests\MessageTemplates.Net40Tests.csproj /p:Configuration=Release
if($LASTEXITCODE -ne 0) { exit 2 }

& ..\..\packages\xunit.runner.console.2.2.0-beta1-build3239\tools\xunit.console.x86.exe ..\..\test\MessageTemplates.Net40Tests\bin\Release\MessageTemplates.Net40Tests.dll

if($LASTEXITCODE -ne 0) { exit 2 }

Pop-Location
