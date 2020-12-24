using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    public partial class TestResultWindow : Form
    {
        public TestResultWindow()
        {
            InitializeComponent();
        }

        internal void Show(string result)
        {
            txtTests.Text = "";
            txtTime.Text = "";
            treeResult.Nodes.Clear();

            if (result != null)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                Show();

                var serializer = new XmlSerializer(typeof(TestSuites));
                var testSuites = (TestSuites)serializer.Deserialize(new StringReader(result));

                txtTests.Text = "" + testSuites.Tests;
                txtTime.Text = testSuites.Time;

                var treeNodesTestSuites = new List<TreeNode>();
                foreach (TestSuite testSuite in testSuites.TestSuite.TestSuites)
                {
                    List<TreeNode> treeNodesTestCases = new List<TreeNode>();
                    foreach (var testCase in testSuite.TestCases)
                    {
                        if (testCase.Error != null)
                        {
                            TreeNode[] treeNodesError = { new TreeNode(testCase.Error) };
                            var treeNodeTestCase = new TreeNode($"{testCase.Name}", treeNodesError);
                            treeNodeTestCase.ForeColor = Color.DarkRed;
                            treeNodesTestCases.Add(treeNodeTestCase);
                        }
                        else
                        {
                            var treeNodeTestCase = new TreeNode($"{testCase.Name}");
                            treeNodeTestCase.ForeColor = testCase.Status == null ? Color.DarkGreen : Color.DarkRed;
                            treeNodesTestCases.Add(treeNodeTestCase);
                        }
                    }
                    treeNodesTestSuites.Add(new TreeNode($"{testSuite.Name} ({testSuite.Tests})", treeNodesTestCases.ToArray()));
                }
                var root = new TreeNode($"{testSuites.TestSuite.Name} ({testSuites.TestSuite.Tests})", treeNodesTestSuites.ToArray());

                treeResult.Nodes.Add(root);
                treeResult.ExpandAll();
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void TestResultWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
