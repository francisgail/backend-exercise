using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace backend_exercise.Models
{

    /// <summary>
    /// Lineup model class
    /// </summary>
    public class Lineup
    {
        /// <summary>
        /// Gets or sets the lineup identifier.
        /// </summary>
        /// <value>
        /// The lineup identifier.
        /// </value>
        public long LineupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public decimal Points { get; set; }

        /// <summary>
        /// Gets or sets the total winnings.
        /// </summary>
        /// <value>
        /// The total winnings.
        /// </value>
        public decimal TotalWinnings { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public Player[] Players { get; set; }

        [IgnoreDataMember]
        public int Index { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        public decimal Multiplier { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal Score { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        /// <value>
        /// The team.
        /// </value>
        public string Team { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public string Position { get; set; }
    }
}
