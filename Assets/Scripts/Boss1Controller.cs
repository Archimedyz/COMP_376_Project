using UnityEngine;
using System.Collections;

public class Boss1Controller : MonoBehaviour
{
	public GameObject fygarPrefab;
	public GameObject pookaPrefab;

	private PlayerTesting player;
	private GameObject[] enemies;
	public DigDug digdug;

	int bossDifficulty = 4;
	
	public float maxY, minY;

	private bool canMove;

	public Vector3 playerPosition;
	private float enemySpeed = 1.25f;

	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<PlayerTesting> ();
		digdug = GameObject.Find ("DigDug").GetComponent<DigDug> ();
		enemies = new GameObject[6];
		CreateWave ();
	}

	void Update ()
	{
		if (enemies [0] != null) {
			if (enemies [0].transform.position.y >= enemies [0].GetComponent<Pooka> ().GetInitialPosition ().y) {
				player.SetCanMove (true);
				digdug.SetCanMove (true);
				for (int i = 0; i < bossDifficulty; i++) {
					if (i < 3) {
						enemies [i].GetComponent<Pooka> ().SetCanMove (true);
						enemies [i].GetComponent<Pooka> ().SetBoundaries (maxY, minY);
					} else {
						enemies [i].GetComponent<Fygar> ().SetCanMove (true);
						enemies [i].GetComponent<Fygar> ().SetBoundaries (maxY, minY);
					}
				}
			}
		}
	}

	public void CreateWave ()
	{
		enemySpeed += 0.25f;
		player.SetCanMove (false);
		player.SetPosition (playerPosition);
		digdug.SetCanMove (false);

		if (enemies [0] != null) {
			for (int i = 0; i < transform.childCount; i++) {
				Destroy (enemies [i]);
			}
		}

		for (int i = 0; i < bossDifficulty; i++) {
			if (i < 3) {
				enemies [i] = Instantiate (pookaPrefab, new Vector3 (-3.1f + (i * 0.3f), -7f + (i * 2), -1f), Quaternion.identity) as GameObject;
				enemies [i].transform.parent = transform;
				enemies [i].GetComponent<Pooka> ().SetSpeed (enemySpeed);
			} else {
				enemies [i] = Instantiate (fygarPrefab, new Vector3 (-1.5f + ((i - 3) * 0.3f), -7f + ((i - 3) * 2), -1f), Quaternion.identity) as GameObject;
				enemies [i].transform.parent = transform;
				enemies [i].GetComponent<Fygar> ().SetSpeed (enemySpeed);
			}
		}
	}

	public void IncreaseDifficulty ()
	{
		bossDifficulty ++;
	}
}
