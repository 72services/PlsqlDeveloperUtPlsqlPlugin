using System;
using System.Collections.Generic;
using System.Drawing;
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
            try
            {
                this.plugin = plugin;

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
                    CenterToScreen();
                    Show();

                    this.plugin.ExecuteSql(sql);

                    var result = this.plugin.GetResult();

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
                else
                {
                    txtTests.Text = "";
                    txtTime.Text = "";
                    treeResult.Nodes.Clear();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }


    }
}
