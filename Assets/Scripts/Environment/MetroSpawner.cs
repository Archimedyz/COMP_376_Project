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

    GameObject mMainCameraRef; // for displaying the warnings.

    float spawnTimer;


	// Use this for initialization
	void Start () {
        mMainCameraRef = GameObject.FindGameObjectWithTag("MainCamera");
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
        } else if(spawnTimer >= (1.0f / spawnRate) + 1.5f) { // show some warning. sound the choo-choo.

        }
	}
}
