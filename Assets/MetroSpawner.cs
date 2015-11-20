using UnityEngine;
using System.Collections;

public class MetroSpawner : MonoBehaviour {

    [SerializeField]
    GameObject mMetroPrefabRef;

    public int spawnOrderInLayer;
    public float spawnRate; // trains per second
    public bool spawnRight;
    public float distanceToLive;

    float spawnTimer;


	// Use this for initialization
	void Start () {
	
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
        }
	}
}
