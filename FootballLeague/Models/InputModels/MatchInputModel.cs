namespace FootballLeague.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class MatchInputModel
    {
        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        [Required]
        public int HomeGoals { get; set; }

        [Required]
        public int AwayGoals { get; set; }
    }
}
