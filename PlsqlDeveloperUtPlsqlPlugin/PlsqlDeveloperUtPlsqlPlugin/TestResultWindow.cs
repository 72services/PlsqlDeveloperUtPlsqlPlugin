using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    public partial class TestResultWindow : Form
    {
        private TestRunner testRunner = new TestRunner();

        public TestResultWindow()
        {
            InitializeComponent();
        }

        internal void RunTests(string type, string owner, string name, string subType)
        {
            testRunner.Run(type, owner, name, subType, Reporter.JUNIT);
            ShowResult();
        }
        private void ShowResult()
        {
            txtTests.Text = "";
            txtTime.Text = "";
            treeResult.Nodes.Clear();

            var testSuites = testRunner.GetJunitResult();

            if (testSuites != null)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                Show();

                txtTests.Text = "" + testSuites.Tests;
                txtTime.Text = testSuites.Time;

                var treeNodesTestSuites = new List<TreeNode>();
                foreach (TestSuite testSuite in testSuites.TestSuite.TestSuites)
                {
                    var treeNodesTestCases = new List<TreeNode>();
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
