namespace FootballLeague.ViewModels
{
    using AutoMapper;
    using Common.Mapping;
    using FootballLeague.Models;

    public class MatchListViewModel : IHaveCustomMapping
    {
        public int Id { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            mapper.CreateMap<Match, MatchListViewModel>()
                .ForMember(dest => dest.HomeTeam, cfg => cfg.MapFrom(src => src.HomeTeam.Name))
                .ForMember(dest => dest.AwayTeam, cfg => cfg.MapFrom(src => src.AwayTeam.Name));
        }
    }
}
