namespace FootballLeague.Services
{
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using FootballLeague.Common;
    using FootballLeague.Data.Contracts;
    using FootballLeague.Models;
    using System;
    using System.Threading.Tasks;
    using FootballLeague.ViewModels;
    using FootballLeague.InputModels;
    using System.Collections.Generic;
    using AutoMapper;
    using FootballLeague.Models.Enums;

    public class TeamService : BaseService, ITeamService
    {
        private readonly IRepository<Team> repo;

        public TeamService(IRepository<Team> repo, IMapper mapper)
            : base(mapper)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<TeamListViewModel>> GetAllAsync()
        {
            var teams = this.repo.All();

            return await this.mapper.ProjectTo<TeamListViewModel>(teams).ToListAsync();
        }

        public async Task<TeamViewModel> CreateAsync(TeamInputModel model)
        {
            var existing = await this.repo.All()
                .FirstOrDefaultAsync(t => t.Name == model.Name);

            if (existing != null)
            {
                throw new InvalidOperationException(string.Format(Constants.ENTITY_ALREADY_EXISTS, nameof(Team)));
            }

            var result = await this.repo.SaveAsync(new Team
            {
                Name = model.Name
            });

            return this.mapper.Map<TeamViewModel>(result);
        }

        public async Task<TeamViewModel> EditAsync(int id, TeamInputModel model)
        {
            var team = await this.GetByIdAsync(id);

            team.Name = model.Name;

            var result = await this.repo.SaveAsync(team);

            return this.mapper.Map<TeamViewModel>(result);
        }

        public async Task DeleteAsync(int id)
        {
            var team = await this.GetByIdAsync(id);

            this.repo.Delete(team);

            await this.repo.SaveAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var team = await this.repo.GetByIdAsync(id);

            return team != null;
        }

        /// <summary>
        ///  Update team info after a match has been added/edited/deleted
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="matchResult"></param>
        /// <param name="multiplier">if positive we are adding new data, otherwise we are reverting previously added data</param>
        /// <returns></returns>
        public async Task UpdateTeamInfoAsync(int teamId, MatchResult matchResult, int multiplier = 1)
        {
            var team = await this.GetByIdAsync(teamId);

            switch (matchResult)
            {
                case MatchResult.Lost:
                    team.Lost += multiplier;
                    break;
                case MatchResult.Draw:
                    team.Draw += multiplier;
                    break;
                case MatchResult.Won:
                    team.Won += multiplier;
                    break;
                default:
                    break;
            }

            team.Points += (int)matchResult * multiplier;

            await this.repo.SaveAsync(team);
        }

        private async Task<Team> GetByIdAsync(int id)
        {
            var team = await this.repo.GetByIdAsync(id);

            this.EnsureExists(team, nameof(Team));

            return team;
        }

    }
}
