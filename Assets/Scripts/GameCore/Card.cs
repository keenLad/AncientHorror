using Newtonsoft.Json;
using UnityEngine;

namespace AncientHorror.GameCore {
    public class Card {
        [JsonProperty]
        public int id { get; private set; }
        [JsonProperty]
        public int? location { get; private set; }
        [JsonProperty]
        public int cardId { get; private set; }
        [JsonProperty]
        public string cardName { get; private set; }
        [JsonProperty]
        public string mainText { get; private set; }
        [JsonProperty]
        public string successText { get; private set; }
        [JsonProperty]
        public string failureText { get; private set; }
        [JsonProperty]
        public int? successCardId { get; private set; }
        [JsonProperty]
        public int? failureCardId { get; private set; }
        [JsonProperty]
        public int? extensionNum { get; private set; }
    }
}
