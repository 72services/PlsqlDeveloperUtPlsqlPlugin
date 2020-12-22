using System.Windows.Forms;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    public partial class About : Form
    {
        private PlsqlDeveloperUtPlsqlPlugin plugin;

        public About()
        {
            InitializeComponent();
        }

        internal void Show(PlsqlDeveloperUtPlsqlPlugin plugin)
        {
            this.plugin = plugin;
            Show();
        }
    }
}
