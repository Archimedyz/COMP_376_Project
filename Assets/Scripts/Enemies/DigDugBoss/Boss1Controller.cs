using UnityEngine;
using System.Collections;

public class Boss1Controller : MonoBehaviour
{
	public GameObject fygarPrefab;
	public GameObject pookaPrefab;
	
	private Player player;
	private GameObject[] enemies;
	private DigDug digdug;
	
	int bossDifficulty = 4;
	
	private bool canMove;
	
	private float enemySpeed = 1.25f;
	
	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private SpriteRenderer mSpriteRenderer;
	private int mInitialOrderInLayer;
	private bool floorBoundaryInitialized;
	
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		digdug = GameObject.Find ("DigDug").GetComponent<DigDug> ();
		enemies = new GameObject[6];	
		CreateWave (0);
	}
	
	void Update ()
	{
		int deadEnemy = CheckDead ();
		
		if (deadEnemy != -1) {
			if (deadEnemy < 3) {
				StartCoroutine (AddPooka (deadEnemy));
			} else if (deadEnemy < 6) {
				StartCoroutine (AddFygar (deadEnemy));
			}
		}
		
		if (enemies [0] != null) {
			if (enemies [0].transform.position.y >= enemies [0].GetComponent<Pooka> ().GetFloorBoundaryY () - 4) {
				player.SetCanMove (true);
				digdug.SetCanMove (true);
				for (int i = 0; i < bossDifficulty - 1; i++) {
					if (i < 3) {
						enemies [i].GetComponent<Pooka> ().SetCanMove (true);
					} else {
						enemies [i].GetComponent<Fygar> ().SetCanMove (true);
					}
				}
			}
		}  
	}
	
	private IEnumerator AddPooka (int position)
	{
		enemies [position] = Instantiate (pookaPrefab, new Vector3 ((digdug.transform.position.x - 8f) + (position * 0.3f), -7f + (position * 2), -1f), Quaternion.identity) as GameObject;
		enemies [position].transform.parent = transform;
		enemies [position].GetComponent<Pooka> ().SetSpeed (enemySpeed);
		yield return new WaitForSeconds (3.0f);
	}
	
	private IEnumerator AddFygar (int position)
	{
		enemies [position] = Instantiate (fygarPrefab, new Vector3 ((digdug.transform.position.x - 5.5f) + ((position - 3) * 0.3f), -7f + ((position - 3) * 2), -1f), Quaternion.identity) as GameObject;
		enemies [position].transform.parent = transform;
		enemies [position].GetComponent<Fygar> ().SetSpeed (enemySpeed);
		yield return new WaitForSeconds (3.0f);
	}
	
	private int CheckDead ()
	{
		for (int i = 0; i < bossDifficulty - 1; i++) {
			if (enemies [i] == null) {
				return i;
			}
		}
		return -1;
	}
	
	public void CreateWave (int wave)
	{
		enemySpeed += 0.25f;
		player.SetCanMove (false);
		if (wave != 0)
			player.SetPosition (digdug.transform.position - new Vector3 (10, 0, 0));
		digdug.SetCanMove (false);
		
		for (int i = 0; i < bossDifficulty - 1; i++) {
			if (i < 3) {
				enemies [i] = Instantiate (pookaPrefab, new Vector3 ((digdug.transform.position.x - 8f) + (i * 0.3f), -7f + (i * 2), -1f), Quaternion.identity) as GameObject;
				enemies [i].transform.parent = transform;
				enemies [i].GetComponent<Pooka> ().SetSpeed (enemySpeed);
			} else {
				enemies [i] = Instantiate (fygarPrefab, new Vector3 ((digdug.transform.position.x - 5.5f) + ((i - 3) * 0.3f), -7f + ((i - 3) * 2), -1f), Quaternion.identity) as GameObject;
				enemies [i].transform.parent = transform;
				enemies [i].GetComponent<Fygar> ().SetSpeed (enemySpeed);
			}
		}
	}
	
	public void DestroyWave ()
	{
		if (enemies [0] != null) {
			for (int i = 0; i < transform.childCount; i++) {
				Destroy (enemies [i]);
			}
		}
	}
	
	public void IncreaseDifficulty ()
	{
		bossDifficulty ++;
	}
}
