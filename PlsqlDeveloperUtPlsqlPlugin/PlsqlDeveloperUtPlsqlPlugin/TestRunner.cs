using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    public partial class TestRunner : Form
    {
        private PlsqlDeveloperUtPlsqlPlugin plugin;

        public TestRunner()
        {
            InitializeComponent();
        }

        internal void Show(PlsqlDeveloperUtPlsqlPlugin plugin, string type, string owner, string name, string subType)
        {
            this.plugin = plugin;

            string sql = null;

            if (owner != null && name != null)
            {
                sql = $"select * from table(ut.run('{owner}.{name}', ut_junit_reporter()))";
            }
            else if (owner != null)
            {
                sql = $"select * from table(ut.run('{owner}', ut_junit_reporter()))";
            }

            if (sql != null)
            {
                txtQuery.Text = sql;

                CenterToScreen();
                
                Show();

                this.plugin.ExecuteSql(sql);

                string result = this.plugin.GetResult();

                XDocument document = XDocument.Parse(result);
                txtResult.Text = document.ToString();

                XmlSerializer serializer = new XmlSerializer(typeof(TestSuites));
                TestSuites testSuites = (TestSuites)serializer.Deserialize(new StringReader(document.ToString()));

                MessageBox.Show($"# Time : {testSuites.Time}");
            }
            else
            {
                txtQuery.Text = "";
                txtResult.Text = "";
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
