namespace FootballLeague.Controllers
{
    using FootballLeague.InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService service;

        public MatchController(IMatchService service)
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
        public async Task<IActionResult> CreateAsync(MatchInputModel model)
        {
            var result = await this.service.CreateAsync(model);

            return this.Ok(result);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditAsync(int id, MatchInputModel model)
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
