using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ARKM_Bot.DataModels
{
    public class PriceData
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }
}