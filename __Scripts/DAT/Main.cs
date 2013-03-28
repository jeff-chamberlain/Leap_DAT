using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	static public string state = "calibrating";
	static public ArrayList trackedFingers = new ArrayList();
	static public Vector3 center;
	static public float scale;
	
	private Calibrate calibrateScript;
	private TestingManager testingScript;
	
	void Awake()
	{
		Application.targetFrameRate = 300;
	}
	void Start()
	{
		calibrateScript = this.GetComponent<Calibrate>();
		testingScript = this.GetComponent<TestingManager>();
	}
	void Update ()
	{
		switch( state )
		{
		case "calibrating":
			calibrateScript.Run();
			break;
		case "testing":
			testingScript.Run();
			break;
		default:
			break;
		}
	}
}
