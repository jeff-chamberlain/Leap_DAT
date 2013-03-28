using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	static public string state = "calibrating", path;
	static public ArrayList trackedFingers = new ArrayList();
	static public Vector3 center;
	static public float scale;
	
	public string filePath = @"C:\Users\mbp01\Desktop\LeapDATData\", fileName = "DAT", trialNum = "1";
	
	private Calibrate calibrateScript;
	private TestingManager testingScript;
	
	void Awake()
	{
		Application.targetFrameRate = 200;
	}
	void Start()
	{
		calibrateScript = this.GetComponent<Calibrate>();
		testingScript = this.GetComponent<TestingManager>();
		path = Application.persistentDataPath + "/" + fileName + "." + trialNum + ".txt";
		if (!System.IO.File.Exists( path ))
        {
            // Create a file to write to.
            string createText = "Initiate Data " + System.Environment.NewLine;
            System.IO.File.WriteAllText(path, createText);
        }
		else {
			string addText = "New Data Entry " + System.Environment.NewLine;
            System.IO.File.AppendAllText(path, addText);
		}
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
