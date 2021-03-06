# pswinsearch

This is a module for PowerShell to search the computer using Windows Search.

## Overview

The module provides the cmdlet `Invoke-WindowsSearch` (the alias is `iws`). This cmdlet makes a query to the Windows Search subsystem and gets results as PowerShell objects.

You can make a query in one of the following query languages or a combination of both.

(1) Advanced Query Syntax (AQS)

The Advanced Query Syntax is the query language that is used in the search box in Windows Explorer.

(2) Windows Search SQL

The Windows Search subsystem implements the OLE DB provider as its core query engine. By accessing it directly, you can make a search query by SQL. It offers wordy but cleaner syntax than AQS and can be written in a locale-independent manner.

The syntaxes of these query languages are documented in the following pages:

[Advanced Query Syntax]( https://docs.microsoft.com/ja-jp/windows/desktop/search/-search-3x-advancedquerysyntax)

[Windows Search SQL]( https://docs.microsoft.com/ja-jp/windows/desktop/search/-search-sql-windowssearch-entry)

The detailed documentation for the cmdlets in the module is available in the [docs/]( https://github.com/horker/pswinsearch/tree/master/docs) directory.

## Installation

This module is published in [PowerShell Gallery](https://www.powershellgallery.com/packages/pswinsearch)

```powershell
Install-Module pswinsearch
```

## Examples

### Example 1
```powershell
PS C:\> iws "the beatles"
```

This command searches for "the beatles" by Windows Search.

### Example 2
```powershell
PS C:\> iws "the beatles" -Path ~\Music -SelectColumns Search.Rank, ItemPathDisplay -TotalCount 5 -Sorting "Search.Rank DESC"
```

This command searches for "the beatles" in the Music folder. The outputs include the columns System.Search.Rank and System.ItemPathDisplay. Top 5 results are displayed in the descending order of the search score.

### Example 3
```powershell
PS C:\> iws "the beatles" -Where "@DateModified >= '2018/01/01'"
```

This is an example of a combination of AQS and SQL queries. This command searches for "the beatles" with the condition that the last modified date is newer than or equal to January 1, 2018.

The word `@DateModified` is an abbreviation of `System.DateModified`. You can place a canonical name without the leading `System.` (e.g., `ItemFolderPathDisplayNarrow`) or a display name (`Folder`) of the property after the `@` prefix. This representation is supported in the -Where and -SQL parameters.

### Example 4
```powershell
PS C:\> iws -Where "@Music.Artist like '%beatles%' and @DateCreated >= '2018/01/01'"
```

This command searches for files that meet the condition that the System.Music.Artist property contains "beatles" and the created date is newer than or equal to January 1, 2018.

### Example 5
```powershell
PS C:\> iws -SQL "SELECT System.ItemName FROM SystemIndex WHERE @Music.Artist like '%beatles%' and @DateCreated >= '2018/01/01'"
```

This is an example using SQL as query language.

## Notes

This cmdlet searches only the files indexed by the Windows Search subsystem. This behavior is different from that of Windows Explorer.


## License

This module is licensed under the MIT License. See LICENSE.txt for more information.