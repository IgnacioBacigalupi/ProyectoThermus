using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thermus.Api.Dtos;

namespace Thermus.Api.Services
{
    public interface IReadingServices
    {
        Task<int> CreateAsync(ReadingsDto dto, CancellationToken ct = default);

         Task<LecturaDto> LecturaUltAsync();
    }
}