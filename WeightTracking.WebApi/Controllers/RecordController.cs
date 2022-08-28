using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeightTracking.Application.DTOs;
using WeightTracking.Application.Interfaces;
using WeightTracking.WebApi.ViewModels;

namespace WeightTracking.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/person/{personId:int}/[controller]")]
    public class RecordController : Controller
    {
        private IRecordService _recordService;
        private IMapper _mapper;

        public RecordController(IRecordService recordService, IMapper mapper)
        {
            _recordService = recordService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int personId, [FromBody] CreateRecordVM model)
        {
            var recordDTO = _mapper.Map<RecordDTO>(model);

            await _recordService.CreateByPersonIdAsync(personId, recordDTO);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int personID)
        {
            var recordsDTO = await _recordService.GetAllByPersonIdAsync(personID);

            return Ok(recordsDTO);
        }

        [HttpDelete]
        [Route("{recordId:int}")]
        public async Task<IActionResult> Delete(int personId, int recordId)
        {
            await _recordService.DeleteByPersonIdAsync(personId, recordId);

            return Ok();
        }

        [HttpPut]
        [Route("{recordId:int}")]
        public async Task<IActionResult> Update(int personId, int recordId, [FromBody] UpdateRecordVM model)
        {
            var recordDTO = _mapper.Map<RecordDTO>(model);
            recordDTO.Id = recordId;

            await _recordService.UpdateByPersonIdAsync(personId, recordDTO);

            return Ok();
        }
    }
}

