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
            var reading = new Reading
            {
                Device = device,
                Temperature = dto.Temperature,
                Humidity = dto.Humidity,
                TakenAtUtc = DateTime.UtcNow,
            };
            if(reading.Humidity > 70 && device.ExternalId == "sensor-baño")
            {
                // Aquí podrías agregar la lógica para enviar una alerta, como enviar un correo electrónico o una notificación.
                Console.WriteLine("Alerta: Alta humedad detectada en el sensor del baño.");
            }
            _db.Readings.Add(reading);

            await _db.SaveChangesAsync();

            return reading.Id;
        
        }
    }
}