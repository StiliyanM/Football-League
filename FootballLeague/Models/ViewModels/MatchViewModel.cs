namespace FootballLeague.ViewModels
{
    using FootballLeague.Common.Mapping;
    using Models;

    public class MatchViewModel : IMapFrom<Match>
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }
    }
}
