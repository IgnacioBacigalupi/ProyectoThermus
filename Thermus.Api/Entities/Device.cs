using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermus.Api.Entities
{
    public class Device
    {
        public int Id { get; set; }

        public required string ExternalId { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }
    }
}