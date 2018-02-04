using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ModalDialog : MonoBehaviour
{
	[SerializeField] private Button _yesBtn;
	[SerializeField] private Button _noBtn;
	[SerializeField] private Text _question;

	public UnityEvent OnConfirm;

	public string question{
		get{ 
			return _question.text;
		}
		set{ 
			_question.text = value;
		}
	}

	void Awake(){
		_yesBtn.onClick.AddListener (new UnityAction (OnYesClick));
	}

	void OnYesClick(){
		OnConfirm.Invoke ();
		Close ();
	}

	public void Close(){
		Destroy (gameObject);
	}

}


