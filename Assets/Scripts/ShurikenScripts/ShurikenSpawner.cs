
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
	
	// Instantiated shuriken
	private Rigidbody Clone;
	
	// Player has 4 shurikens
	private const int maxShurikens = 4;  // 4 for now but maybe more?
	private int amount;
	private Transform[] shurikens;    // key A at [0], key S at [1]. key D at [2], key W at [3]

	// Text hovering above the shurikens, it says what controls to use it
	[SerializeField]
	GameObject TextControlPrefab;
	private GameObject textObject;   // store the Text game object
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

		amount = 0;  // when zero, none are instantiated
		shurikens = new Transform[maxShurikens]{Player.transform, Player.transform, Player.transform, Player.transform,};  // intialized to Player position to avoid exceptions 

		textControl = new TextMesh[maxShurikens];   // what each textMesh says
		textPositions = new Transform[maxShurikens];  // their positions

		teleportBlast = new ParticleSystem[maxShurikens];  // play the particle system when teleport to it

		shurikenSripts = new ShurikenController[maxShurikens]; // shuriken scripts

		speed = 150.0f;
		turnSpeed = 300.0f;
	}
	
	void Update () 
	{
		facingDirection = script.GetFacingDirection ();
		
		// Throw Shuriken left or right
		if (amount < maxShurikens) 
		{
			if (Input.GetKeyDown (KeyCode.I)) 
			{
				Clone = Instantiate (ShurikenPrefab, spawnThrow.transform.position, Quaternion.identity) as Rigidbody; 
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3(Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -2.0f), 
				                          Quaternion.identity) as GameObject;
				textControl[amount] = textObject.GetComponent<TextMesh>();     // get the TextMesh so can alter it later
				textControl[amount].text = "HOLD O";    // set initial so compiler doesnt complain
				textPositions[amount] = textObject.GetComponent<Transform>();  // get its position to alter later

				teleportBlast[amount] = Clone.GetComponent<ParticleSystem>();  // get its particle system

				if (facingDirection == Vector2.right)  // Throw to the right  
				{	    	
					Clone.AddForce(speed * new Vector3(1.0f, 0.0f, 0.0f));
					Clone.AddTorque(new Vector3(0, 0, -turnSpeed));
					
					shurikens[amount] = Clone.transform; 					
				} else    // Throw to the left
				{   	
					Clone.AddForce (speed * new Vector3(-1.0f, 0.0f, 0.0f));
					Clone.AddTorque(new Vector3(0, 0, turnSpeed));
					
					shurikens[amount] = Clone.transform; 
				}

				//clone.timeoutDestructor = 5;
				amount++;
			} 
			// Place Shuriken at feet	
			else if (Input.GetKeyDown (KeyCode.J)) 
			{
				Clone = Instantiate (ShurikenPrefab, spawnPlace.transform.position, Quaternion.identity) as Rigidbody;
				textObject = Instantiate (TextControlPrefab, 
				                          new Vector3(Clone.transform.position.x - 0.414f, Clone.transform.position.y + 0.352f, -2.0f), 
				                          Quaternion.identity) as GameObject;
				textControl[amount] = textObject.GetComponent<TextMesh>();     // get the TextMesh so can alter it later
				textControl[amount].text = "HOLD O";    // set initial so compiler doesnt complain
				textPositions[amount] = textObject.GetComponent<Transform>();  // get its position to alter later

				teleportBlast[amount] = Clone.GetComponent<ParticleSystem>();  // get its particle system

				shurikens[amount] = Clone.transform; 

				amount++;
			}
		}
		// update Text positions so they allign with shurikens
		for (int i = 0; i < amount; i++)  
			textPositions[i].position = new Vector3(shurikens[i].position.x - 0.414f, shurikens[i].position.y + 0.352f, -2.0f);
				
		// When user holds down the O key alter text
		if(Input.GetKey(KeyCode.O))    
		{
			// set controls Text
			for(int i = 0; i < amount; i++)
			{
				if(i == 0)      textControl[0].text = "A PRESS A";  
				else if(i == 1) textControl[1].text = "S PRESS S";  
				else if(i == 2) textControl[2].text = "D PRESS D";  
				else if(i == 3) textControl[3].text = "W PRESS W"; 
			}

			// Teleport the player to shuriken and play the effect
			if(Input.GetKeyDown (KeyCode.A))   		  // 1st shuriken
			{
				Player.transform.position = shurikens[0].position;  
				if(amount > 0)
					teleportBlast[0].Play (false);			 
			}
			else if (Input.GetKeyDown (KeyCode.S))    // 2nd shuriken
			{
				Player.transform.position = shurikens[1].position;
				if(amount > 1)
					teleportBlast[1].Play (false);
			}
			else if (Input.GetKeyDown (KeyCode.D))    // 3rd shuriken
			{
				Player.transform.position = shurikens[2].position;
				if(amount > 2)
					teleportBlast[2].Play (false);
			}
			else if (Input.GetKeyDown (KeyCode.W))    // 4th shuriken
			{
				Player.transform.position = shurikens[3].position;	
				if(amount > 3)
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
			for(int i = 0; i < amount; i++)
			{
				if(i == 0)      textControl[0].text = "HOLD O"; 
				else if(i == 1) textControl[1].text = "HOLD O";
				else if(i == 2) textControl[2].text = "HOLD O";
				else if(i == 3) textControl[3].text = "HOLD O";
			}
		}
	}
}


// fix deleting a shuirken,  add if(object == null)
// FIGURE OUT COLLISION ISSUES
// TODO: TRIGGER SET ITS PARENT , GET REFERENCE IN ITS SCRIPT THEN IN HERE GO INTO KEYDOWN SHIT AND GET REFERENE TO THAT VAIRABLE AND MOVE THE ENEMY TO NEW LOCATION














