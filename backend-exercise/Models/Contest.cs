using System;

namespace backend_exercise.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Contest
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sport identifier.
        /// </summary>
        /// <value>
        /// The sport identifier.
        /// </value>
        public int SportId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the season.
        /// </summary>
        /// <value>
        /// The season.
        /// </value>
        public int Season { get; set; }

        /// <summary>
        /// Gets or sets the entry fee.
        /// </summary>
        /// <value>
        /// The entry fee.
        /// </value>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        /// <value>
        /// The entries.
        /// </value>
        public int Entries { get; set; }

        /// <summary>
        /// Gets or sets the maximum entries.
        /// </summary>
        /// <value>
        /// The maximum entries.
        /// </value>
        public int MaxEntries { get; set; }

        /// <summary>
        /// Gets or sets the prize pool.
        /// </summary>
        /// <value>
        /// The prize pool.
        /// </value>
        public decimal PrizePool { get; set; }

        /// <summary>
        /// Gets or sets the prizes.
        /// </summary>
        /// <value>
        /// The prizes.
        /// </value>
        public Prize[] Prizes { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Prize
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public int From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public int To { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the package total.
        /// </summary>
        /// <value>
        /// The package total.
        /// </value>
        public decimal PackageTotal { get; set; }
    }
}
