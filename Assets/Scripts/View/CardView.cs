using System.Collections;
using System.Collections.Generic;
using AncientHorror.GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CardView : MonoBehaviour {
    public static CardView instance { get; private set; }

    [SerializeField] private Image _background;
    [SerializeField] private Text _cardName;
    [SerializeField] private TextMeshProUGUI _cardText;
    [SerializeField] private Text _cardNum;
	[SerializeField] private Button _success;
	[SerializeField] private Button _failure;

    private Card _card;
    public Card card {
        get { return _card; }
        set {
            _card = value;
            Init();
        }
    }

    void Awake() {
        gameObject.SetActive(false);
        instance = this;
    }

    void Init() {
        _cardName.text = card.cardName;
		SetText(card.mainText);
        _cardNum.text = card.cardId.ToString();

        if (card.location != null) {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(GameSettings.instance.getLocation(_card.location.Value).spriteColor, out color);
            _background.color = color;
        }
        gameObject.SetActive(true);

		bool isButtonsNeed = !string.IsNullOrEmpty(card.successText) || !string.IsNullOrEmpty(card.failureText) ||
			card.failureCardId.HasValue || card.successCardId.HasValue;


		ShowButtons (isButtonsNeed);
		transform.localPosition = Vector3.zero;
    }

    public void Close() {
        gameObject.SetActive(false);
    }

	public void OnSuccess(){
		if (card.successCardId != null) {
			card = GameSettings.instance.GetCardById (card.successCardId.Value);
		} else {
			SetText(card.successText);
			ShowButtons (false);
		}

	}

	public void OnError(){
		if (card.failureCardId != null) {
			card = GameSettings.instance.GetCardById (card.failureCardId.Value);
		} else {
			SetText(card.failureText);
			ShowButtons (false);
		}
	}

	private void ShowButtons(bool isShow){

		_success.gameObject.SetActive (isShow);
		_failure.gameObject.SetActive (isShow);

	}

	private void SetText(string text){
		string processedText = text;

		foreach (AncientHorror.GameCore.Sprite sprite in GameSettings.instance.sprites) {
			string source = string.Format ("<sprite={0}>", sprite.id);
			processedText = processedText.Replace(sprite.code, source);
		}
		if (string.IsNullOrEmpty(processedText)) {
			processedText = "Ничего не происходит";
		}
		_cardText.text = processedText;
	}

	//First/Last finger position
	Vector3 fp,lp;

	//Distance needed for a swipe to take some Action
	public float DragDistance;
	bool isEnd = false;

	void Update()
	{
		//Check the touch inputs
		foreach (Touch touch in Input.touches)
		{
			

			if (touch.phase == TouchPhase.Began)
			{
				fp = touch.position;
				lp = touch.position;
				isEnd = false;
			}


			if (touch.phase == TouchPhase.Moved)
			{
				lp = touch.position;

				if (Mathf.Abs(lp.x - fp.x) > DragDistance || Mathf.Abs(lp.y - fp.y) > DragDistance)
				{ //It’s a drag
					//Now check what direction the drag was
					//First check which axis
					if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
					{
						isEnd = true;

						//If the horizontal movement is greater than the vertical movement…
						float delta = (lp.x - fp.x)/100f;
						if (lp.x > fp.x)
						{ //Right move
							transform.Translate(delta,0,0);
						}
						else
						{ //Left move
							transform.Translate(delta,0,0);
						}

					}
				}
			}

			if (touch.phase == TouchPhase.Ended && isEnd) {
				Close ();
			}
		}
	}
}
