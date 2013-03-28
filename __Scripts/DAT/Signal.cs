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
		if( col.gameObject.tag == "FingerSpot" )
		{
			Destroy( this.gameObject );
			testingScript.Reset();
		}
	}
}
