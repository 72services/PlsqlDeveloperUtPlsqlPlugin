using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    class TestRunner
    {
        internal void Run(string type, string owner, string name, string subType, Reporter reporter)
        {
            string sql = null;

            if (type.Equals("USER"))
            {
                sql = $"select * from table(ut.run('{name}', {GetReporter(reporter)}))";
            }
            else if (type.Equals("PACKAGE"))
            {
                sql = $"select * from table(ut.run('{owner}.{name}', {GetReporter(reporter)}))";
            }
            else if (type.Equals("_ALL"))
            {
                sql = $"select * from table(ut.run('{owner}', {GetReporter(reporter)}))";
            }

            if (sql != null)
            {
                ExecuteSql(sql);
            }
        }

        internal TestSuites GetJUnitResult()
        {
            var sb = new StringBuilder();
            while (!PlsqlDeveloperUtPlsqlPlugin.sqlEof())
            {
                var value = PlsqlDeveloperUtPlsqlPlugin.sqlField(0);

                var converteredValue = Marshal.PtrToStringAnsi(value);
                sb.Append(converteredValue).Append("\r\n");

                PlsqlDeveloperUtPlsqlPlugin.sqlNext();
            }
            var result = sb.ToString();

            var serializer = new XmlSerializer(typeof(TestSuites));
            var testSuites = (TestSuites)serializer.Deserialize(new StringReader(result));
            return testSuites;
        }

        private string GetReporter(Reporter reporter)
        {
            switch (reporter)
            {
                case Reporter.JUNIT:
                    return "ut_junit_reporter()";
                default:
                    return "ut_junit_reporter()";
            }
        }

        private void ExecuteSql(string sql)
        {
            var code = PlsqlDeveloperUtPlsqlPlugin.sqlExecute(sql);
            if (code != 0)
            {
                var message = PlsqlDeveloperUtPlsqlPlugin.sqlErrorMessage();
                MessageBox.Show(Marshal.PtrToStringAnsi(message));
            }
        }

    }
}
