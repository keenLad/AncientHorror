using System;
using System.Collections;
using System.Collections.Generic;
using AncientHorror.GameCore;
using Newtonsoft.Json;
using UnityEngine;

public class Preloader : MonoBehaviour {
    
    #region singleton

    public static Preloader instance {
        get;
        private set;
    }

    #endregion

	private static readonly string DOCS_URI = "https://script.google.com/macros/s/AKfycbxOLElujQcy1-ZUer1KgEvK16gkTLUqYftApjNCM_IRTL3HSuDk/exec?id={0}{2}{1}";
    private static readonly string SHEET_ID = "1ZbroOoG2i6_XYlBWJ7tAk0w9zvBzSZUhMepOtAHzTzE";

	public const string CARDS_SHEET = "location_cards";
	public const string GATE_SHEET = "gate_cards";
	public const string REGIONS_SHEET = "regions";
	public const string LOCATIONS_SHEET = "locations";
	public const string SPRITES_SHEET = "sprites";
	public const string BOSSES_SHEET = "bosses";
	public const string MYTH_SHEET = "myths";
	public const string EVIDENCE_SHEET = "evidence";


    public event Action<float> OnProgressChanged;
    public event Action OnLoadComplete;

    private int _started = 0;
    private int _loaded = 0;

    void Awake() {
        instance = this;
    }

    private void Start() {
		GameSettings.instance = new GameSettings ();
    }

	public void LoadFromWeb(){
		StartCoroutine(LoadFromSheet(CARDS_SHEET));
		StartCoroutine(LoadFromSheet(GATE_SHEET));
		StartCoroutine(LoadFromSheet(REGIONS_SHEET));
		StartCoroutine(LoadFromSheet(LOCATIONS_SHEET));
		StartCoroutine(LoadFromSheet(SPRITES_SHEET));
		StartCoroutine(LoadFromSheet(BOSSES_SHEET));
		StartCoroutine(LoadFromSheet(MYTH_SHEET));
		StartCoroutine(LoadFromSheet(EVIDENCE_SHEET));
	}

	public void LoadFromLocal(){
		StartCoroutine(LoadFromAssets(CARDS_SHEET));
		StartCoroutine(LoadFromAssets(GATE_SHEET));
		StartCoroutine(LoadFromAssets(REGIONS_SHEET));
		StartCoroutine(LoadFromAssets(LOCATIONS_SHEET));
		StartCoroutine(LoadFromAssets(SPRITES_SHEET));
		StartCoroutine(LoadFromAssets(BOSSES_SHEET));
		StartCoroutine(LoadFromAssets(MYTH_SHEET));
		StartCoroutine(LoadFromAssets(EVIDENCE_SHEET));
	}

    IEnumerator LoadFromSheet(string type) {
       
        yield return true;
		string isFullLoad = string.IsNullOrEmpty (type) ? "" : "&sheet=";
		string uri = string.Format(DOCS_URI, SHEET_ID, type, isFullLoad);

		yield return LoadData (uri, (result) => {

			result.PrintToFile(Application.persistentDataPath + "/" + type + ".json");

		});
        
    }

	IEnumerator LoadFromAssets(string type){
		string baseUrl = "file:///"+Application.persistentDataPath + "/";

		yield return null;

		string uri = baseUrl + type + ".json";

		yield return LoadData (uri);

	}

	IEnumerator LoadData(string uri, Action<string> callback = null) {
		Debug.Log ("Load from: " + uri);
		_started++;
		WWW request = new WWW(uri);
		yield return request;

		if (OnProgressChanged != null) {
			OnProgressChanged((float) _loaded / (float) _started);
		}
		if (string.IsNullOrEmpty(request.error)) {
			string data = System.Text.Encoding.UTF8.GetString(request.bytes);

			if (callback != null) {
				callback (data);
			}

			GameSettings.instance += JsonConvert.DeserializeObject<GameSettings>(data);
		}
		else {
			Debug.Log(request.error);
		}
		_loaded++;
		if (_started == _loaded && OnLoadComplete != null) {
			OnLoadComplete();
		}
	}
}
