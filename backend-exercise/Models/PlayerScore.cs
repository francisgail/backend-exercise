namespace backend_exercise.Models
{
    /// <summary>
    /// Model class for play score
    /// </summary>
    public class PlayerScore
    {
        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public long PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal Score { get; set; }
    }
}
