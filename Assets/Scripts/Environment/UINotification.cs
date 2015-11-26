using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UINotification : MonoBehaviour {
    public enum TYPE {EXP,HPGAIN,HPLOSS,CRIT,LVLUP};
    private int duration = 5;

    public int Duration
    {
        get { return duration; }
        set { duration = value; }
    }
    private Text text;
    private float decrease = 0.02f;

    public float Decrease
    {
        get { return decrease; }
        set { decrease = value; }
    }
    private float timer = 0;
	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
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
