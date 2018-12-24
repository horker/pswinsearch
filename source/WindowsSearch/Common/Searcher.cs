using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Horker.WindowsSearch
{
    public class Searcher : IDisposable
    {
        OleDbConnection _connection;

        public Searcher()
        {
            using (var helper = new SearchQueryHelper())
            {
                _connection = new OleDbConnection(helper.ConnectionString);
            }
            _connection.Open();
        }

        public void Close()
        {
            if (_connection != null)
                _connection.Close();
        }

        public void Dispose()
        {
            Close();
        }

        public IEnumerable<PSObject> Search(string sql)
        {
            var command = new OleDbCommand(sql, _connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var obj = new PSObject();

                for (var i = 0; i < reader.VisibleFieldCount; ++i)
                {
                    var name = reader.GetName(i);
                    name = name.Replace("System.", "");
                    var value = reader.GetValue(i);

                    var prop = new PSNoteProperty(name, value);
                    obj.Properties.Add(prop);
                }

                yield return obj;
            }
        }
    }
}
