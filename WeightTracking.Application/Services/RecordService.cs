using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WeightTracking.Application.DTOs;
using WeightTracking.Application.Exceptions;
using WeightTracking.Application.Interfaces;
using WeightTracking.DataAccess.Entities;
using WeightTracking.DataAccess.Interfaces;

namespace WeightTracking.Application.Services
{
    public class RecordService : IRecordService
    {
        private IGenericRepository<Record> _recordRepository;
        private IGenericRepository<Person> _personRepository;
        private IMapper _mapper;
        private ILogger<RecordService> _logger;

        public RecordService(IGenericRepository<Record> recordRepository, IGenericRepository<Person> personRepository, IMapper mapper, ILogger<RecordService> logger)
        {
            _recordRepository = recordRepository;
            _personRepository = personRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateByPersonIdAsync(int personId, RecordDTO recordDTO)
        {
            if (personId <= 0 || recordDTO == null || recordDTO.Weigth <= 0 || recordDTO.Height <= 0)
                throw new AppBadRequestException("not validated");

            var person = await _personRepository.GetByIdAsync(personId);

            if (person == null)
                throw new AppNotFoundException("Person not found");

            var record = _mapper.Map<Record>(recordDTO);
            record.PersonId = personId;

            await _recordRepository.InsertAsync(record);
            await _recordRepository.SaveChangesAsync();

            _logger.LogInformation("RecordService | New record created {@record}", record);
        }

        public async Task DeleteByPersonIdAsync(int personId, int id)
        {
            if (personId <= 0 || id <= 0)
                throw new AppBadRequestException("not validated");

            var record = await _recordRepository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.PersonId == personId && x.Id == id);

            if (record == null)
                throw new AppNotFoundException("Record from person not found");

            _recordRepository.Delete(record);
            await _recordRepository.SaveChangesAsync();

            _logger.LogInformation("RecordService | Record deleted {@record}", record);
        }

        public async Task<List<RecordDTO>> GetAllByPersonIdAsync(int personId)
        {
            if (personId <= 0)
                throw new AppBadRequestException("not validated");

            var records = await _recordRepository
                .AsQueryable()
                .Where(x => x.PersonId == personId)
                .ToListAsync();

            return _mapper.Map<List<RecordDTO>>(records);
        }

        public async Task UpdateByPersonIdAsync(int personId, RecordDTO recordDTO)
        {
            if (personId <= 0 || recordDTO == null || recordDTO.Weigth <= 0 || recordDTO.Height <= 0)
                throw new AppBadRequestException("not validated");

            var record = await _recordRepository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.PersonId == personId && x.Id == recordDTO.Id);

            if (record == null)
                throw new AppNotFoundException("Record from person not found");

            record.Date = recordDTO.Date;
            record.Weigth = recordDTO.Weigth;
            record.Height = recordDTO.Height;

            await _recordRepository.SaveChangesAsync();

            _logger.LogInformation("RecordService | Record updated {@record}", record);
        }
    }
}

