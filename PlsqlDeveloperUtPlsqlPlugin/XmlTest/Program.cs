using PlsqlDeveloperUtPlsqlPlugin;
using System;
using System.IO;
using System.Xml.Serialization;

namespace XmlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestSuites));

            using (Stream reader = new FileStream("c:\\Users\\simon\\Workspace72\\PlsqlDeveloperUtPlsqlPlugin\\PlsqlDeveloperUtPlsqlPlugin\\XmlTest\\XMLFile1.xml", FileMode.Open))
            {
                TestSuites testSuites = (TestSuites)serializer.Deserialize(reader);

                System.Diagnostics.Debug.WriteLine("Tests: " + testSuites.Tests);
            }
        }
    }
}
