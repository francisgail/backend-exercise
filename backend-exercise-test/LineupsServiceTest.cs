using backend_exercise.Models;
using backend_exercise.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace backend_exercise_test
{
    [TestClass]
    public class LineupsServiceTest
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "|DataDirectory|\\TestFiles\\TestUpdateLineupPoints.xml", "Row", DataAccessMethod.Sequential), 
          TestMethod]
        public void TestUpdateLineupPoints()
        {
            var lineupService = new LineupsService();

            var testDataJson = TestContext.DataRow["TestData"].ToString();
            var expectedResultJson = TestContext.DataRow["ExpectedResult"].ToString();

            var testLineUp = JsonConvert.DeserializeObject<Lineup[]>(testDataJson);
            var expectedLineUp = JsonConvert.DeserializeObject<Lineup[]>(expectedResultJson);
            
            lineupService.UpdateLineupPoints(testLineUp);

            Assert.AreEqual(expectedLineUp[0].Points, testLineUp[0].Points);
        }
    }
}
