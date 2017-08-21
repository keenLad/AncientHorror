using System.Collections;
using System.Collections.Generic;
using AncientHorror.GameCore;
using UnityEngine;
using UnityEngine.UI;

public class ContentView : MonoBehaviour {
	[SerializeField] 
	private LocationView _locationPrefab;
	[SerializeField]
	private Text _gatesButton;
	[SerializeField]
	private LocationView _expedition;

	[SerializeField] private Transform _content;

	public void SetLocations() {
		foreach (Location location in GameSettings.instance.locations) {
			if (location.isHided == 0) {
				LocationView locationObject = GameObject.Instantiate (_locationPrefab);
				locationObject.name = location.name;
				locationObject.transform.SetParent (_content, false);

				locationObject.location = location;
			}

		}
		_gatesButton.text = GameSettings.instance.GetTotalCardsCount(18).ToString();

		Card card = GameSettings.instance.GetCardByRegion (5, false);
		_expedition.location = GameSettings.instance.getLocation (card.location.Value);

	}

	public void GetGate(){
		CardView.instance.card = GameSettings.instance.GetCardByRegion(6);
		_gatesButton.text = (GameSettings.instance.GetTotalCardsCount(18) - GameSettings.instance.GetUsedCardsCount(18)).ToString();
	}

	public void GetExpedition(){
		Card card = GameSettings.instance.GetCardByRegion (5, false);
		if (card != null) {
			_expedition.location = GameSettings.instance.getLocation (card.location.Value);
		} else {
			_expedition.gameObject.SetActive (false);
		}
	}
}
