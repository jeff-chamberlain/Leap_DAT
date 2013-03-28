using UnityEngine;
using System.Collections;

public class Signal : MonoBehaviour {
	
	private TestingManager testingScript;
	
	// Use this for initialization
	void Start () {
		testingScript = Camera.main.GetComponent<TestingManager>();
	}
	
	void OnCollisionEnter( Collision col )
	{
		if( col.gameObject.tag == "FingerSpot" && col.gameObject.GetComponent<FingerSpot>().tracked )
		{
			col.gameObject.GetComponent<FingerSpot>().tracked = false;
			Destroy( this.gameObject );
			testingScript.SignalHit( Time.time );
		}
	}
}
