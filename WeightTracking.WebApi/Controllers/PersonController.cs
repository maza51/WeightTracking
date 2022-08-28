using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeightTracking.Application.DTOs;
using WeightTracking.Application.Interfaces;
using WeightTracking.WebApi.ViewModels;

namespace WeightTracking.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private IPersonService _personService;
        private IMapper _mapper;
        private ILogger<PersonController> _logger;

        public PersonController(IPersonService personService, IMapper mapper, ILogger<PersonController> logger)
        {
            _personService = personService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int count)
        {
            var ownerName = HttpContext.User.Identity?.Name;

            _logger.LogInformation($"Getting all person by owner {ownerName}");

            var persons = await _personService.GetAllByOwnerNameAsync(ownerName);

            return Ok(persons);
        }

        [HttpGet]
        [Route("{personId:int}")]
        public async Task<IActionResult> GetBlyId(int personId)
        {
            var ownerName = HttpContext.User.Identity?.Name;

            var person = await _personService.GetByIdByOwnerNameAsync(ownerName, personId);

            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonVM model)
        {
            var personDTO = _mapper.Map<PersonDTO>(model);

            var ownerName = HttpContext.User.Identity?.Name;

            await _personService.CreateByOwnerNameAsync(ownerName, personDTO);

            return Ok();
        }

        [HttpDelete]
        [Route("{personId:int}")]
        public async Task<IActionResult> Delete(int personId)
        {
            var ownerName = HttpContext.User.Identity?.Name;

            await _personService.DeleteByOwnerNameAsync(ownerName, personId);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("PublicPerson")]
        public async Task<IActionResult> GetAllPublic()
        {
            var persons = await _personService.GetAllPublicAsync();

            return Ok(persons);
        }

        [HttpPut]
        [Route("{personId:int}")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonVM updatePerson, int personId)
        {
            var personDTO = _mapper.Map<PersonDTO>(updatePerson);
            personDTO.Id = personId;

            var ownerName = HttpContext.User.Identity?.Name;

            await _personService.UpdateAsync(ownerName, personDTO);

            return Ok();
        }
    }
}

