using backend_exercise.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend_exercise
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            try
            {
                Task.Run(async () => { await MainAsync(args); }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Mains the asynchronous.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static async Task MainAsync(string[] args)
        {
            //Get data and validate
            var lineupService = new LineupsService();
            var lineups = await lineupService.GetLineups();
            if (lineups == null || !lineups.Any())
            {
                Console.WriteLine("No lineups to process.");
                return;
            }
            
            var playerScoreService = new PlayerScoreService();
            var playerScores = await playerScoreService.GetPlayerScores();
            if (playerScores == null || !playerScores.Any())
            {
                Console.WriteLine("No player scores to process.");
                return;
            }

            var contestService = new ContestService();
            var contest = await contestService.GetContest();
            if (contest?.Prizes == null || !contest.Prizes.Any())
            {
                Console.WriteLine("No contest data to process.");
                return;
            }

            //set the player scores
            playerScoreService.SetPlayerScore(lineups, playerScores);

            //update the points per lineup
            lineupService.UpdateLineupPoints(lineups);

            //create a new lineup collection with assigned ranks
            var rankedLineups = contestService.AssignRanks(lineups);
            
            //assign total winnings and create the lineups for submissions
            var lineupsToSubmit = contestService.AssignTotalWinnings(rankedLineups, contest);
            
            //submit the score lineups
            var result = await lineupService.SubmitScoredLineup(lineupsToSubmit);

            Console.WriteLine(result);
        }
    }
}
