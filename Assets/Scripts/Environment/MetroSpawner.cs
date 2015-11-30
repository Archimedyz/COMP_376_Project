using UnityEngine;
using System.Collections;

public class MetroSpawner : MonoBehaviour {

    [SerializeField]
    GameObject mMetroPrefabRef;

    public int spawnOrderInLayer;
    public bool spawnRight;
    public float distanceToLive;
    public float topOfTrack;
    public float bottomOfTrack;

    GameObject mUICanvasRef; // for displaying the warnings.
    Player mPlayerRef; // to adjust the spawnrates.

    [SerializeField]
    GameObject mWarningBarPrefab;
    AudioSource mTrainWarningAudio;
    GameObject mWarningBarRef;

    float spawnTimer;


	// Use this for initialization
	void Start () {
        mUICanvasRef = GameObject.FindGameObjectWithTag("UICanvas");
        mPlayerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mTrainWarningAudio = GetComponent<AudioSource>();
        ResetSpawnTimer();
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;
	    if(spawnTimer <= 0.0f) {
            spawnTimer = 0.0f;
            GameObject spawnedMetro = Instantiate(mMetroPrefabRef, transform.position, Quaternion.identity) as GameObject;
            spawnedMetro.SendMessage("SetOrderInLayer", spawnOrderInLayer);
            spawnedMetro.SendMessage("SetGoingRight", spawnRight);
            spawnedMetro.SendMessage("SetDistanceToLive", distanceToLive);
            spawnedMetro.SendMessage("SetTopOfTrack", topOfTrack);
            spawnedMetro.SendMessage("SetBottomOfTrack", bottomOfTrack);
            Destroy(mWarningBarRef);
            mWarningBarRef = null;
            ResetSpawnTimer();
        } else if(spawnTimer <= 2.5f) { // show some warning. sound the choo-choo.
            if(mWarningBarRef == null) {
                mWarningBarRef = Instantiate(mWarningBarPrefab, mUICanvasRef.transform.position, Quaternion.identity) as GameObject;
                mWarningBarRef.transform.SetParent(mUICanvasRef.transform, false);
                mWarningBarRef.transform.localPosition = new Vector3((spawnRight ? -0.5f : 0.5f) * mUICanvasRef.GetComponent<RectTransform>().rect.width, 0.0f, 0.0f);
                mWarningBarRef.transform.localScale = mWarningBarPrefab.transform.localScale;
                if(mPlayerRef.mFloorIndex == 1) {
                    mTrainWarningAudio.Play();
                }
            } else {
                mWarningBarRef.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 0.0f, 0.0f, (Mathf.Sin(Time.time * 5.0f) + 1) * 0.25f);
            }
        }
	}

    void ResetSpawnTimer() {
        float spawnBaseCooldown = (mPlayerRef.mFloorIndex == 1) ? 7.5f : 15.0f;
        spawnTimer = spawnBaseCooldown - Random.Range(0.0f, spawnBaseCooldown * 0.2f); // allow for fluctuation between spawn times
    }
}
