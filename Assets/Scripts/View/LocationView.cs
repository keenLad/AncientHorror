using System.Collections;
using System.Collections.Generic;
using AncientHorror.GameCore;
using UnityEngine;
using UnityEngine.UI;

public class LocationView : MonoBehaviour {
	[SerializeField] private Text _name;
	[SerializeField] private Text _count;
	[SerializeField] private Image _bg;

	private Location _location;

	public Location location {
		get { return _location; }
		set {
			_location = value;
			Init();
		}
	}

	void Init() {
		_name.text = location.name;
		Color color = Color.white;
		ColorUtility.TryParseHtmlString(location.spriteColor, out color);
		_bg.color = color;

		SetCounter();
	}

	void SetCounter() {
		int totalCards = GameSettings.instance.GetTotalCardsCount(location.id);
		_count.text = string.Format("{0}/{1}", totalCards - GameSettings.instance.GetUsedCardsCount(location.id), totalCards);
	}

	public void GetCard() {
		CardView.instance.card = GameSettings.instance.GetCard(location.id);
		SetCounter();
	}

	public void UpdateCards(){
		if (GameSettings.instance.GetTotalCardsCount (location.id) == GameSettings.instance.GetUsedCardsCount(location.id)) {
			GameSettings.instance.UpdateCards (location.id);
			SetCounter ();
		}
	}
}
