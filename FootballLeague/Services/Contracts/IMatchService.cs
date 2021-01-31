namespace FootballLeague.Services.Contracts
{
    using FootballLeague.InputModels;
    using System.Threading.Tasks;
    using ViewModels;
    using System.Collections.Generic;

    public interface IMatchService
    {
        Task<IEnumerable<MatchListViewModel>> GetAllAsync();

        Task<MatchViewModel> CreateAsync(MatchInputModel model);

        Task<MatchViewModel> EditAsync(int id, MatchInputModel model);

        Task DeleteAsync(int id);
    }
}
