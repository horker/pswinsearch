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
    [Cmdlet("Find", "FileInIndex")]
    [Alias("ffi")]
    public class FindFileInIndex : PSCmdlet
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

        [Parameter(Position = 0, Mandatory = false)]
        public string Query = "";

        [Parameter(Position = 1, Mandatory = false)]
        public string[] SelectColumns = DEFAULT_COLUMNS;

        [Parameter(Position = 2, Mandatory = false)]
        [AllowEmptyCollection()]
        public string[] Sorting = new string[] { "System.Search.Rank DESC" };

        [Parameter(Position = 3, Mandatory = false)]
        public int TotalCount;

        [Parameter(Position = 4, Mandatory = false)]
        public string[] Path;

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
            var conditionClause = "";
            if (!string.IsNullOrEmpty(Query))
            {
                var tokenizer = new Tokenizer(Query);
                var parser = new Parser(tokenizer);
                var root = parser.Parse();
                var codeGenerator = new CodeGenerator(root);
                conditionClause = codeGenerator.Generate();
            }

            var columnsClause = "\"" + string.Join("\", \"", ConvertToCanonicalNames(SelectColumns)) + "\"";

            var sortingClause = string.Join(", ", ConvertSortingToCanonicalNames(Sorting));

            var topClause = "";
            if (MyInvocation.BoundParameters.ContainsKey("TotalCount"))
                topClause = "TOP " + TotalCount + " ";

            var scopeClause = "";
            if (MyInvocation.BoundParameters.ContainsKey("Path"))
                scopeClause = "(" + String.Join(" OR ", Path.Select(s => "SCOPE='file:" + s + "'")) + ")";

            var whereClause = "";
            if (!string.IsNullOrEmpty(conditionClause))
            {
                if (!string.IsNullOrEmpty(scopeClause))
                    whereClause = " WHERE " + conditionClause + " AND " + scopeClause;
                else
                    whereClause = " WHERE " + conditionClause;
            }
            else
            {
                if (!string.IsNullOrEmpty(scopeClause))
                    whereClause = " WHERE " + scopeClause;
            }

            var statement = "SELECT " + topClause + columnsClause + " FROM SystemIndex" + whereClause;
            WriteVerbose(statement);

            using (var searcher = new Searcher())
            {
                foreach (var result in searcher.Search(statement))
                    WriteObject(result);
            }
        }
    }
}
