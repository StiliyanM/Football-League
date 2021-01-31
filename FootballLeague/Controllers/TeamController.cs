namespace FootballLeague.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Services.Contracts;
    using FootballLeague.InputModels;

    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService service;

        public TeamController(ITeamService service)
        {
            this.service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await this.service.GetAllAsync();

            return this.Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(TeamInputModel model)
        {
            var result = await this.service.CreateAsync(model);

            return this.Ok(result);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditAsync(int id, TeamInputModel model)
        {
            var result = await this.service.EditAsync(id, model);

            return this.Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await this.service.DeleteAsync(id);

            return this.NoContent();
        }
    }
}
