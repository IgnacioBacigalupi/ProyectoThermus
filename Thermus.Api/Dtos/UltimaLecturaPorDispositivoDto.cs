namespace Thermus.Api.Dtos;

public class UltimaLecturaPorDispositivoDto
{
    public int DeviceId { get; set; }

    public string ExternalId { get; set; } = string.Empty;

    public string? Name { get; set; }

    public string? Location { get; set; }

    public decimal Temperature { get; set; }

    public decimal Humidity { get; set; }

    public DateTime TakenAtUtc { get; set; }
}
