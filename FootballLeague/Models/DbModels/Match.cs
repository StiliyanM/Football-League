namespace FootballLeague.Models
{
    using FootballLeague.Common.Mapping;
    using InputModels;
    using System.ComponentModel.DataAnnotations;

    public class Match : IMapFrom<MatchInputModel>
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public Team HomeTeam { get; set; }

        public int HomeTeamId { get; set; }

        public Team AwayTeam { get; set; }

        public int AwayTeamId { get; set; }

        [Required]
        public int HomeGoals { get; set; }

        [Required]
        public int AwayGoals { get; set; }
    }
}
