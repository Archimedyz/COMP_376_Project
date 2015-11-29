using UnityEngine;
using System.Collections;

public class MetroSpawner : MonoBehaviour {

    [SerializeField]
    GameObject mMetroPrefabRef;

    public int spawnOrderInLayer;
    public float spawnRate; // trains per second
    public bool spawnRight;
    public float distanceToLive;
    public float topOfTrack;
    public float bottomOfTrack;

    GameObject mUICanvasRef; // for displaying the warnings.
    [SerializeField]
    GameObject mWarningBarPrefab;
    GameObject mWarningBarRef;

    float spawnTimer;


	// Use this for initialization
	void Start () {
        mUICanvasRef = GameObject.FindGameObjectWithTag("UICanvas");
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer += Time.deltaTime;
	    if(spawnTimer >= (1.0f / spawnRate)) {
            spawnTimer = 0.0f;
            GameObject spawnedMetro = Instantiate(mMetroPrefabRef, transform.position, Quaternion.identity) as GameObject;
            spawnedMetro.SendMessage("SetOrderInLayer", spawnOrderInLayer);
            spawnedMetro.SendMessage("SetGoingRight", spawnRight);
            spawnedMetro.SendMessage("SetDistanceToLive", distanceToLive);
            spawnedMetro.SendMessage("SetTopOfTrack", topOfTrack);
            spawnedMetro.SendMessage("SetBottomOfTrack", bottomOfTrack);
            Destroy(mWarningBarRef);
            mWarningBarRef = null;
        } else if(spawnTimer >= (1.0f / spawnRate) - 2.0f) { // show some warning. sound the choo-choo.
            if(mWarningBarRef == null) {
                mWarningBarRef = Instantiate(mWarningBarPrefab, mUICanvasRef.transform.position, Quaternion.identity) as GameObject;
                mWarningBarRef.transform.SetParent(mUICanvasRef.transform, false);
                mWarningBarRef.transform.localPosition = new Vector3((spawnRight ? -0.5f : 0.5f) * mUICanvasRef.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f);
                mWarningBarRef.transform.localScale = mWarningBarPrefab.transform.localScale;
            } else {
                mWarningBarRef.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 0.0f, 0.0f, (Mathf.Sin(Time.time * 5.0f) + 1) * 0.25f);
                Debug.Log("Alpha" + (Mathf.Sin(Time.time) * 127.0f));
            }
        }
	}
}
