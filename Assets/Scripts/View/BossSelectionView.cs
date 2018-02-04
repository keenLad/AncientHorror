using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossSelectionView : MonoBehaviour {

	[SerializeField] VerticalLayoutGroup _container;
	[SerializeField] SelectBossButton _prefab;

	public UnityEvent OnBossSelected;

	public void Init(){
		foreach (var boss in GameSettings.instance.bosses) {
			if (boss.isEnabled == 1) {
				SelectBossButton btn = Instantiate<SelectBossButton> (_prefab);
				btn.transform.SetParent (_container.transform);
				btn.transform.localScale = _prefab.transform.localScale;
				btn.boss = boss;
				btn.OnComplete.AddListener(new UnityAction( () => {
					OnBossSelected.Invoke ();
				}));
			}
			
		}
	
	}
}
