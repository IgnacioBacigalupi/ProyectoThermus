using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermus.Api.Entities
{
    public class Reading
    {
        public int Id { get; set; } 

        public int DeviceId { get; set; }

        public DateTime TakenAtUtc { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }

        public  Device? Device { get; set; }

    }
}