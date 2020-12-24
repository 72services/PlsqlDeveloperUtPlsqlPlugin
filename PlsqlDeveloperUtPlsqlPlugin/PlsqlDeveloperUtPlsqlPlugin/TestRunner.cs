using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    class TestRunner
    {
        private TestResultWindow testResultWindow;

        internal void Run(string type, string owner, string name, string subType)
        {
            string sql = null;

            if (type.Equals("USER"))
            {
                sql = $"select * from table(ut.run('{name}', ut_junit_reporter()))";
            }
            else if (type.Equals("PACKAGE"))
            {
                sql = $"select * from table(ut.run('{owner}.{name}', ut_junit_reporter()))";
            }
            else if (type.Equals("_ALL"))
            {
                sql = $"select * from table(ut.run('{owner}', ut_junit_reporter()))";
            }

            if (sql != null)
            {
                ExecuteSql(sql);

                var result = GetResult();

                if (testResultWindow == null)
                {
                    testResultWindow = new TestResultWindow();
                }
                testResultWindow.Show(result);
            }
        }

        private void ExecuteSql(string sql)
        {
            int code = PlsqlDeveloperUtPlsqlPlugin.sqlExecute(sql);
            if (code != 0)
            {
                var message = PlsqlDeveloperUtPlsqlPlugin.sqlErrorMessage();
                MessageBox.Show(Marshal.PtrToStringAnsi(message));
            }
        }

        private string GetResult()
        {
            StringBuilder sb = new StringBuilder();
            while (!PlsqlDeveloperUtPlsqlPlugin.sqlEof())
            {
                var value = PlsqlDeveloperUtPlsqlPlugin.sqlField(0);

                var converteredValue = Marshal.PtrToStringAnsi(value);
                sb.Append(converteredValue).Append("\r\n");

                PlsqlDeveloperUtPlsqlPlugin.sqlNext();
            }
            return sb.ToString();
        }
    }
}
