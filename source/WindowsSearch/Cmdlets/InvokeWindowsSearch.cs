using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    [Cmdlet("Invoke", "WindowsSearch")]
    [Alias("iws")]
    [CmdletBinding(DefaultParameterSetName = "aqs")]
    public class InvokeWindowsSearch : PSCmdlet
    {
        public static readonly string[] DEFAULT_COLUMNS = new string[] {
            "System.Search.Rank",
            "System.ItemPathDisplay",
            "System.ItemFolderPathDisplay",
            "System.ItemFolderNameDisplay",
            "System.ItemName",
            "System.ItemTypeText",
            "System.DateModified",
            "System.DateCreated",
            "System.DateAccessed",
            "System.Size",
            "System.Kind"
        };

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "sql")]
        public string SQL;

        [Parameter(Position = 0, Mandatory = false, ParameterSetName = "aqs")]
        [AllowEmptyString()]
        public string Query;

        [Parameter(Position = 1, Mandatory = false, ParameterSetName = "aqs")]
        public string[] ContentProperties = new string[] { "System.FullText" };

        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "aqs")]
        public string[] SelectColumns = DEFAULT_COLUMNS;

        [Parameter(Position = 3, Mandatory = false, ParameterSetName = "aqs")]
        public string[] Sorting = new string[] { "System.Search.Rank DESC" };

        [Parameter(Position = 4, Mandatory = false, ParameterSetName = "aqs")]
        public string Where;

        [Parameter(Position = 2, Mandatory = false, ParameterSetName = "sql")]
        [Parameter(Position = 5, Mandatory = false, ParameterSetName = "aqs")]
        [Alias("MaxResults")]
        public int TotalCount = int.MaxValue;

        [Parameter(Position = 6, Mandatory = false, ParameterSetName = "aqs")]
        public SearchQueryHelper.SEARCH_QUERY_SYNTAX QuerySyntax;

        [Parameter(Position = 7, Mandatory = false, ParameterSetName = "aqs")]
        public CultureInfo ContentLocale;

        [Parameter(Position = 8, Mandatory = false, ParameterSetName = "aqs")]
        public CultureInfo KeywordLocale;

        [Parameter(Position = 9, Mandatory = false, ParameterSetName = "aqs")]
        public string[] Path;

        [Parameter(Position = 10, Mandatory = false, ParameterSetName = "aqs")]
        public string[] AdditionalColumns;

        private string[] ConvertToCanonicalNames(string[] names)
        {
            return names.Select(x => PropertyNameResolver.Instance.GetCanonicalName(x)).ToArray();
        }

        private string[] ConvertSortingToCanonicalNames(string[] names)
        {
            var results = new List<string>();
            foreach (var n in names)
            {
                var components = n.Split(new char[] { ' ', '\t' }, 2);
                var canonicalName = PropertyNameResolver.Instance.GetCanonicalName(components[0]);
                results.Add(canonicalName + ' ' + components[1]);
            }
            return results.ToArray();
        }

        protected override void BeginProcessing()
        {
            if (ParameterSetName == "aqs")
            {
                if (string.IsNullOrEmpty(Query))
                {
                    // Condition that is always true
                    Query = "System.Search.Rank:>=0";
                }

                using (var helper = new SearchQueryHelper())
                {
                    if (MyInvocation.BoundParameters.ContainsKey("AdditionalColumns"))
                        SelectColumns = SelectColumns.Concat(AdditionalColumns).ToArray();

                    var selectColumns = ConvertToCanonicalNames(SelectColumns);
                    WriteVerbose("SelectColumns: " + string.Join(",", selectColumns));
                    helper.SelectColumns = selectColumns;

                    var contentProperties = ConvertToCanonicalNames(ContentProperties);
                    WriteVerbose("ContentProperties: " + string.Join(",", contentProperties));
                    helper.ContentProperties = contentProperties;

                    var sorting = ConvertSortingToCanonicalNames(Sorting);
                    WriteVerbose("Sorting: " + string.Join(",", sorting));
                    helper.Sorting = sorting;

                    var scopeClause = "";
                    if (MyInvocation.BoundParameters.ContainsKey("Path"))
                        scopeClause = "AND (" + String.Join(" OR ", Path.Select(s => "SCOPE='" + s + "'")) + ") ";

                    if (MyInvocation.BoundParameters.ContainsKey("Where"))
                    {
                        Where = PropertyExpander.Expand(Where);
                        var m = Regex.Match(Where, @"^\s*(and|or)\b", RegexOptions.IgnoreCase);
                        if (!m.Success)
                           Where = "AND (" + Where + ")";
                    }

                    if (!string.IsNullOrEmpty(scopeClause) || !string.IsNullOrEmpty(Where))
                        helper.WhereRestrictions = scopeClause + Where;

                    if (MyInvocation.BoundParameters.ContainsKey("TotalCount"))
                        helper.MaxResults = TotalCount;

                    if (MyInvocation.BoundParameters.ContainsKey("QuerySyntax"))
                        helper.QuerySyntax = QuerySyntax;

                    if (MyInvocation.BoundParameters.ContainsKey("ContentLocale"))
                        helper.ContentLocale = ContentLocale;

                    if (MyInvocation.BoundParameters.ContainsKey("KeywordLocale"))
                        helper.KeywordLocale = KeywordLocale;

                    WriteVerbose("Query: " + Query);
                    SQL = helper.GenerateSQLFromUserQuery(Query);
                    WriteVerbose("Generated SQL: " + SQL);
                }

                using (var searcher = new Searcher())
                {
                    foreach (var result in searcher.Search(SQL))
                        WriteObject(result);
                }
            }
            else
            {
                SQL = PropertyExpander.Expand(SQL);
                WriteVerbose("SQL: " + SQL);

                using (var searcher = new Searcher())
                {
                    var count = 0;
                    foreach (var result in searcher.Search(SQL))
                    {
                        if (count >= TotalCount)
                            break;
                        WriteObject(result);
                        ++count;
                    }
                }
            }

        }
    }
}
