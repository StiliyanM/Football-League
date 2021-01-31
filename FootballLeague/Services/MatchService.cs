namespace FootballLeague.Services
{
    using AutoMapper;
    using Contracts;
    using FootballLeague.Common;
    using FootballLeague.Data.Contracts;
    using FootballLeague.InputModels;
    using FootballLeague.Models;
    using FootballLeague.Models.Enums;
    using FootballLeague.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MatchService : BaseService, IMatchService
    {
        private readonly IRepository<Match> repo;
        private readonly ITeamService teamService;

        public MatchService(IRepository<Match> repo, IMapper mapper, ITeamService teamService)
            : base(mapper)
        {
            this.repo = repo;
            this.teamService = teamService;
        }

        public async Task<IEnumerable<MatchListViewModel>> GetAllAsync()
        {
            var matches = this.repo.All();

            return await this.mapper.ProjectTo<MatchListViewModel>(matches).ToListAsync();
        }

        public async Task<MatchViewModel> CreateAsync(MatchInputModel model)
        {
            await this.EnsureMatchIsValidAsync(model.HomeTeamId, model.AwayTeamId);

            var match = this.mapper.Map<Match>(model);

            await this.UpdateTeamStatisticsAsync(model.HomeTeamId, model.HomeGoals, model.AwayTeamId, model.AwayGoals);

            var result = await this.repo.SaveAsync(match);

            return this.mapper.Map<MatchViewModel>(result);
        }

        public async Task<MatchViewModel> EditAsync(int id, MatchInputModel model)
        {
            var match = await this.GetByIdAsync(id);

            await this.EnsureMatchIsValidAsync(model.HomeTeamId, model.AwayTeamId);

            // remove previous statistics

            await this.UpdateTeamStatisticsAsync(match.HomeTeamId, match.HomeGoals, match.AwayTeamId, match.AwayGoals, -1);

            // add new statistics

            match.HomeTeamId = model.HomeTeamId;
            match.HomeGoals = model.HomeGoals;
            match.AwayTeamId = model.AwayTeamId;
            match.AwayGoals = model.AwayGoals;

            await this.UpdateTeamStatisticsAsync(match.HomeTeamId, match.HomeGoals, match.AwayTeamId, match.AwayGoals);

            var result = await this.repo.SaveAsync(match);

            return this.mapper.Map<MatchViewModel>(result);
        }

        public async Task DeleteAsync(int id)
        {
            var match = await this.GetByIdAsync(id);

            await this.UpdateTeamStatisticsAsync(match.HomeTeamId, match.HomeGoals, match.AwayTeamId, match.AwayGoals, -1);

            this.repo.Delete(match);

            await this.repo.SaveAsync();
        }

        /// <summary>
        ///  Update both team statistics(points, won, draw and lost info) after adding, editing or deleting a match.
        /// </summary>
        /// <param name="homeTeamId"></param>
        /// <param name="homeGoals"></param>
        /// <param name="awayTeamId"></param>
        /// <param name="awayGoals"></param>
        /// <param name="amplifier">Either 1 or -1. If negative it means that the match has either been edited or deleted, so we have to undo the points added to the respective teams</param>
        /// <returns></returns>
        private async Task UpdateTeamStatisticsAsync(int homeTeamId, int homeGoals, int awayTeamId, int awayGoals, int amplifier = 1)
        {
            var homeTeamPoints = this.GetTeamMatchResult(homeGoals, awayGoals);

            await this.teamService.UpdateTeamInfoAsync(homeTeamId, homeTeamPoints, amplifier);

            var awayTeamPoints = this.GetTeamMatchResult(awayGoals, homeGoals);

            await this.teamService.UpdateTeamInfoAsync(awayTeamId, awayTeamPoints, amplifier);
        }

        private MatchResult GetTeamMatchResult(int teamGoals, int opponentGoals)
        {
            var hasWinner = teamGoals != opponentGoals;

            if (hasWinner)
            {
                return teamGoals > opponentGoals ? MatchResult.Won : MatchResult.Lost;
            }

            return MatchResult.Draw;
        }

        private async Task EnsureMatchIsValidAsync(int homeTeamId, int awayTeamId)
        {
            var homeTeamExists = await this.teamService.ExistsAsync(homeTeamId);

            if (!homeTeamExists)
            {
                throw new ArgumentException(string.Format(Constants.ENTITY_DOES_NOT_EXIST, Constants.HOME_TEAM));
            }

            var awayTeamExists = await this.teamService.ExistsAsync(awayTeamId);

            if (!awayTeamExists)
            {
                throw new ArgumentException(string.Format(Constants.ENTITY_DOES_NOT_EXIST, Constants.AWAY_TEAM));
            }

            if (homeTeamId == awayTeamId)
            {
                throw new InvalidOperationException(Constants.HOME_AWAY_TEAM_CANNOT_BE_SAME);
            }
        }

        private async Task<Match> GetByIdAsync(int id)
        {
            var match = await this.repo.GetByIdAsync(id);

            this.EnsureExists(match, nameof(Match));

            return match;
        }
    }
}
