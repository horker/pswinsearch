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
    public class GetWindowsSearchProperty : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            var names = PropertyNameResolver.Instance.CanonicalNames;
            var displayNames = PropertyNameResolver.Instance.DisplayNames;

            for (var i = 0; i < names.Count; ++i)
            {
                var obj = new PSObject();
                var prop = new PSNoteProperty("CanonicalName", names[i]);
                obj.Properties.Add(prop);
                prop = new PSNoteProperty("DisplayName", displayNames[i]);
                obj.Properties.Add(prop);

                WriteObject(obj);
            }
        }
    }
}
