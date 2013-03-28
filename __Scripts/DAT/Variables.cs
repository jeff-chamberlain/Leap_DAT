using UnityEngine;
using System.Collections;

public class Variables {

	public int cueIndex, cueType;
	public Vector3 signalVec;
	public float angle;
	public bool valid;
	
	public Variables( float rad )
	{
		//Determine if the Cue points directly at the Probe, gives a 90 degree wedge, a 180 degree wedge, or
		//gives no information (360 degrees)
		cueIndex = Random.Range(0, 8);
		//Randomize the angle within the wedge
		switch( cueIndex )
		{
		case 0:
			angle = Mathf.Deg2Rad * 0;
			cueType = 1;
			break;
		case 1:
			angle = Mathf.Deg2Rad * 180;
			cueType = 1;
			break;
		case 2:
			angle = Random.Range(Mathf.Deg2Rad * (315 + TestingManager.signalOffset ), 
						Mathf.Deg2Rad * (405  - TestingManager.signalOffset));
			if(angle >= 2 * Mathf.PI) //This prevents angles over 360 degrees. Without it the probe sometimes jumps around
				angle -= 2 * Mathf.PI;
			cueType = 2;
			break;
		case 3:
			angle = Random.Range(Mathf.Deg2Rad * (135 + TestingManager.signalOffset), 
						Mathf.Deg2Rad * (225  - TestingManager.signalOffset));
			cueType = 2;
			break;
		case 4:
			angle = Random.Range(Mathf.Deg2Rad * (270 + TestingManager.signalOffset), 
						Mathf.Deg2Rad * (450  - TestingManager.signalOffset));
			if(angle >= 2 * Mathf.PI)
				angle -= 2 * Mathf.PI;
			cueType = 3;
			break;
		case 5:
			angle = Random.Range(Mathf.Deg2Rad * (90 + TestingManager.signalOffset), 
						Mathf.Deg2Rad * (270  - TestingManager.signalOffset));
			cueType = 3;
			break;
		case 6:
		case 7:
			angle = Random.Range(Mathf.Deg2Rad * 0, Mathf.Deg2Rad * 360);
			cueType = 4;
			break;
		}
		//Find Vector using the resulting angle, based off the center of the screen
		Vector3 vec = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0F);
		vec *= rad;
		signalVec = new Vector3(Main.center.x + vec.x, Main.center.y + vec.y, Main.center.z);
		//Randomize if Probe is "green" or "red"
		float validRand = Random.value;
		if(validRand > 0.2F)
			valid = true;
	}
}
