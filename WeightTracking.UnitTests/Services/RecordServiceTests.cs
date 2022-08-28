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
using Record = WeightTracking.DataAccess.Entities.Record;

namespace WeightTracking.UnitTests.Services
{
    public class RecordServiceTests
    {
        private AppDbContext _dbContext;
        private IGenericRepository<Record> _recordRepository;
        private IGenericRepository<Person> _personRepository;
        private IMapper _mapper;
        private Mock<ILogger<RecordService>> _logger;
        private IRecordService _recordService;

        public RecordServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);

            _recordRepository = new GenericRepository<Record>(_dbContext);
            _personRepository = new GenericRepository<Person>(_dbContext);

            _logger = new Mock<ILogger<RecordService>>();

            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<RecordProfile>(); }).CreateMapper();

            _recordService = new RecordService(_recordRepository, _personRepository, _mapper, _logger.Object);
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

        private async Task SeedRecordAsync(int id = 1, float height = 1, float weight = 1, int personId = 1)
        {
            await _recordRepository.InsertAsync(new Record
            {
                Id = id,
                Date = DateTime.Now,
                Height = height,
                Weigth = weight,
                PersonId = personId
            });

            await _recordRepository.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateByPersonIdAsync_WithValidPersonIdAndRecordDTO_Should_ReturnTrue()
        {
            await SeedPersonAsync(id: 1);

            var recordDTO = new RecordDTO
            {
                Date = DateTime.Now,
                Height = 1,
                Weigth = 1
            };

            await _recordService.CreateByPersonIdAsync(1, recordDTO);

            var record = await _recordService.GetAllByPersonIdAsync(1);

            Assert.True(record.Count > 0);
        }

        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(-1, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, -1, 1)]
        [InlineData(1, 1, 0)]
        [InlineData(1, 1, -1)]
        public async Task CreateByPersonIdAsync_WitInvalidPersonIdOrRecordDTO_Should_ThrowException(int personId, int height, int weight)
        {
            await SeedPersonAsync(id: 1);

            var recordDTO = new RecordDTO
            {
                Date = DateTime.Now,
                Height = height,
                Weigth = weight
            };

            await Assert.ThrowsAsync<AppBadRequestException>(async () =>
                await _recordService.CreateByPersonIdAsync(personId, recordDTO)
            );
        }

        [Fact]
        public async Task GetAllByPersonIdAsync_WithValidPersonId_Should_ReturnTrue()
        {
            await SeedPersonAsync(id: 1);

            await SeedRecordAsync(personId: 1);

            var personsDTO = await _recordService.GetAllByPersonIdAsync(1);

            Assert.True(personsDTO.Count > 0);
        }
    }
}