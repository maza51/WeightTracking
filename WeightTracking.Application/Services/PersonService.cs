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
    public class PersonService : IPersonService
    {
        private IGenericRepository<Person> _personRepository;
        private IMapper _mapper;
        ILogger<PersonService> _logger;

        public PersonService(IGenericRepository<Person> personRepository, IMapper mapper, ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PersonDTO> GetByIdByOwnerNameAsync(string owner, int personId)
        {
            if (personId <= 0 || string.IsNullOrEmpty(owner))
                throw new AppBadRequestException("not validated");

            var person = await _personRepository
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OwnerName == owner && x.Id == personId);

            if (person == null)
                throw new AppNotFoundException("Person from owner not found");

            return _mapper.Map<PersonDTO>(person);
        }

        public async Task CreateByOwnerNameAsync(string owner, PersonDTO personDTO)
        {
            if (personDTO == null || string.IsNullOrEmpty(personDTO.Name) || string.IsNullOrEmpty(owner))
                throw new AppBadRequestException("not validated");

            var person = _mapper.Map<Person>(personDTO);
            person.OwnerName = owner;

            await _personRepository.InsertAsync(person);
            await _personRepository.SaveChangesAsync();

            _logger.LogInformation("PersonService | New person created {@person}", person);
        }

        public async Task DeleteByOwnerNameAsync(string owner, int personId)
        {
            if (string.IsNullOrEmpty(owner) || personId <= 0)
                throw new AppBadRequestException("not validated");

            var person = await _personRepository
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OwnerName == owner && x.Id == personId);

            if (person == null)
                throw new AppNotFoundException("Person from owner not found");

            _personRepository.Delete(person);
            await _personRepository.SaveChangesAsync();

            _logger.LogInformation("PersonService | Person deleted {@person}", person);
        }

        public async Task<List<PersonDTO>> GetAllByOwnerNameAsync(string owner)
        {
            if (string.IsNullOrEmpty(owner))
                throw new AppBadRequestException("not validated");

            var persons = await _personRepository.AsQueryable()
                .Where(x => x.OwnerName == owner)
                //.Include(x => x.Records)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<PersonDTO>>(persons);
        }

        public async Task<List<PersonDTO>> GetAllPublicAsync()
        {
            var persons = await _personRepository.AsQueryable()
                .Where(x => x.IsPublic == true)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<PersonDTO>>(persons);
        }

        public async Task UpdateAsync(string owner, PersonDTO personDTO)
        {
            if (personDTO == null || string.IsNullOrEmpty(personDTO.Name) || string.IsNullOrEmpty(owner))
                throw new AppBadRequestException("not validated");

            var personInDb = await _personRepository
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.OwnerName == owner && x.Id == personDTO.Id);

            if (personInDb == null)
                throw new AppNotFoundException("Person from owner not found");

            personInDb.Name = personDTO.Name;
            personInDb.IsPublic = personDTO.IsPublic;

            await _personRepository.SaveChangesAsync();

            _logger.LogInformation("PersonService | Person updated {@personInDb}", personInDb);
        }
    }
}

