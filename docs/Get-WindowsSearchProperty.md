---
external help file: Horker.WindowsSearch.dll-Help.xml
Module Name: pssearch
online version:
schema: 2.0.0
---

# Get-WindowsSearchProperty

## SYNOPSIS
Gets properties of the Windows Property System.

## SYNTAX

```
Get-WindowsSearchProperty [[-Name] <String[]>] [[-DisplayName] <String[]>] [<CommonParameters>]
```

## DESCRIPTION
The cmdlet returns properties of the Windows Property System.

The Windows Property System is one of the subsystems of Windows. It defines a set of information of file properties used in system applications including Windows Explorer. Examples of such properties are a file name (`System.ItemName`), a file size (`System.Size`) and a last modified date (`System.DateModified`).

Several of the properties are specific to particular types of files. For example, audio files have the sample rate property (`System.Audio.SampleRate`).

Each property has two types of names, a canonical name and a display name. Canonical names are used to uniquely identify properties. Display names are human-readable and used as titles when properties are displayed in applications. For example, the property that represents a file name has a canonical name `System.ItemName` and a display name `Name` (in the English locale). Display names are locale-specific and thus vary depending on the language of the system.

You can see properties for a file by right-clicking the file icon, selecting `Properties` from the context menu and choosing the `Details` tab in Windows Explorer.

For more information, see the RELATED LINKS section.

When the cmdlet is invoked without parameters, it returns all properties defined in the system. When the `-Name` parameter is specified, it returns properties that contain it in their canonical names. When the `-DisplayName` parameter is specified, it returns properties that contain it in their display names.

## EXAMPLES

### Example 1
```powershell
PS C:\> Get-WindowsSearchProperty
```

Returns all properties defined in the system.

### Example 2
```powershell
PS C:\> Get-WindowsSearchProperty -Name PathName
```

Returns properties that contains `PathName` in their canonical names.

## PARAMETERS

### -DisplayName
When specified, the cmdlet returns properties that contains the -DisplayName value in their display names.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
When specified, the cmdlet returns properties that contains the -Name value in their canonical names.

```yaml
Type: String[]
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
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

## RELATED LINKS

[Windows Property System]( https://docs.microsoft.com/ja-jp/windows/desktop/properties/windows-properties-system)