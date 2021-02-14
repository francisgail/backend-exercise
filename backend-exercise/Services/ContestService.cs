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
    public class ContestService: SDHttpServiceClientBase
    {
        /// <summary>
        /// The lineups resource
        /// </summary>
        private readonly string _contestResource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContestService"/> class.
        /// </summary>
        public ContestService()
        {
            _contestResource = string.Format(ConfigurationManager.AppSettings["ContestResource"],
                ConfigurationManager.AppSettings["ContestantId"]);
        }

        /// <summary>
        /// Gets the contest.
        /// </summary>
        /// <returns></returns>
        public async Task<Contest> GetContest()
        {
            var contest = await CallEndpoint<object, Contest>(HttpMethod.Get, _contestResource, null);

            return contest;
        }

        /// <summary>
        /// Assigns the total winnings.
        /// </summary>
        /// <param name="rankedLineups">The ranked lineups.</param>
        /// <param name="contest">The contest.</param>
        /// <returns></returns>
        public Lineup[] AssignTotalWinnings(Lineup[] rankedLineups, Contest contest)
        {
            //set index property
            for (var i = 0; i < rankedLineups.Length; i++)
            {
                rankedLineups[i].Index = i + 1;
            }

            //group rankedLineups by position
            var lineupsGroupedByRank =
                rankedLineups.GroupBy(
                    _ => _.Position,
                    _ => _,
                    (key, g) =>
                    {
                        var enumerable = g as Lineup[] ?? g.ToArray(); //avoid multiple enumeration
                        return new
                        {
                            Position = key,
                            Lineups = enumerable.ToList(),
                            LineupCount = enumerable.Length
                        };
                    }).ToList();

            //get the amount(s) for each grouped rank
            foreach (var rankGroup in lineupsGroupedByRank)
            {
                //get the total prize for the rank
                var totalPrize = rankGroup.Lineups.Select(lineup => contest.Prizes
                        .FirstOrDefault(_ => lineup.Index >= _.From && lineup.Index <= _.To)
                        ?.Amount)
                    .Where(prize => prize.HasValue)
                    .Aggregate<decimal?, decimal?>(0, (current, prize) => current + prize);

                if (!totalPrize.HasValue)
                {
                    continue;
                }

                foreach (var lineup in rankGroup.Lineups)
                {
                    //assign to each lineup in the group the sum of amount / lineup count 
                    lineup.TotalWinnings = (decimal)(totalPrize / rankGroup.LineupCount);
                }
            }

            //extract the lineups from the grouped data
            var lineupsToSubmit = lineupsGroupedByRank.SelectMany(_ => _.Lineups).ToArray();

            return lineupsToSubmit;
        }

        /// <summary>
        /// Assigns the ranks.
        /// </summary>
        /// <param name="lineups">The lineups.</param>
        /// <returns></returns>
        public Lineup[] AssignRanks(Lineup[] lineups)
        {
            //sort lineups by lineup points in descending order
            var sortedLineups = lineups.OrderByDescending(_ => _.Points).ToArray();

            //assign all the points into an array
            var sortedLineupsPoints = sortedLineups.Select(_ => _.Points).ToArray();

            //initialize an ranking array
            var ranks = new int[sortedLineupsPoints.Length];

            //create the rankings
            ranks[0] = 1;
            for (var i = 1; i < ranks.Length; i++)
            {
                ranks[i] = sortedLineupsPoints[i] == sortedLineupsPoints[i - 1] ? ranks[i - 1] : i + 1;
            }

            //assign the ranks the sortedLineups
            for (var i = 0; i < ranks.Length; i++)
            {
                sortedLineups[i].Position = ranks[i];
            }

            return sortedLineups;
        }
    }
}
