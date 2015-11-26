using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICanvas : MonoBehaviour {

    public Text mUINotification;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void CreateDamageLabel(string text, Vector3 position, UINotification.TYPE t)
    {
        Text dmg = (Text)Instantiate(mUINotification, position, Quaternion.identity);
        dmg.transform.SetParent(transform,true);
        switch (t)
        {
            case UINotification.TYPE.CRIT:
                dmg.color = Color.yellow;
                dmg.fontSize = 14;
                break;
            case UINotification.TYPE.EXP:
                dmg.fontSize = 10;
                dmg.color = Color.white;
                break;
            case UINotification.TYPE.HPGAIN:
                dmg.color = Color.green;
                dmg.fontSize = 12;
                break;
            case UINotification.TYPE.HPLOSS:
                dmg.color = Color.red;
                break;
            case UINotification.TYPE.LVLUP:
                dmg.color = Color.yellow;
                dmg.fontSize = 15;
                dmg.GetComponent<UINotification>().Duration = 8;
                dmg.GetComponent<UINotification>().Decrease = 0.01f;
                break;
        }
        dmg.transform.localScale = mUINotification.transform.localScale;
        dmg.text = text;
    }
}
