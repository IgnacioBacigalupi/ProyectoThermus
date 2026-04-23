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

        public async Task<LecturaDto> LecturaUltAsync()
        {
            return await _db.Readings
                .OrderByDescending(x => x.TakenAtUtc)
                .Select(x => new LecturaDto
                {
                    Temperature = x.Temperature,
                    Humidity = x.Humidity
                })
                .FirstAsync();
        }

        public async Task<List<UltimaLecturaPorDispositivoDto>> ObtenerUltimasPorDispositivoAsync()
        {
            var result = await _db.Devices
                .AsNoTracking()
                .Select(device => new
                {
                    Device = device,
                    UltimaLectura = _db.Readings
                        .Where(r => r.DeviceId == device.Id)
                        .OrderByDescending(r => r.TakenAtUtc)
                        .Select(r => new
                        {
                            r.Temperature,
                            r.Humidity,
                            r.TakenAtUtc
                        })
                        .FirstOrDefault()
                })
                .Where(x => x.UltimaLectura != null)
                .Select(x => new UltimaLecturaPorDispositivoDto
                {
                    DeviceId = x.Device.Id,
                    ExternalId = x.Device.ExternalId,
                    Name = x.Device.Name,
                    Location = x.Device.Location,
                    Temperature = x.UltimaLectura!.Temperature,
                    Humidity = x.UltimaLectura.Humidity,
                    TakenAtUtc = x.UltimaLectura.TakenAtUtc
                })
                .OrderBy(x => x.Location)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return result;
        }

        public async Task<List<ReadingHistoryDto>> ObtenerUltimasLecturasPorDispositivoAsync(int deviceId, int take = 50)
        {
            return await _db.Readings
                .AsNoTracking()
                .Where(x => x.DeviceId == deviceId)
                .OrderByDescending(x => x.TakenAtUtc)
                .Take(take)
                .Select(x => new ReadingHistoryDto
                {
                    Temperature = x.Temperature,
                    Humidity = x.Humidity,
                    TakenAtUtc = x.TakenAtUtc
                })
                .ToListAsync(); 
        }
    }


}
