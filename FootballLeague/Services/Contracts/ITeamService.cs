namespace FootballLeague.Services.Contracts
{
    using FootballLeague.InputModels;
    using FootballLeague.ViewModels;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using FootballLeague.Models.Enums;

    public interface ITeamService
    {
        Task<IEnumerable<TeamListViewModel>> GetAllAsync();

        Task<TeamViewModel> CreateAsync(TeamInputModel model);

        Task<TeamViewModel> EditAsync(int id, TeamInputModel model);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task UpdateTeamInfoAsync(int teamId, MatchResult matchResult, int multiplier = 1);
    }
}
