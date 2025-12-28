using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thermus.Api.Dtos;
using Thermus.Api.Entities;
using Thermus.Api.Infrastructure;

namespace Thermus.Api.Services
{
    public class ReadingServices : IReadingServices
    {
        private readonly ThermusDbContext _db;

        public ReadingServices(ThermusDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateAsync(ReadingsDto dto, CancellationToken ct = default)
        {
            var device = await _db.Devices.FirstOrDefaultAsync(x => x.ExternalId == dto.ExternalId);
            if (device == null)
            {
                device = new Device
                {
                    ExternalId = dto.ExternalId,
                    Name = dto.Name,
                    Location = dto.Location
                };
                _db.Devices.Add(device);
            }
            var reading = new Entities.Reading
            {
                Device = device,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                TakenAtUtc = DateTime.UtcNow,
            };
            _db.Readings.Add(reading);

            await _db.SaveChangesAsync();

            return reading.Id;
        
        }
    }
}