using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffAnimation : MonoBehaviour {
	public bool animationIsActive = false;
	public int buffDuration=5;
	public GameObject MovingClockHandle;

	public GameObject UpperSign;
	public GameObject MidSign;
	public GameObject LowerSign;
	public int signChangingSpeed = 10;

	private float signChangingFrequency;
	private Vector3 movingHandleInitialPosition;
	private int timeLeft=-1;
	private float currentTransperancyLevel = 0.01f;

	// Use this for initialization
	void Start () {
		movingHandleInitialPosition = MovingClockHandle.transform.position;
		signChangingFrequency = 1 / (float)signChangingSpeed;
		Invoke ("StartAnimation", 2);
	}

	public void StartAnimation(){
		animationIsActive = true;
		MovingClockHandle.transform.position = movingHandleInitialPosition;
		timeLeft = buffDuration;
		InitColor ();
		AnimationStep ();
		AnimateSign ();
	}

	private void InitColor(){
		SetTransperancyToSpecialObject (UpperSign, currentTransperancyLevel);
		SetTransperancyToSpecialObject (MidSign, currentTransperancyLevel);
		SetTransperancyToSpecialObject (LowerSign, 1);

	}

	private void SetTransperancyToSpecialObject(GameObject obj, float newTransperancy){ //sets transperanty without changing the color to a specially constructed obj
		Color currentColor = obj.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
		obj.transform.GetChild (0).GetComponent<MeshRenderer> ().material.color = new Color (currentColor.r, currentColor.g, currentColor.b, newTransperancy);

	}

	private void AnimationStep(){

		if (timeLeft < 0) {
			animationIsActive = false;
			return;
		} else {
			if (timeLeft != buffDuration) { //second has not elapsed yet
								AnimateClock ();
			}
			timeLeft -= 1;
			Invoke ("AnimationStep", 1);
		}
	}

	private void AnimateSign(){
		if (timeLeft < 0) {
			currentTransperancyLevel = 0.01f;
			return;
		} else {
			currentTransperancyLevel=currentTransperancyLevel + 1/(float)signChangingSpeed;
			currentTransperancyLevel = Mathf.Repeat(currentTransperancyLevel,1.0f);

			SetTransperancyToSpecialObject (UpperSign, Mathf.Repeat((currentTransperancyLevel+0.33f),1.0f));
			SetTransperancyToSpecialObject (MidSign,  Mathf.Repeat((currentTransperancyLevel+0.66f),1.0f));
			SetTransperancyToSpecialObject (LowerSign, currentTransperancyLevel);

			Invoke ("AnimateSign", signChangingFrequency);
		}
	}
	private void AnimateClock(){
		MovingClockHandle.transform.Rotate (0, 90 / buffDuration, 0);
	}
}
