using System;
using WeightTracking.Application.DTOs;

namespace WeightTracking.Application.Interfaces
{
    public interface IRecordService
    {
        Task CreateByPersonIdAsync(int personId, RecordDTO recordDTO);

        Task DeleteByPersonIdAsync(int personId, int id);

        Task UpdateByPersonIdAsync(int personId, RecordDTO recordDTO);

        Task<List<RecordDTO>> GetAllByPersonIdAsync(int personId);
    }
}

