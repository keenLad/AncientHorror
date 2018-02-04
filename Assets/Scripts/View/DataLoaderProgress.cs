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
	[SerializeField] BossSelectionView _bossView;


	public GameSettings settings;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		_localButton.SetActive (File.Exists (Application.persistentDataPath + "/location_cards.json"));
		Preloader.instance.OnProgressChanged += PreloaderOnOnProgressChanged;
		Preloader.instance.OnLoadComplete += PreloaderOnOnLoadComplete;
	}

	private void PreloaderOnOnLoadComplete() {
		_bossView.gameObject.SetActive (true);
		_bossView.Init ();

		_bossView.OnBossSelected.AddListener(new UnityEngine.Events.UnityAction( () => {
			_bossView.gameObject.SetActive (false);
			this.gameObject.SetActive (false);

			_content.SetLocations ();
			_content.gameObject.SetActive (true);
		}));
		
	}

	private void PreloaderOnOnProgressChanged(float f) {
		_slider.value = f;
	}

	public void ReloadScene(){
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}
}
