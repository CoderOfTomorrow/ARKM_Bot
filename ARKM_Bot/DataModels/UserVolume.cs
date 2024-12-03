using System.Text.Json.Serialization;

namespace ARKM_Bot.DataModels
{
    public class UserVolume
    {
        [JsonPropertyName("spotTakerVolume")]
        public string SpotVolume { get; set; }
    }
}