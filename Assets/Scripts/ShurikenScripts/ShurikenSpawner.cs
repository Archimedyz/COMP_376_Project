
// Authored by Robert Anthony Di Monaco


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
	private ShurikenController[] shurikenSripts;
	
	void Start () 
	{
		// Get references to other scripts
		script = Player.GetComponent<Player>();

		amountUsed = 0;  // when zero, none are instantiated
		shurikens = new Transform[Max]{ null, null, null, null }; 

		textControl = new TextMesh[Max]{ null, null, null, null };  // what each textMesh says
		textPositions = new Transform[Max]{ null, null, null, null };  // their positions

		teleportBlast = new ParticleSystem[Max]{ null, null, null, null };  // play the particle system when teleport to it

		shurikenSripts = new ShurikenController[Max]{ null, null, null, null }; // shuriken scripts 

		speed = 150.0f;
		turnSpeed = 300.0f;
	}
	
	void Update () 
	{
		facingDirection = script.GetFacingDirection ();
		
		// Throw Shuriken left or right
		if (amountUsed < 4) 
		{
			if (Input.GetKeyDown (KeyCode.I)) 
			{
				Clone = Instantiate (ShurikenPrefab, new Vector3(spawnThrow.transform.position.x, spawnThrow.transform.position.y, -2.0f), 
				                     	  Quaternion.identity) as Rigidbody; 
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3(Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -2.0f), 
				                          Quaternion.identity) as GameObject;
				for(int i = 0; i < Max; i++)  // get first open slot
				{
					if(shurikens[i] == null)
					{
						shurikens[i] = Clone.transform;

						textControl[i] = textObject.GetComponent<TextMesh>();     // get the TextMesh 
						textControl[i].text = "HOLD O";    // set initial so compiler doesnt complain
						textPositions[i] = textObject.GetComponent<Transform>();  // get its position 
						
						teleportBlast[i] = Clone.GetComponent<ParticleSystem>();  // get its particle system

						break; // found empty slot 
					}
				}
				if (facingDirection == Vector2.right)  			// Throw to the right  
				{	    	
					Clone.AddForce(speed * new Vector3(1.0f, 0.0f, 0.0f));
					Clone.AddTorque(new Vector3(0, 0, -turnSpeed));	
				} else    										// Throw to the left
				{   	
					Clone.AddForce (speed * new Vector3(-1.0f, 0.0f, 0.0f));
					Clone.AddTorque(new Vector3(0, 0, turnSpeed));
				}
				amountUsed++;  
			} 
			// Place Shuriken at feet	
			else if (Input.GetKeyDown (KeyCode.J)) 
			{
				Clone = Instantiate (ShurikenPrefab, new Vector3(spawnPlace.transform.position.x, spawnPlace.transform.position.y, -2.0f), 
				                     	  Quaternion.identity) as Rigidbody;
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3(Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -2.0f), 
				                          Quaternion.identity) as GameObject;
				for(int i = 0; i < Max; i++)  // get first open slot
				{
					if(shurikens[i] == null)
					{
						shurikens[i] = Clone.transform;
						
						textControl[i] = textObject.GetComponent<TextMesh>();     // get the TextMesh 
						textControl[i].text = "HOLD O";    // set initial so compiler doesnt complain
						textPositions[i] = textObject.GetComponent<Transform>();  // get its position 
						
						teleportBlast[i] = Clone.GetComponent<ParticleSystem>();  // get its particle system
						
						break; // found empty slot 
					}
				}
				amountUsed++;
			}
		}

		// update Text positions so they allign with shurikens
		for (int i = 0; i < Max; i++)  
			if(shurikens[i] != null)
				textPositions[i].position = new Vector3(shurikens[i].position.x - 0.414f, shurikens[i].position.y + 0.352f, -2.0f);
				
		// When user holds down the O key alter text
		if(Input.GetKey(KeyCode.O))    
		{
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

//			// When user holds down O key and a WASD alter text
//			if (Input.GetKey (KeyCode.A))
//			{
//				//Destroy (shurikens[0].gameObject); 
//				//amount--;
//			}else if (Input.GetKey (KeyCode.S)) 
//			{
//				//Destroy (shurikens[1].gameObject); 
//				//amount--;
//			}else if (Input.GetKey (KeyCode.D)) 
//			{
//				//Destroy (shurikens[2].gameObject); 
//				//amount--;
//			}else if (Input.GetKey (KeyCode.W)) 
//			{
//				//Destroy (shurikens[3].gameObject); 
//				//amount--;
//			}
		}
		if (Input.GetKeyUp (KeyCode.O)) 
		{
			// set them back once user releases the O key
			for(int i = 0; i < Max; i++)
			{
				if(shurikens[i] != null)
					textControl[i].text = "HOLD O"; 
			}
		}
	}
}




// TODO: TRIGGER SET ITS PARENT , GET REFERENCE IN ITS SCRIPT THEN IN HERE GO INTO KEYDOWN SHIT AND GET REFERENE TO THAT VAIRABLE AND MOVE THE ENEMY TO NEW LOCATION



// is null checks and amount-- when destory










