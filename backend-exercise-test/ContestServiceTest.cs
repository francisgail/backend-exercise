using System.Linq;
using backend_exercise.Models;
using backend_exercise.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace backend_exercise_test
{
    [TestClass]
    public class ContestServiceTest
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestFiles\\TestContest.xml", "Rank", DataAccessMethod.Sequential), 
          TestMethod]
        public void TestAssignRanks()
        {
            var contestService = new ContestService();

            var testDataJson = TestContext.DataRow["TestData"].ToString();
            var expectedResultJson = TestContext.DataRow["ExpectedResult"].ToString();

            var testLineUp = JsonConvert.DeserializeObject<Lineup[]>(testDataJson);
            var expectedLineUp = JsonConvert.DeserializeObject<Lineup[]>(expectedResultJson);

            var rankedLineups = contestService.AssignRanks(testLineUp);

            foreach (var rankedLineup in rankedLineups)
            {
                Assert.AreEqual(expectedLineUp.First(_ => _.LineupId == rankedLineup.LineupId).Position, rankedLineup.Position);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestFiles\\TestContest.xml", "TotalWinning", DataAccessMethod.Sequential),
         TestMethod]
        public void TestAssignTotalWinnings()
        {
            var contestService = new ContestService();

            var testDataJson = TestContext.DataRow["TestData"].ToString();
            var expectedResultJson = TestContext.DataRow["ExpectedResult"].ToString();
            var contestJson = TestContext.DataRow["Contest"].ToString();
            
            var testLineUp = JsonConvert.DeserializeObject<Lineup[]>(testDataJson);
            var expectedLineUp = JsonConvert.DeserializeObject<Lineup[]>(expectedResultJson);
            var contest = JsonConvert.DeserializeObject<Contest>(contestJson);

            var lineupWithTotalWinnings = contestService.AssignTotalWinnings(testLineUp, contest);

            foreach (var lineupWithTotalWinning in lineupWithTotalWinnings)
            {
                Assert.AreEqual(expectedLineUp.First(_ => _.LineupId == lineupWithTotalWinning.LineupId).TotalWinnings, lineupWithTotalWinning.TotalWinnings);
            }
        }
    }
}
