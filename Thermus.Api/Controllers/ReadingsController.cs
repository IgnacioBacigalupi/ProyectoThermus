using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Thermus.Api.Dtos;
using Thermus.Api.Infrastructure;
using Thermus.Api.Entities;
using Thermus.Api.Services;

namespace Thermus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingsController : ControllerBase
    {
        private readonly IReadingServices _readingServices;
        public ReadingsController(IReadingServices readingServices)
        {
            _readingServices = readingServices;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReadingsDto dto)
        {
            Console.WriteLine($"ALERTA Trace={HttpContext.TraceIdentifier} HumedadAlta ...");
            var result = await _readingServices.CreateAsync(dto);
            return Ok(result);
        }

        [HttpGet("ultima")]
        public async Task<ActionResult<LecturaDto>> ObtenerUltimaLectura()
        {
            var result = await _readingServices.LecturaUltAsync();
            return Ok(result);
        }

        [HttpGet("ultimas-por-dispositivo")]
        public async Task<ActionResult<List<UltimaLecturaPorDispositivoDto>>> ObtenerUltimasPorDispositivo()
        {
            var result = await _readingServices.ObtenerUltimasPorDispositivoAsync();
            return Ok(result);
        }

        [HttpGet("dispositivo/{deviceId:int}/ultimas")]
        public async Task<ActionResult<List<ReadingHistoryDto>>> ObtenerUltimasLecturasPorDispositivo(int deviceId)
        {
            var result = await _readingServices.ObtenerUltimasLecturasPorDispositivoAsync(deviceId);
            return Ok(result);
        }

    }
}
