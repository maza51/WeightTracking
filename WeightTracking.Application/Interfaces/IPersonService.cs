using System;
using WeightTracking.Application.DTOs;

namespace WeightTracking.Application.Interfaces
{
    public interface IPersonService
    {
        Task<List<PersonDTO>> GetAllByOwnerNameAsync(string owner);

        Task CreateByOwnerNameAsync(string owner, PersonDTO personDTO);

        Task DeleteByOwnerNameAsync(string owner, int personId);

        Task<PersonDTO> GetByIdByOwnerNameAsync(string owner, int personId);

        Task<List<PersonDTO>> GetAllPublicAsync();

        Task UpdateAsync(string owner, PersonDTO personDTO);
    }
}

