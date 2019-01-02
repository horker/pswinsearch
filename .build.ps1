task . Compile, CompileHelp, Build, ImportDebug, UpdateHelpMarkdown

Set-StrictMode -Version 4

############################################################
# Settings
############################################################

$SOURCE_PATH = "$PSScriptRoot\source\x64"
$SCRIPT_PATH = "$PSScriptRoot\scripts"

$MODULE_PATH = "$PSScriptRoot\pswinsearch"
$MODULE_PATH_DEBUG = "$PSScriptRoot\debug\pswinsearch"

$HELP_PATH = "$PSScriptRoot\docs\en-US"

$SOLUTION_FILE = "$PSScriptRoot\source\pswinsearch.sln"

$OBJECT_FILES = @(
    "Horker.WindowsSearch.dll"
    "Horker.WindowsSearch.pdb"
    "SearchQueryHelper.dll"
    "SearchQueryHelper.pdb"
)

############################################################
# Helper cmdlets
############################################################

function New-Folder2 {
    param(
        [string]$Path
    )

    try {
        $null = New-Item -Type Directory $Path -EA Stop
        Write-Host -ForegroundColor DarkCyan "$Path created"
    }
    catch {
        Write-Host -ForegroundColor DarkYellow $_
    }
}

function Copy-Item2 {
    param(
        [string]$Source,
        [string]$Dest,
        [switch]$Force
    )

    try {
        Copy-Item $Source $Dest -EA Stop -Force:$Force
        Write-Host -ForegroundColor DarkCyan "Copy from $Source to $Dest done"
    }
    catch {
        Write-Host -ForegroundColor DarkYellow $_
    }
}

function Remove-Item2 {
    param(
        [string]$Path
    )

    Resolve-Path $PATH | foreach {
        try {
            Remove-Item $_ -EA Stop -Recurse -Force
            Write-Host -ForegroundColor DarkCyan "$_ removed"
        }
        catch {
            Write-Host -ForegroundColor DarkYellow $_
        }
    }
}

############################################################
# Tasks
############################################################

task Compile {
    msbuild $SOLUTION_FILE /p:Configuration=Debug /nologo /v:minimal
    msbuild $SOLUTION_FILE /p:Configuration=Release /nologo /v:minimal
}

task Build {
    . {
        $ErrorActionPreference = "Continue"

        function Copy-ObjectFiles {
            param(
                [string]$targetPath,
                [string]$objectPath
            )

            New-Folder2 $targetPath

            Copy-Item2 "$SCRIPT_PATH\*" $targetPath
            $OBJECT_FILES | foreach {
                $path = Join-Path $objectPath $_
                Copy-Item2 $path $targetPath
            }
        }

        Copy-ObjectFiles $MODULE_PATH "$SOURCE_PATH\Release"
        Copy-ObjectFiles $MODULE_PATH_DEBUG "$SOURCE_PATH\Debug"

        New-Folder2 "$MODULE_PATH\en-US"
        Copy-Item2 "$HELP_PATH\*" "$MODULE_PATH\en-US"
        New-Folder2 "$MODULE_PATH_DEBUG\en-US"
        Copy-Item2 "$HELP_PATH\*" "$MODULE_PATH_DEBUG\en-US"
    }
}

task Test Build, ImportDebug, {
    Invoke-Pester "$PSScriptRoot\tests"
}

task ImportDebug {
    Import-Module $MODULE_PATH_DEBUG -Force
}

task Clean {
    Remove-Item2 "$MODULE_PATH\*"
    Remove-Item2 "$MODULE_PATH_DEBUG\*"
}

task UpdateHelpMarkdown {
    $null = Update-MarkdownHelp docs
}

task CompileHelp {
    $null = New-ExternalHelp docs -OutputPath docs\en-US -Force
}
