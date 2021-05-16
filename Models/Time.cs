using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace world_time_app.Models
{
    public class Time
    {
        [JsonPropertyName("datetime")]
        public DateTime Datetime { get; set; }

        public string Area { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }

    }
}
