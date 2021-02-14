using backend_exercise.Models;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace backend_exercise.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="backend_exercise.Services.SDHttpServiceClientBase" />
    public class PlayerScoreService: SDHttpServiceClientBase
    {
        /// <summary>
        /// The player score resource
        /// </summary>
        private readonly string _playerScoreResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScoreService"/> class.
        /// </summary>
        public PlayerScoreService()
        {
            _playerScoreResource = string.Format(ConfigurationManager.AppSettings["PlayerResource"],
                ConfigurationManager.AppSettings["ContestantId"]);
        }

        /// <summary>
        /// Gets the player scores.
        /// </summary>
        /// <returns></returns>
        public async Task<PlayerScore[]> GetPlayerScores()
        {
            var playerScores = await CallEndpoint<object, PlayerScore[]>(HttpMethod.Get, _playerScoreResource, null);

            return playerScores;
        }

        /// <summary>
        /// Sets the player score.
        /// </summary>
        /// <param name="lineups">The lineups.</param>
        /// <param name="playerScores">The player scores.</param>
        public void SetPlayerScore(Lineup[] lineups, PlayerScore[] playerScores)
        {
            //set player scores
            foreach (var lineup in lineups)
            {
                foreach (var lineupPlayer in lineup.Players)
                {
                    if (lineupPlayer.Score == 0 && lineupPlayer.Multiplier != 0)
                    {
                        var playerScore = playerScores.FirstOrDefault(_ => _.PlayerId == lineupPlayer.PlayerId)?.Score;
                        lineupPlayer.Score = playerScore * lineupPlayer.Multiplier ?? 0m;
                    }
                }
            }
        }
    }
}
