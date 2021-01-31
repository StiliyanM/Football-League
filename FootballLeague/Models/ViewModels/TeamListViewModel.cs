namespace FootballLeague.ViewModels
{
    using FootballLeague.Common.Mapping;
    using Models;

    public class TeamListViewModel : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public int Won { get; set; }

        public int Draw { get; set; }

        public int Lost { get; set; }
    }
}
