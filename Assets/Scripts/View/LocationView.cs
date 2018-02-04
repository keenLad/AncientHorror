using System.Collections;
using System.Collections.Generic;
using AncientHorror.GameCore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LocationView : MonoBehaviour {
	[SerializeField] private Text _name;
	[SerializeField] private Text _count;
	[SerializeField] private Image _bg;
	[SerializeField] private float _delay = 2f;
	[SerializeField] private UnityEvent  OnLongPress;

	private Location _location;
	private Coroutine _delayRoutine;
	private bool isCardNeeded = true;

	public Location location {
		get { return _location; }
		set {
			_location = value;
			Init();
		}
	}

	void Init() {
		string regionName = GameSettings.instance.getRegion(location.region).name;

		if (!string.IsNullOrEmpty (regionName)) {
			regionName += "\n";
		}

		_name.text = regionName + location.name;
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
		if (!isCardNeeded) {
			return;
		}
		
		CardView.instance.card = GameSettings.instance.GetCard(location.id);
		SetCounter();
	}

	public void UpdateCards(){
		if (GameSettings.instance.GetTotalCardsCount (location.id) == GameSettings.instance.GetUsedCardsCount(location.id)) {
			GameSettings.instance.UpdateCards (location.id);
			SetCounter ();
		}
	}

	public void OnPointerDown(){
		isCardNeeded = true;
		_delayRoutine = StartCoroutine (StartWithDelay());
	}

	public void OnPointerUp(){
		StopCoroutine (_delayRoutine);
	}

	public void OnPointerExit(){
		StopCoroutine (_delayRoutine);
	}

	IEnumerator StartWithDelay(){
		yield return new WaitForSeconds (_delay);
		isCardNeeded = false;
		OnLongPress.Invoke();
	}
}
