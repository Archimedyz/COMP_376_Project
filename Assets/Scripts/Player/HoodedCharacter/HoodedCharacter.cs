using UnityEngine;
using System.Collections;

public class HoodedCharacter : MonoBehaviour
{
	private bool mMoving;
	private bool mWaving;
	private bool mDissapear = false;

	private Animator mAnimator;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
	}
	

	void Update ()
	{
		ResetBoolean ();
		if (mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("FinalState")) {
			GameObject.Find ("LevelController").GetComponent<Level1Story> ().SetHoodedDissapeared (true);
			gameObject.SetActive (false);
		}
		UpdateAnimator ();
	}

	public void SetDissapears ()
	{
		mDissapear = true;
	}

	private void ResetBoolean ()
	{
		mMoving = false;
		mWaving = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isWaving", mWaving);
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isDisappearing", mDissapear);
	}
}
