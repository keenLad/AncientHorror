using System;
using System.Collections.Generic;
using AncientHorror.GameCore;
using Newtonsoft.Json;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Sprite = AncientHorror.GameCore.Sprite;
using System.Collections;

public class GameSettings {
    
    public static GameSettings instance = new GameSettings();

	[JsonProperty(Preloader.CARDS_SHEET)]
	public List<Card> cards = new List<Card>();

	[JsonProperty(Preloader.GATE_SHEET)]
    private List<Card> gateCards {
        set {
            if (cards == null) {
                cards = new List<Card>();
            }

            cards.AddRange(value);
        }
    }
	[JsonProperty(Preloader.EVIDENCE_SHEET)]
	private List<Card> evidenceCards {
		set {
			if (cards == null) {
				cards = new List<Card>();
			}

			cards.AddRange(value);
		}
	}
    [JsonProperty]
	public List<Location> locations = new List<Location>();
    [JsonProperty]
	public List<Region> regions = new List<Region>();
    [JsonProperty]
	public List<Sprite> sprites = new List<Sprite> ();
	[JsonProperty]
	public List<Boss> bosses = new List<Boss> ();
	[JsonProperty(Preloader.MYTH_SHEET)]
	private List<Myth> _myths = new List<Myth> ();
	[JsonProperty]
	public Dictionary<int, string> extensions = new Dictionary<int, string> ();

	public List<Myth> activeMythses = new List<Myth> ();

	private Boss _currentBoss;
	public Boss currentBoss{ get{ 
			return _currentBoss;
		} 
		set{ 
			_currentBoss = value;
			SetRegionVisible (_currentBoss.regionId, true);
		}
	}

    
    private List<Card> _usedCards = new List<Card>();

    public Card GetCard(int locationId) {
        Card result = null;

        result = cards.Where(card => card.location != null && card.location.Value == locationId).OrderBy(o=>Guid.NewGuid()).FirstOrDefault();
        if (result != default(Card)) {
            _usedCards.Add(result);
            cards.Remove(result);
        }

        return result;
    }

    public int GetUsedCardsCount(int locationId) {
        return _usedCards.Count(card => card.location != null && card.location.Value == locationId);
    }

    public int GetTotalCardsCount(int locationId) {
        int result = 0;
        result = cards.Union(_usedCards).Count(card => card.location != null && card.location.Value == locationId);
        return result;
    }

    public Location getLocation(int id) {
        return locations.FirstOrDefault(location => location.id == id);
    }

	public Region getRegion(int id) {
		return regions.FirstOrDefault(region => region.id == id);
	}

	public Card GetCardById(int id){
		return cards.FirstOrDefault(card => card.id == id);
	}

	public Card GetCardByRegion(int regionId, bool isRemove = true){
		Card result = null;

		var query = cards
			.Join (locations, card => card.location, location => location.id, (card, location) => new{Card = card, Location = location})
			.Where (cardWithLocation => cardWithLocation.Location.region == regionId)
			.OrderBy (o => Guid.NewGuid ())
			.FirstOrDefault ();
		if (query != null) {
			result = query.Card;
		}
		if (isRemove && result != null) {
			_usedCards.Add(result);
			cards.Remove(result);
		}

		return result;
	}

	public void UpdateCards(int locationId) {
        var used = _usedCards.Where(card => card.location != null && card.location.Value == locationId).ToList();
        cards.AddRange(used);
        foreach (var card in used) {
            _usedCards.Remove(card);
        }
    }

	public void DeleteLocation(int locationId){
		var locationCards = cards.Where(card => card.location == locationId).ToList();
		foreach (var card in locationCards) {
			_usedCards.Add (card);
			cards.Remove (card);
		}
	}

	void SetRegionVisible(int id, bool isVisible){
		List<Location> regionLocations = locations.Where (location => location.region == id).ToList ();
		foreach (var item in regionLocations) {
			item.isHided = isVisible ? 0 : 1;
		}
	}

    public static GameSettings operator +(GameSettings a, GameSettings b) {

        if (b.cards != null) {
            a.cards.AddRange(b.cards);
        }
        if (b.locations != null) {
            a.locations.AddRange(b.locations);
        }
        if (b.regions != null) {
            a.regions.AddRange(b.regions);
        }
        if (b.sprites != null) {
            a.sprites.AddRange(b.sprites);
        }
		if (b.bosses != null) {
			a.bosses.AddRange (b.bosses);
		}

		if (b._myths != null) {
			a._myths.AddRange (b._myths);
		}

        return a;
    }

	private void CreateMythDeck(){
		
		foreach (var round in _currentBoss.myths) {
			List<Myth> result = new List<Myth> ();
			for(int type = 0; type < round.Value.Count; type++) {
				for (int count = 0; count < round.Value [type]; count++) {
					Myth item = _myths.Where (myth => myth.type == type + 1).OrderBy (o => UnityEngine.Random.value).FirstOrDefault ();
					result.Add (item);
					_myths.Remove (item);
				}
			}
			result = result.OrderBy( x => UnityEngine.Random.value ).ToList( );
			activeMythses.AddRange (result);

		}

		string log = "";
		foreach (var item in activeMythses) {
			log += item.ToString() + "\n";
		}
		Debug.Log (log);
	}
    
    
}
