using backend_exercise.Models;
using backend_exercise.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace backend_exercise_test
{
    [TestClass]
    public class TestPlayerScoreService
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestFiles\\TestPlayerScore.xml", "Row", DataAccessMethod.Sequential),
         TestMethod]
        public void TestSetPlayerScore()
        {
            var playerScoreService = new PlayerScoreService();

            var testDataJson = TestContext.DataRow["TestData"].ToString();
            var expectedResultJson = TestContext.DataRow["ExpectedResult"].ToString();
            var playerScoresJson = TestContext.DataRow["PlayerScores"].ToString();

            var testLineUps = JsonConvert.DeserializeObject<Lineup[]>(testDataJson);
            var expectedLineUp = JsonConvert.DeserializeObject<Lineup[]>(expectedResultJson);
            var playerScores = JsonConvert.DeserializeObject<PlayerScore[]>(playerScoresJson);

            playerScoreService.SetPlayerScore(testLineUps, playerScores);

            for (var i = 0; i < testLineUps[0].Players.Length; i++)
            {
                Assert.AreEqual(expectedLineUp[0].Players[i].Score, testLineUps[0].Players[i].Score);
            }
        }
    }
}
