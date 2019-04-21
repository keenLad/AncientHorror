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
    [SerializeField] string DOCS_URI = "https://script.google.com/macros/s/AKfycbxOLElujQcy1-ZUer1KgEvK16gkTLUqYftApjNCM_IRTL3HSuDk/exec?id={0}{2}{1}";
    [SerializeField] string SHEET_ID = "1ZbroOoG2i6_XYlBWJ7tAk0w9zvBzSZUhMepOtAHzTzE";

    //[SerializeField] string CARDS_SHEET = "location_cards";
    //[SerializeField] string GATE_SHEET = "gate_cards";
    //[SerializeField] string REGIONS_SHEET = "regions";
    //[SerializeField] string LOCATIONS_SHEET = "locations";
    //[SerializeField] string SPRITES_SHEET = "sprites";
    //[SerializeField] string BOSSES_SHEET = "bosses";
    //[SerializeField] string MYTH_SHEET = "myths";
    //[SerializeField] string EVIDENCE_SHEET = "evidence";
    [SerializeField] string [] sheets = { };


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
        foreach (var sheet in sheets) {
            StartCoroutine (LoadFromSheet(sheet));
        }
	}

	public void LoadFromLocal(){
        foreach (var sheet in sheets) {
            StartCoroutine (LoadFromAssets(sheet));
        }
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
