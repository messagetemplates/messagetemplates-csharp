Push-Location $PSScriptRoot

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

& dotnet restore

$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]

echo "build: Version suffix is '$suffix'"

Push-Location src\MessageTemplates

if($suffix) {
    & dotnet pack -c Release --include-symbols --include-source --no-build -o ..\..\artifacts --version-suffix=$suffix
} else {
    & dotnet pack -c Release --include-symbols --include-source --no-build -o ..\..\artifacts
}
if($LASTEXITCODE -ne 0) { exit 1 }

Pop-Location

Push-Location test\MessageTemplates.Tests

& dotnet test -c Release
if($LASTEXITCODE -ne 0) { exit 2 }

Pop-Location

Push-Location test\MessageTemplates.Net40Tests

&nuget install ..\..\test\MessageTemplates.Net40Tests\packages.config -SolutionDirectory ..\..\

&msbuild ..\..\test\MessageTemplates.Net40Tests\MessageTemplates.Net40Tests.csproj /p:Configuration=Release
if($LASTEXITCODE -ne 0) { exit 2 }

& ..\..\packages\xunit.runner.console.2.2.0-beta1-build3239\tools\xunit.console.x86.exe ..\..\test\MessageTemplates.Net40Tests\bin\Release\MessageTemplates.Net40Tests.dll

if($LASTEXITCODE -ne 0) { exit 2 }

Pop-Location


Push-Location 

&nuget install test\MessageTemplates.Net35Tests\packages.config -SolutionDirectory .

&msbuild test\MessageTemplates.Net35Tests\MessageTemplates.Net35Tests.csproj /p:Configuration=Release
if($LASTEXITCODE -ne 0) { exit 2 }

& packages\xunit.runner.console.2.2.0-beta1-build3239\tools\xunit.console.x86.exe test\MessageTemplates.Net35Tests\bin\Release\MessageTemplates.Net35Tests.dll

if($LASTEXITCODE -ne 0) { exit 2 }

Pop-Location
