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
            var result = await _readingServices.CreateAsync(dto);
            return Ok(result);
        }

    }
}