using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICanvas : MonoBehaviour {
    [SerializeField]
    private Text mDamageText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void CreateDamageLabel(Vector3 position)
    {
        Text dmg = (Text)Instantiate(mDamageText, position, Quaternion.identity);
        dmg.transform.SetParent(transform,true);
        dmg.transform.localScale = mDamageText.transform.localScale;
    }
}
