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
    [Cmdlet("Get", "WindowsSearchProperty")]
    [OutputType(typeof(PSObject))]
    public class GetWindowsSearchProperty : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        public string[] Name = new string[0];

        [Parameter(Position = 1, Mandatory = false)]
        public string[] DisplayName = new string[0];

        protected override void BeginProcessing()
        {
            var propertyNames = PropertyNameResolver.Instance.CanonicalNames;
            var displayNames = PropertyNameResolver.Instance.DisplayNames;

            var propertyRegex = new Regex(string.Join("|", Name.Select(x => Regex.Escape(x))), RegexOptions.IgnoreCase);
            var displayNameRegex = new Regex(string.Join("|", DisplayName.Select(x => Regex.Escape(x))), RegexOptions.IgnoreCase);

            for (var i = 0; i < propertyNames.Count; ++i)
            {
                var p = propertyNames[i];
                var d = displayNames[i];

                var show = true;
                if (Name.Length > 0)
                    show = propertyRegex.Match(p).Success;
                if (DisplayName.Length > 0)
                    show = displayNameRegex.Match(d).Success;
                if (!show)
                    continue;

                var obj = new PSObject();
                var prop = new PSNoteProperty("CanonicalName", propertyNames[i]);
                obj.Properties.Add(prop);
                prop = new PSNoteProperty("DisplayName", displayNames[i]);
                obj.Properties.Add(prop);

                WriteObject(obj);
            }
        }
    }
}
