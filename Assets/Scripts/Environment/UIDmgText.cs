using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDmgText : MonoBehaviour {
    private int duration;
    private Text text;
    private float decrease;
    private float timer = 0;
	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
        decrease = 0.04f;
        duration = 4;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > 0.2f)
        {
            if (timer > duration) { Destroy(gameObject); }
                
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a-decrease);
            transform.Translate(new Vector3(0, decrease, 0));
        }
	}
}
