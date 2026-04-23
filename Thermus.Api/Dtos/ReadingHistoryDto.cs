namespace Thermus.Api.Dtos;

public class ReadingHistoryDto
{
    public decimal Temperature { get; set; }

    public decimal Humidity { get; set; }

    public DateTime TakenAtUtc { get; set; }
}
