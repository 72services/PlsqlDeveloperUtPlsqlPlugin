using System.Windows.Forms;
using System.Xml.Linq;

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

            string sql = $"select * from table(ut.run('{owner}.{name}', ut_junit_reporter()))";
            txtQuery.Text = sql;

            CenterToScreen();

            Show();

            this.plugin.ExecuteSql(sql);

            string result = this.plugin.GetResult();

            XDocument document = XDocument.Parse(result);

            txtResult.Text = document.ToString();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
