param(
    [string]$pathToSecrets = "../secrets/",
    [string]$apiKeyFile = "SelfEduNugetPushKey",
    [string]$nugetSource = "nuget.org",
    [switch]$signKey,
    [switch]$clear
)

if($clear)
{
    Remove-Item .\PackageOutput\ -Recurse
}

dotnet pack -c Release --output .\PackageOutput
$apiKey = Get-Content "$pathToSecrets$apiKeyFile" -Raw
if ($signKey)
{
    nuget push -Source "$nugetSource" -ApiKey "$apiKey" .\PackageOutput\
}
else
{
    nuget push -Source "$nugetSource" .\PackageOutput\
}