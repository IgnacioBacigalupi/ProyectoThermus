using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermus.Api.Dtos
{
    public class ReadingsDto
    {
        public required string ExternalId { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }
    }
}