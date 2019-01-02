$key = cat $PSScriptRoot\..\private\NugetApiKey.txt

Publish-Module -Path $PSScriptRoot\..\pswinsearch -NugetApiKey $key -Verbose
