using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight.Api.Infrastructure.Dtos
{
    public class BaggageDto
    {
        public string Label { get; set; }
        public int Weight { get; set; }
    }
}
