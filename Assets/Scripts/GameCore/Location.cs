using Newtonsoft.Json;

namespace AncientHorror.GameCore {
    public class Location {

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
        public int isHided { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Location: id={0}, name={1}]", id, name);
		}
    }
}
