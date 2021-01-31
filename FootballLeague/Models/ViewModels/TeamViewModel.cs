namespace FootballLeague.ViewModels
{
    using FootballLeague.Common.Mapping;
    using Models;

    public class TeamViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
