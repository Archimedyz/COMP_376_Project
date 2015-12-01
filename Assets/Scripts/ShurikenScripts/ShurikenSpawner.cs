
//** 
//** Authored by Robert Anthony Di Monaco
//**

using UnityEngine;
using System.Collections;

/* Jump: space
 * Defend: c
 * charge: X
 * z: normal and slide
 * q: bash
 * 
 * Bash and Slide
 * Normal and Charge
 * 4 Buttons
 * 3 buttons shuriken
 * 
 */

public class ShurikenSpawner : MonoBehaviour 
{
	// References, they're self explanatory********
	[SerializeField]
	GameObject spawnThrow, spawnPlace, Player;  
	[SerializeField]
	Rigidbody ShurikenPrefab;
	private Player script;
	private Vector2 facingDirection;
	
	// Speeds of thrown Shuriken
	private float speed, turnSpeed;

	// Player has 4 shurikens
	private Rigidbody Clone; // placeholder for Instantiated shuriken
	private const int Max = 4;  // 4 for now but maybe more?
	private int amountUsed;  // how many shurikens are active in scene
	private Transform[] shurikens;    // key A at [0], key S at [1]. key D at [2], key W at [3]

	// Text hovering above the shurikens, it says what controls to use it
	[SerializeField]
	GameObject TextControlPrefab;
	private GameObject textObject;   // place holder for the Text game object
	private TextMesh[] textControl;  // has all the texts in the scene, use this to alter if you want it to say something else
	private Transform[] textPositions;

	// particle system which is played when player teleports
	private ParticleSystem[] teleportBlast;

	// shuriken scripts, each has its own
	private ShurikenController[] shurikenScripts;

	// bool values needed for teleporting enemy
	private bool[] isPressed;    // 0 is A, 1 is S, 2 is D, 3 is W

	void Start () 
	{
		// Get references to other scripts
		script = Player.GetComponent<Player>();

		amountUsed = 0;  // when zero, none are instantiated
		shurikens = new Transform[Max]{ null, null, null, null }; 

		textControl = new TextMesh[Max]{ null, null, null, null };  // what each textMesh says
		textPositions = new Transform[Max]{ null, null, null, null };  // their positions

		teleportBlast = new ParticleSystem[Max]{ null, null, null, null };  // play the particle system when teleport to it

		shurikenScripts = new ShurikenController[Max]{ null, null, null, null }; // shuriken scripts 

		isPressed = new bool[Max]{ false, false, false, false };    // none have been pressed at start

		speed = 1050.0f;
		turnSpeed = 300.0f;
	}
	
	void Update () 
	{
		facingDirection = script.GetFacingDirection ();
		
		// Throw Shuriken left or right
		if (amountUsed < 4) {
			if (Input.GetKeyDown (KeyCode.I)) 
			{
				Clone = Instantiate (ShurikenPrefab, new Vector3 (spawnThrow.transform.position.x, spawnThrow.transform.position.y, -3.0f), 
				                     	  Quaternion.identity) as Rigidbody; 
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3 (Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -3.0f), 
				                          Quaternion.identity) as GameObject;

				// get first open slot
				for (int i = 0; i < Max; i++) 
				{  
					if (shurikens [i] == null) {
						shurikens [i] = Clone.transform;

						textControl [i] = textObject.GetComponent<TextMesh> ();     // get the TextMesh 
						textControl [i].text = "O HOLD O";    // set initial so compiler doesnt complain
						textPositions [i] = textObject.GetComponent<Transform> ();  // get its position 
						
						teleportBlast [i] = Clone.GetComponent<ParticleSystem> ();  // get its particle system

						shurikenScripts[i] = Clone.GetComponent<ShurikenController>();  // get its script
						shurikenScripts[i].SetPlaced(false); 

						// Assign it the appropriate tag
						if(i == 0) Clone.gameObject.tag = "Shuriken0";
						else if(i == 1) Clone.gameObject.tag = "Shuriken1";
						else if(i == 2) Clone.gameObject.tag = "Shuriken2";
						else if(i == 3) Clone.gameObject.tag = "Shuriken3";

						break; // found empty slot 
					}
				}
				if (facingDirection == Vector2.right) {  			// Throw to the right	    	
					Clone.AddForce (speed * new Vector3 (1.0f, 0.0f, 0.0f));
					Clone.AddTorque (new Vector3 (0, 0, -turnSpeed));	
				} else {    										// Throw to the left   	
					Clone.AddForce (speed * new Vector3 (-1.0f, 0.0f, 0.0f));
					Clone.AddTorque (new Vector3 (0, 0, turnSpeed));
				}
				amountUsed++;  
			} 
			// Place Shuriken at feet	
			else if (Input.GetKeyDown (KeyCode.J)) 
			{
				Clone = Instantiate (ShurikenPrefab, new Vector3 (spawnPlace.transform.position.x, spawnPlace.transform.position.y, -3.0f), 
				                     	  Quaternion.identity) as Rigidbody;
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3 (Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -3.0f), 
				                          Quaternion.identity) as GameObject;

				// get first open slot
				for (int i = 0; i < Max; i++) 
				{ 
					if (shurikens [i] == null) {
						shurikens [i] = Clone.transform;
						
						textControl [i] = textObject.GetComponent<TextMesh> ();     // get the TextMesh 
						textControl [i].text = "O HOLD O";    // set initial so compiler doesnt complain
						textPositions [i] = textObject.GetComponent<Transform> ();  // get its position 
						
						teleportBlast [i] = Clone.GetComponent<ParticleSystem> ();  // get its particle system

						shurikenScripts[i] = Clone.GetComponent<ShurikenController>();  // get its script
						shurikenScripts[i].SetPlaced(true);   // this shuriken does not move

						// Assign it the appropriate tag
						if(i == 0) Clone.gameObject.tag = "Shuriken0";
						else if(i == 1) Clone.gameObject.tag = "Shuriken1";
						else if(i == 2) Clone.gameObject.tag = "Shuriken2";
						else if(i == 3) Clone.gameObject.tag = "Shuriken3";

						break; // found empty slot 
					}
				}
				amountUsed++;
			}
		}

		// update Text positions so they allign with shurikens
		for (int i = 0; i < Max; i++)  
			if (shurikens [i] != null)
				textPositions [i].position = new Vector3 (shurikens [i].position.x - 0.414f, shurikens [i].position.y + 0.352f, -3.0f);

		// When user holds down the O key alter text
		if(Input.GetKey(KeyCode.O) && !Input.GetKey(KeyCode.K))    
		{
			Time.timeScale = 0.2f;  // SLOW MOTION so player has time to choose

			// set controls Text
			for(int i = 0; i < Max; i++)
			{
				if(shurikens[i] != null)
					if(i == 0)      textControl[0].text = "A PRESS A";  
					else if(i == 1) textControl[1].text = "S PRESS S";  
					else if(i == 2) textControl[2].text = "D PRESS D";  
					else if(i == 3) textControl[3].text = "W PRESS W"; 
			}

			// Teleport the player to shuriken and play the effect
			if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)   		  // 1st shuriken
			{
				Player.transform.position = shurikens[0].position;  
				teleportBlast[0].Play (false);			 
			}
			else if (Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)    // 2nd shuriken
			{
				Player.transform.position = shurikens[1].position;
				teleportBlast[1].Play (false);
			}
			else if (Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)    // 3rd shuriken
			{
				Player.transform.position = shurikens[2].position;
				teleportBlast[2].Play (false);
			}
			else if (Input.GetKeyDown (KeyCode.W) && shurikens[3] != null )    // 4th shuriken
			{
				Player.transform.position = shurikens[3].position;	
				teleportBlast[3].Play (false);
			}
		}

		// alter controls Text for shurikens with enemyOnPlaced or with a parent --> so player knows it has soemthing to teleport
		if(!Input.GetKey(KeyCode.O) && !Input.GetKey(KeyCode.K))
			for(int i = 0; i < Max; i++)
			{
				if(shurikens[i] != null)
					if(shurikenScripts[i].GetEnemyOnPlaced() != null || shurikens[i].parent != null)
						textControl[i].text = "O HOLD O\nK HOLD K";    // sets if shuriken has something to teleport
				else
					textControl[i].text = "O HOLD O";    // set back 
			}

		// When user holds down K key can teleport enemy from one shuriken to another
		if (Input.GetKey (KeyCode.K) && !Input.GetKey(KeyCode.O))
		{
			Time.timeScale = 0.2f;  // SLOW MOTION so player has time to choose

			// set text on those with something to teleport
			for(int i = 0; i < Max; i++)
			{
				if(shurikens[i] != null && (shurikenScripts[i].GetEnemyOnPlaced() != null || shurikens[i].parent != null))
					if(i == 0)      textControl[0].text = "A TELEPORT A";  
					else if(i == 1) textControl[1].text = "S TELEPORT S";
					else if(i == 2) textControl[2].text = "D TELEPORT D"; 
					else if(i == 3) textControl[3].text = "W TELEPORT W"; 
			}

			// Whats getting teleported
			if(Input.GetKeyDown(KeyCode.A) && isPressed[1] == false && isPressed[2] == false && isPressed[3] == false)  isPressed[0] = true; 
			else if(Input.GetKeyDown(KeyCode.S) && isPressed[0] == false && isPressed[2] == false && isPressed[3] == false) isPressed[1] = true;
			else if(Input.GetKeyDown(KeyCode.D) && isPressed[0] == false && isPressed[1] == false && isPressed[3] == false) isPressed[2] = true;
			else if(Input.GetKeyDown(KeyCode.W) && isPressed[0] == false && isPressed[1] == false && isPressed[2] == false) isPressed[3] = true;

		// TELEPORTING A
			if(isPressed[0] && shurikens[0] != null && (shurikenScripts[0].GetEnemyOnPlaced() != null || shurikens[0].parent != null))      				
			{
				// Display new text, where can teleport to
				if(shurikens[1] != null) textControl[1].text = "S TARGET TARGET S";
				if(shurikens[2] != null) textControl[2].text = "D TARGET TARGET D";
				if(shurikens[3] != null) textControl[3].text = "W TARGET TARGET W";

				// Where its getting teleported
				if(shurikenScripts[0].GetPlaced()) 	// placed shurikens
				{
					// Teleport whats on shuriken to new location
					if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikenScripts[0].GetEnemyOnPlaced().position = shurikens[1].position; 
						teleportBlast[1].Play (false);
					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikenScripts[0].GetEnemyOnPlaced().position = shurikens[2].position;
						teleportBlast[2].Play (false);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikenScripts[0].GetEnemyOnPlaced().position = shurikens[3].position;
						teleportBlast[3].Play (false);
					}
				}
				else   		// mobile parented shurikens
				{
					// Teleport shuriken's parent to new location
					if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikens[0].parent.position = shurikens[1].position;  
						teleportBlast[1].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (0);

					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikens[0].parent.position = shurikens[2].position;
						teleportBlast[2].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (0);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikens[0].parent.position = shurikens[3].position;
						teleportBlast[3].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (0);
					}
				}
			}
		// TELEPORTING S
			else if(isPressed[1] && shurikens[1] != null && (shurikenScripts[1].GetEnemyOnPlaced() != null || shurikens[1].parent != null))      		
			{ 
				// Display new text, where can teleport to
				if(shurikens[0] != null) textControl[0].text = "A TARGET TARGET A";
				if(shurikens[2] != null) textControl[2].text = "D TARGET TARGET D";
				if(shurikens[3] != null) textControl[3].text = "W TARGET TARGET W";

				// Where its getting teleported
				if(shurikenScripts[1].GetPlaced())   		// placed shurikens
				{
					// Teleport whats on shuriken to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikenScripts[1].GetEnemyOnPlaced().position = shurikens[0].position;
						teleportBlast[0].Play (false);
					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikenScripts[1].GetEnemyOnPlaced().position = shurikens[2].position;
						teleportBlast[2].Play (false);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikenScripts[1].GetEnemyOnPlaced().position = shurikens[3].position;
						teleportBlast[3].Play (false);
					}
				}
				else if(shurikens[1].parent != null)  		// mobile parented shurikens
				{
					// Teleport shuriken's parent to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikens[1].parent.position = shurikens[0].position;
						teleportBlast[0].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (1);
					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikens[1].parent.position = shurikens[2].position;
						teleportBlast[2].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (1);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikens[1].parent.position = shurikens[3].position;
						teleportBlast[3].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (1);
					}
				}
			}
		// TELEPORTING D
			else if(isPressed[2] && shurikens[2] != null && (shurikenScripts[2].GetEnemyOnPlaced() != null || shurikens[2].parent != null))				
			{
				// Display new text, where can teleport to
				if(shurikens[0] != null) textControl[0].text = "A TARGET TARGET A";
				if(shurikens[1] != null) textControl[1].text = "S TARGET TARGET S";
				if(shurikens[3] != null) textControl[3].text = "W TARGET TARGET W";

				// Where its getting teleported
				if(shurikenScripts[2].GetPlaced())   		// placed shurikens
				{
					// Teleport whats on shuriken to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikenScripts[2].GetEnemyOnPlaced().position = shurikens[0].position;  
						teleportBlast[0].Play (false);
					}else if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikenScripts[2].GetEnemyOnPlaced().position = shurikens[1].position;
						teleportBlast[1].Play (false);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikenScripts[2].GetEnemyOnPlaced().position = shurikens[3].position;
						teleportBlast[3].Play (false);
					}
				}
				else if(shurikens[2].parent != null)  		// mobile parented shurikens
				{
					// Teleport shuriken's parent to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikens[2].parent.position = shurikens[0].position; 
						teleportBlast[0].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (2);
					}else if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikens[2].parent.position = shurikens[1].position;
						teleportBlast[1].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (2);
					}else if(Input.GetKeyDown (KeyCode.W) && shurikens[3] != null)
					{
						shurikens[2].parent.position = shurikens[3].position;
						teleportBlast[3].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (2);
					}
				}
			}
		// TELEPORTING W
			else if(isPressed[3] && shurikens[3] != null && (shurikenScripts[3].GetEnemyOnPlaced() != null || shurikens[3].parent != null))				
			{
				// Display new text, where can teleport to
				if(shurikens[0] != null) textControl[0].text = "A TARGET A";
				if(shurikens[1] != null) textControl[1].text = "S TARGET S";
				if(shurikens[2] != null) textControl[2].text = "D TARGET D";

				// Where its getting teleported
				if(shurikenScripts[3].GetPlaced())   		// placed shurikens
				{
					// Teleport whats on shuriken to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikenScripts[3].GetEnemyOnPlaced().position = shurikens[0].position;
						teleportBlast[0].Play (false);
					}else if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikenScripts[3].GetEnemyOnPlaced().position = shurikens[1].position;
						teleportBlast[1].Play (false);
					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikenScripts[3].GetEnemyOnPlaced().position = shurikens[2].position;
						teleportBlast[2].Play (false);
					}
				}
				else if(shurikens[3].parent != null)  		// mobile parented shurikens
				{
					// Teleport shuriken's parent to new location
					if(Input.GetKeyDown (KeyCode.A) && shurikens[0] != null)
					{
						shurikens[3].parent.position = shurikens[0].position; 
						teleportBlast[0].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (3);
					}else if(Input.GetKeyDown (KeyCode.S) && shurikens[1] != null)
					{
						shurikens[3].parent.position = shurikens[1].position;
						teleportBlast[1].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (3);
					}else if(Input.GetKeyDown (KeyCode.D) && shurikens[2] != null)
					{
						shurikens[3].parent.position = shurikens[2].position;
						teleportBlast[2].Play (false);

						// can only teleport enemy once, then the shuriken gets destroyed
						DestroyIt (3);
					}
				}
			}
		}

		if (Input.GetKeyUp (KeyCode.O) || Input.GetKeyUp (KeyCode.K)) 
		{
			Time.timeScale = 1.0f;    // Undo slow motion 

			// set them back once user releases the O key or the K key
			for(int i = 0; i < Max; i++)
			{
				if(shurikens[i] != null)
					if(shurikenScripts[i].GetEnemyOnPlaced() != null || shurikens[i].parent != null)
						textControl[i].text = "O HOLD O\nK HOLD K";    // sets if shuriken has something to teleport
				else
					textControl[i].text = "O HOLD O";    // set back 
			}

			// set bool elements to false
			for(int i = 0; i < Max; i++)
				isPressed[i] = false;
		}
	}

	// Used to destroy parented Shurikens when its teleported
	public void DestroyIt(int i)
	{
		Destroy(shurikens[i].gameObject);   // destroy it 						
		shurikens[i] = null;         // free variable up
		Destroy(textControl[i].gameObject);   // destroy text					
		amountUsed--;      // return it to inventory
	}

	// When player holds O key and K key while near shuriken they'll pick it up
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Shuriken0") 
		{
			if (Input.GetKey (KeyCode.O) && Input.GetKey (KeyCode.K))
			{
				Destroy(other.gameObject);   // destroy it 

				shurikens[0] = null;         // free variable up
				Destroy(textControl[0].gameObject);   // destroy text
				amountUsed--;      // return it to inventory
			}
		}
		else if(other.gameObject.tag == "Shuriken1") 
		{
			if (Input.GetKey (KeyCode.O) && Input.GetKey (KeyCode.K))
			{
				Destroy(other.gameObject);   // destroy it 
				
				shurikens[1] = null;         // free variable up
				Destroy(textControl[1].gameObject);   // destroy text					
				amountUsed--;      // return it to inventory
			}
		}
		else if(other.gameObject.tag == "Shuriken2") 
		{
			if (Input.GetKey (KeyCode.O) && Input.GetKey (KeyCode.K))
			{
				Destroy(other.gameObject);   // destroy it 
				
				shurikens[2] = null;         // free variable up
				Destroy(textControl[2].gameObject);   // destroy text				
				amountUsed--;      // return it to inventory
			}
		}
		else if(other.gameObject.tag == "Shuriken3") 
		{
			if (Input.GetKey (KeyCode.O) && Input.GetKey (KeyCode.K))
			{
				Destroy(other.gameObject);   // destroy it 
				
				shurikens[3] = null;         // free variable up
				Destroy(textControl[3].gameObject);   // destroy text				
				amountUsed--;      // return it to inventory
			}
		}
	}
}



















