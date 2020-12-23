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

        internal void Show(PlsqlDeveloperUtPlsqlPlugin plugin, string text)
        {
            this.plugin = plugin;
            txtMessage.Text = text;
            Show();
        }
    }
}
