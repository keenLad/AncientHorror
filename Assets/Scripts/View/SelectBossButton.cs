using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectBossButton : MonoBehaviour {

	[SerializeField] Text _bossName;
	public UnityEvent OnComplete;

	Boss _boss;


	public Boss boss {
		get {
			return _boss;
		}
		set {
			_boss = value;
			_bossName.text = _boss.name;
		}
	}

	public void SetBoss(){
		GameSettings.instance.currentBoss = boss;
		OnComplete.Invoke ();
	}
}
