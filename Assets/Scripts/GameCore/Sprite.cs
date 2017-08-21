using Newtonsoft.Json;

namespace AncientHorror.GameCore {
    public class Sprite {
        [JsonProperty]
        public string code { get; private set; }
        [JsonProperty]
        public string name { get; private set; }
        [JsonProperty("sprite_code")]
        public string id { get; private set; }
    }
}