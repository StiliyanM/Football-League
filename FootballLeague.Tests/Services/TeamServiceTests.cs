namespace FootballLeague.Tests.Services
{
    using AutoMapper;
    using FootballLeague.Data.Contracts;
    using FootballLeague.InputModels;
    using FootballLeague.Models;
    using FootballLeague.Services;
    using FootballLeague.Services.Contracts;
    using FootballLeague.Tests.Helpers;
    using FootballLeague.ViewModels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class TeamServiceTests
    {
        private ITeamService service;

        private Mock<IMapper> mapper;
        private Mock<IRepository<Team>> repo;

        [TestInitialize]
        public void Setup()
        {
            this.mapper = new Mock<IMapper>();
            this.repo = new Mock<IRepository<Team>>();


            this.service = new TeamService(this.repo.Object, this.mapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task CreateAsync_GivenAlreadyExisting_ShouldThrowInvalidOperationException()
        {
            this.repo.Setup(r => r.All())
                .Returns(new List<Team> {
                    new Team
                    {
                        Name = "test"
                    }
                }.AsQueryable());


            await this.service.CreateAsync(new TeamInputModel { Name = "test" });
        }

        [TestMethod]
        public async Task CreateAsync_GivenValidData_ShouldPersistToDb()
        {
            this.repo.Setup(r => r.All())
                .Returns(new TestDbAsyncEnumerable<Team>(new List<Team>() { }));

            var result = await this.service.CreateAsync(new TeamInputModel { Name = "test" });

            this.repo.Verify(r => r.SaveAsync(It.IsAny<Team>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task EditAsync_GivenNonExistent_ShouldThrowArgumentException()
        {
            await this.service.EditAsync(5, new TeamInputModel { Name = "test" });
        }

        [TestMethod]
        public async Task EditAsync_GivenValidData_ShouldPersistToDb()
        {
            var existing = new Team
            {
                Id = 1,
                Name = "Name"
            };

            this.mapper.Setup(m => m.Map<TeamViewModel>(It.IsAny<Team>()))
                .Returns(new TeamViewModel
                {
                    Id = existing.Id,
                    Name = existing.Name
                });

            this.repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(existing));

            var result = await this.service.EditAsync(1, new TeamInputModel { Name = "test" });

            this.repo.Verify(r => r.SaveAsync(It.IsAny<Team>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task DeleteAsync_GivenNonExistent_ShouldThrowArgumentException()
        {

            await this.service.DeleteAsync(5);
        }

        [TestMethod]
        public async Task DeleteAsync_GivenValidData_ShouldDeleteEntity()
        {
            var existing = new Team
            {
                Id = 1,
                Name = "Name"
            };

            this.repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(existing));

            await this.service.DeleteAsync(1);

            this.repo.Verify(r => r.Delete(It.IsAny<Team>()), Times.Once);
            this.repo.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
