using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlsqlDeveloperUtPlsqlPlugin
{
    class TeamCityRecord
    {
        internal string Command { get; }
        internal DateTime Timestamp { get; }
        internal string Name { get; }

        internal TeamCityRecord(string line)
        {
            // "##teamcity[testSuiteStarted timestamp='2020-12-26T13:07:37.527+0100' name='alltests']"
            string content = line.Substring(line.IndexOf("[", line.LastIndexOf("]")));


        }
    }
}
