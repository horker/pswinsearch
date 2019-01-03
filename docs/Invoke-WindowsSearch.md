---
external help file: Horker.WindowsSearch.dll-Help.xml
Module Name: pswinsearch
online version:
schema: 2.0.0
---

# Invoke-WindowsSearch

## SYNOPSIS
Searches the computer using Windows Search.

## SYNTAX

### AdvancedQuerySyntax
```
Invoke-WindowsSearch [[-Query] <String>] [[-ContentProperties] <String[]>] [[-SelectColumns] <String[]>]
 [[-Sorting] <String[]>] [[-Where] <String>] [[-TotalCount] <Int32>] [[-QuerySyntax] <SEARCH_QUERY_SYNTAX>]
 [[-ContentLocale] <CultureInfo>] [[-KeywordLocale] <CultureInfo>] [[-Path] <String[]>]
 [[-AdditionalColumns] <String[]>] [-DisallowDisplayName] [<CommonParameters>]
```

### SQL
```
Invoke-WindowsSearch [-SQL] <String> [[-TotalCount] <Int32>] [<CommonParameters>]
```

## DESCRIPTION
The cmdlet makes a query to the Windows Search subsystem and gets results as PowerShell objects.

You can make a query in one of the following query languages or a combination of both.

(1) Advanced Query Syntax (AQS)

The Advanced Query Syntax is the query language that is used in the search box in Windows Explorer.

(2) Windows Search SQL

The Windows Search subsystem implements the OLE DB provider as its core query engine. By accessing it directly, you can make a search query by SQL. It offers wordy but cleaner syntax than AQS and can be written in a locale-independent manner.

For documentations on these query languages, see the RELATED LINKS section.

When you specify file properties in parameters, you can use display names (e.g., `folder`) as well as canonical names (`System.ItemFolderPathDisplayNarrow`) of properties. In addition, the `System.` prefix of property names can be omitted. You need to place `@` before property names to apply this to the -Where and -SQL parameters.

## EXAMPLES

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

## PARAMETERS

### -AdditionalColumns
Adds columns to the columns defined by the -SelectColumns parameter.

Use this parameter when you want to keep the predefined columns (See the description on -SelectColumns) and add your own columns to a result set.

```yaml
Type: String[]
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 10
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DisallowDisplayName
Disallows using display names in the following parameters: -ContentProperties, -SelectColumns, -Sorting, -Where and -SQL.

By default, display names in these parameters are converted into canonical names before processing. When this parameter is specified, they causes an error.

In the -Query parameter, display names are always allowed.

```yaml
Type: SwitchParameter
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 11
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContentLocale
Sets the locale of the query.

This value is set to the `QueryContentLocale` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: CultureInfo
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ContentProperties
Sets the properties to include in the query if search terms do not explicitly specify properties.

This value is set to the `QueryContentProperties` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: String[]
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeywordLocale
Sets the locale to use when parsing Advanced Query Syntax (AQS) keywords.

This value is set to the `QueryKeywordLocale` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: CultureInfo
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
Restricts the locations to be searched.

The search engine looks up for files in the specified directory and its subdirectories. If you limit a location to a particular directory only, use the `ItemFolderPathDisplay` or `ItemFolderPathDisplayNarrow` properties in the query (their display names are `folder path` and `folder` in the English locale).

```yaml
Type: String[]
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 9
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Query
Specifies the query in Advanced Query Syntax.

The query is converted to Windows Search SQL internally by calling the `GenerateSQLFromUserQuery` method of `ISearchQueryHelper`. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

By specifying the -Verbose parameter, you can see the SQL statement generated by the cmdlet internally. It is useful to make sure what query is actually made when you make a complicated query or get unexpected results.

```yaml
Type: String
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -QuerySyntax
Sets the syntax of the query.

This value is set to the `QuerySyntax` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: SEARCH_QUERY_SYNTAX
Parameter Sets: AdvancedQuerySyntax
Aliases:
Accepted values: NoSyntax, SEARCH_NO_QUERY_SYNTAX, SEARCH_ADVANCED_QUERY_SYNTAX, Advanced, SEARCH_NATURAL_QUERY_SYNTAX, Natural

Required: False
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SQL
Specifies the query in Windows Search SQL.

This value is directly passed to the search engine.

```yaml
Type: String
Parameter Sets: SQL
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SelectColumns
Sets the columns (or properties) requested in the SELECT statement. If not specified, the following columns are set by default:

- System.Search.Rank
- System.ItemPathDisplay
- System.ItemFolderPathDisplay
- System.ItemFolderNameDisplay
- System.ItemName
- System.ItemTypeText
- System.DateModified
- System.DateCreated
- System.DateAccessed
- System.Size
- System.Kind

These values are set to the `QuerySelectColumns` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: String[]
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Sorting
Sets the sort order for the query result set.

The value should be a string or an array of strings that contain a property name followed by the `DESC` or `ASC` keyword. For example, "System.ItemName DESC" or "DateModified ASC".

If not specified, "System.Search.Rank DESC" is used by default.

This value is set to the `QuerySorting` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: String[]
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TotalCount
Sets the maximum number of results to be returned by the query.

This value is set to the `QueryMaxResults` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases: MaxResults

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Where
Sets the restrictions appended to a query in WHERE clauses.

This value is set to the `QueryWhereRestrictions` property of `ISearchQueryHelper` that is used internally on AQS-to-SQL conversion. For more information on `ISearchQueryHelper`, see the RELATED LINKS section.

```yaml
Type: String
Parameter Sets: AdvancedQuerySyntax
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### System.Automation.Management.PSObject

## NOTES

Note that the cmdlet searches only the files indexed by the Windows Search subsystem. This behavior is different from that of Windows Explorer.

## RELATED LINKS

[Advanced Query Syntax]( https://docs.microsoft.com/ja-jp/windows/desktop/search/-search-3x-advancedquerysyntax)

[Windows Search SQL]( https://docs.microsoft.com/ja-jp/windows/desktop/search/-search-sql-windowssearch-entry)

[Windows Property System]( https://docs.microsoft.com/ja-jp/windows/desktop/properties/windows-properties-system)

[ISearchQueryHelper interface](https://docs.microsoft.com/ja-jp/windows/desktop/api/searchapi/nn-searchapi-isearchqueryhelper)