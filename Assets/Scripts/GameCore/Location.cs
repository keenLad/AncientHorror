using Newtonsoft.Json;

namespace AncientHorror.GameCore {
    public class Location {
//	"id": 1,
//	"region": 1,
//	"name": "город",
//	"sprite": "",
//	"spriteColor": "#4C4C4CFF",
//	"isHided": 0
        [JsonProperty]
        public int id { get; private set; }
        [JsonProperty]
        public int region { get; private set; }
        [JsonProperty]
        public string name { get; private set; }
        [JsonProperty]
        public string sprite { get; private set; }
        [JsonProperty]
        public string spriteColor { get; private set; }
        [JsonProperty]
        public int isHided { get; private set; }
    }
}
