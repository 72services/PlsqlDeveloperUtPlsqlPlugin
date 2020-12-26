using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlsqlDeveloperUtPlsqlPlugin.Tests
{
    public class TeamCityRecordTest
    {
        [Fact]
        public void CreateTest()
        {
            var teamCityRecord = new TeamCityRecord("##teamcity[testSuiteStarted timestamp='2020-12-26T13:07:37.527+0100' name='alltests']");

            Assert.Equal("testSuiteStarted", teamCityRecord.Command);
            Assert.Equal(new DateTime(2020, 12, 26, 13, 07, 47, 527), teamCityRecord.Timestamp);
            Assert.Equal("alltests", teamCityRecord.Name);
        }
    }
}
