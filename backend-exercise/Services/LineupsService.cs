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
    public class LineupsService : SDHttpServiceClientBase
    {
        /// <summary>
        /// The lineups resource
        /// </summary>
        private readonly string _lineupsResource;

        /// <summary>
        /// The submit scored lineup resource
        /// </summary>
        private readonly string _submitScoredLineupResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineupsService" /> class.
        /// </summary>
        public LineupsService()
        {
            _lineupsResource = string.Format(ConfigurationManager.AppSettings["LineupsResource"],
                ConfigurationManager.AppSettings["ContestantId"]);

            _submitScoredLineupResource = string.Format(ConfigurationManager.AppSettings["SubmitScoredLineupResource"],
                ConfigurationManager.AppSettings["ContestantId"]);
        }

        /// <summary>
        /// Gets the lineups.
        /// </summary>
        /// <returns></returns>
        public async Task<Lineup[]> GetLineups()
        {
            var lineups = await CallEndpoint<object, Lineup[]>(HttpMethod.Get, _lineupsResource, null);

            return lineups;
        }

        /// <summary>
        /// Submits the scored lineup.
        /// </summary>
        /// <param name="lineups">The lineups.</param>
        /// <returns></returns>
        public async Task<string> SubmitScoredLineup(Lineup[] lineups)
        {
            var result = await CallEndpoint<Lineup[], string>(HttpMethod.Put, _submitScoredLineupResource, lineups);

            return result ?? "Process Complete."; //null content on 204 result.
        }

        /// <summary>
        /// Updates the lineup points.
        /// </summary>
        /// <param name="lineups">The lineups.</param>
        public void UpdateLineupPoints(Lineup[] lineups)
        {
            //update lineup points
            foreach (var lineup in lineups)
            {
                lineup.Points = lineup.Players.Sum(_ => _.Score);
            }
        }
    }
}
