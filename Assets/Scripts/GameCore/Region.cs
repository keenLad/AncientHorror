using Newtonsoft.Json;

namespace AncientHorror.GameCore {
    public class Region {
        [JsonProperty]
        public int id { get; private set; }
        [JsonProperty]
        public string name { get; private set; }

    }
}