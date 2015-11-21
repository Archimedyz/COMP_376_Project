using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICanvas : MonoBehaviour {

    public Text mDamageText;
    public int createdText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void CreateDamageLabel(int damage, Vector3 position)
    {
        createdText++;
        Text dmg = (Text)Instantiate(mDamageText, position, Quaternion.identity);
        dmg.transform.SetParent(transform,true);
        dmg.transform.localScale = mDamageText.transform.localScale;
        dmg.text = damage.ToString();
    }
}
