using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using WeightTracking.Application.DTOs;
using WeightTracking.Application.Exceptions;
using WeightTracking.Application.Interfaces;
using WeightTracking.Application.Profiles;
using WeightTracking.Application.Services;
using WeightTracking.DataAccess;
using WeightTracking.DataAccess.Entities;
using WeightTracking.DataAccess.Interfaces;
using WeightTracking.DataAccess.Repository;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace WeightTracking.UnitTests.Services
{
    public class PersonServiceTests
    {
        private AppDbContext _dbContext;
        private IGenericRepository<Person> _personRepository;
        private IMapper _mapper;
        private Mock<ILogger<PersonService>> _logger;
        private IPersonService _personService;

        public PersonServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            _personRepository = new GenericRepository<Person>(_dbContext);

            _logger = new Mock<ILogger<PersonService>>();

            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<PersonProfile>(); }).CreateMapper();

            _personService = new PersonService(_personRepository, _mapper, _logger.Object);
        }

        private async Task SeedPersonAsync(int id = 1, string name = "TestName", string ownerName = "TestOwnerName", bool isPublic = false)
        {
            await _personRepository.InsertAsync(new Person
            {
                Id = id,
                Name = name,
                OwnerName = ownerName,
                IsPublic = isPublic
            });

            await _personRepository.SaveChangesAsync();
        }

        [Fact]
        public async Task GetByIdByOwnerNameAsync_WithValidOwnerAndPersonID_Should_ReturnPersonDTO()
        {
            await SeedPersonAsync(id: 1, name: "Name", ownerName: "OwnerName");

            var personDTO = await _personService.GetByIdByOwnerNameAsync("OwnerName", 1);

            Assert.NotNull(personDTO);
        }

        [Fact]
        public async Task DeleteByOwnerNameAsync_WithWrongOwner_Should_ThrowException()
        {
            await SeedPersonAsync(id: 1, ownerName: "OwnerName");

            await Assert.ThrowsAsync<AppNotFoundException>(async () =>
                await _personService.DeleteByOwnerNameAsync("OwnerName1", 1)
            );
        }

        [Theory]
        [InlineData("", "Name")]
        [InlineData(null, "Name")]
        [InlineData("OwnerName", "")]
        [InlineData("OwnerName", null)]
        public async Task CreateByOwnerNameAsync_WithInvalidOwnerOrPersonDTO_Should_ThrowException(string ownerName, string name)
        {
            var personDTO = new PersonDTO
            {
                Name = name,
                IsPublic = false
            };

            await Assert.ThrowsAsync<AppBadRequestException>(async () =>
                await _personService.CreateByOwnerNameAsync(ownerName, personDTO)
            );
        }

        [Fact]
        public async Task CreateByOwnerNameAsync_WithValidOwneAndPersonDTO_Should_ReturnTrue()
        {
            var personDTO = new PersonDTO
            {
                Name = "Name",
                IsPublic = false
            };

            await _personService.CreateByOwnerNameAsync("OwnerName", personDTO);

            var persons = await _personService.GetAllByOwnerNameAsync("OwnerName");

            Assert.True(persons.Count > 0);
        }

        [Theory]
        [InlineData("", "Name")]
        [InlineData(null, "Name")]
        [InlineData("OwnerName", "")]
        [InlineData("OwnerName", null)]
        public async Task UpdateAsync_WithInvalidOwnerOrPersonDTO_Should_ThrowException(string ownerName, string name)
        {
            var personDTO = new PersonDTO
            {
                Name = name,
                IsPublic = false
            };

            await Assert.ThrowsAsync<AppBadRequestException>(async () =>
                await _personService.UpdateAsync(ownerName, personDTO)
            );
        }
    }
}

