using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimation : MonoBehaviour {
	public GameObject beam;
	public float beamSpeed = 5f;
	public float beamStepAngle = 1;
	public float maxScanAngle=45f;
	// Use this for initialization

	private float currentBeamAngle = 0;
	void Start () {
		AnimateBeam ();
	}
	
	private void AnimateBeam(){
		if ((currentBeamAngle >= maxScanAngle)|| (currentBeamAngle<=-1*maxScanAngle) )
			beamStepAngle = -1 * beamStepAngle;
		currentBeamAngle = currentBeamAngle + beamStepAngle;
		beam.transform.Rotate (new Vector3(beamStepAngle,0,0));

		Invoke ("AnimateBeam", 1 / beamSpeed);
	}
}
