using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class DataLoaderProgress : MonoBehaviour {

	[SerializeField]  Slider _slider;
	[SerializeField]  ContentView _content;
	[SerializeField] GameObject _localButton;

	public GameSettings settings;

	void Start () {
		_localButton.SetActive (File.Exists (Application.persistentDataPath + "/location_cards.json"));
		Preloader.instance.OnProgressChanged += PreloaderOnOnProgressChanged;
		Preloader.instance.OnLoadComplete += PreloaderOnOnLoadComplete;
	}

	private void PreloaderOnOnLoadComplete() {
		GameSettings.instance.currentBoss = GameSettings.instance.bosses [0];

		Debug.Log (JsonConvert.SerializeObject(GameSettings.instance.activeMythses));

		this.gameObject.SetActive(false);
		_content.SetLocations();
		_content.gameObject.SetActive(true);
		
	}

	private void PreloaderOnOnProgressChanged(float f) {
		_slider.value = f;
	}

	public void ReloadScene(){
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}
}
