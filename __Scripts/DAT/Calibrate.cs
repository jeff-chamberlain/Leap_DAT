using UnityEngine;
using System.Collections;

public class Calibrate : MonoBehaviour {
	
	static public string state = "calibrateCenter";
	
	public GameObject calibrateIcon, calibrateWall, input;
	public float calibrateThreshold;
	public LayerMask centerMask, distMask;
	
	private float calibrateTimer;
	private int calibrateIndex = 0;
	private Vector3 falseLoc = new Vector3( 0.0F, 0.0F, -10.0F );
	private GameObject calibrateObject;
	private ArrayList calibrateValues = new ArrayList();
	private LayerMask calibrateMask;
	private CalibrateIcon iconScript;
	
	// Use this for initialization
	void Start ()
	{
		iconScript = calibrateIcon.GetComponent<CalibrateIcon>();
	}
	
	public void Run()
	{
		switch( state )
		{
		case "calibrateCenter":
			if( calibrateTimer < 2.0F )
			{
				calibrateMask = centerMask;
				RunCalibrate();
			}
			else
			{
				Vector3 sum = Vector3.zero;
				for( int i = 0; i < calibrateValues.Count; i ++ )
				{
					sum += ( Vector3 )calibrateValues[ i ];
				}
				Main.center = sum / calibrateValues.Count;
				calibrateValues.Clear();
				calibrateObject = null;
				calibrateTimer = 0.0F;
				calibrateIcon.transform.position = new Vector3( -5.0F, 0.0F, 0.0F );
				state = "calibrateDist";
			}
			break;
		case "calibrateDist":
			if( calibrateTimer < 2.0F )
			{
				calibrateMask = centerMask;
				RunCalibrate();
			}
			else
			{
				Vector3 sum = Vector3.zero;
				for( int i = 0; i < calibrateValues.Count; i ++ )
				{
					sum += ( Vector3 )calibrateValues[ i ];
				}
				Vector3 ave = sum / calibrateValues.Count;
				float dist = Mathf.Abs( Main.center.x - ave.x );
				Main.scale = 5.0F * ( 0.02F / dist );
				Leap.UnityVectorExtension.InputScale = new Vector3( Main.scale, Main.scale, Main.scale );
				input.transform.position = new Vector3( input.transform.position.x,
					input.transform.position.y, -Main.scale * ( 5.0F / 0.02F ) );
				calibrateValues.Clear();
				calibrateObject = null;
				calibrateTimer = 0.0F;
				calibrateIcon.transform.position = new Vector3( 0.0F, 0.0F, 0.0F );
				state = "secondCenter";
			}
			break;
		case "secondCenter":
			if( calibrateTimer < 2.0F )
			{
				calibrateMask = centerMask;
				RunCalibrate();
				break;
			}
			else
			{
				Vector3 sum = Vector3.zero;
				for( int i = 0; i < calibrateValues.Count; i ++ )
				{
					sum += ( Vector3 )calibrateValues[ i ];
				}
				Main.center = sum / calibrateValues.Count;
				goto case "apply";
			}
		case "apply":
			this.transform.position = new Vector3( Main.center.x, Main.center.y, this.transform.position.z );
			Destroy( calibrateIcon );
			Destroy( calibrateWall );
			this.GetComponent<Calibrate>().enabled = false;
			Main.state = "testing";
			for( int i = 0; i < Main.trackedFingers.Count; i ++ )
			{
				GameObject tracked = ( GameObject )Main.trackedFingers[ i ];
				tracked.GetComponent<Finger>().Found();
			}
			state = "exit";
			break;
		default:
			Debug.Log( "Not Calibrating!!!");
			break;
		}
	}
	bool ThresholdTest( ArrayList vals )
	{
		bool test = ( Vector3 )vals[ vals.Count - 1 ] != falseLoc &&
			Vector3.Distance( ( Vector3 )vals[ vals.Count - 1 ], ( Vector3 )vals[ vals.Count - 2 ] ) < calibrateThreshold;
		return test;
	}
	Vector3 findValue( Transform trans )
	{
		RaycastHit hit;
		Ray transRay = new Ray( trans.position, trans.forward );
		if( Physics.Raycast( transRay, out hit, Mathf.Infinity, calibrateMask ) )
		{
			return hit.point;
		}
		else
		{
			return falseLoc;
		}
		
	}
	void RunCalibrate()
	{
		if( Main.trackedFingers.Count == 0 )
		{
			calibrateTimer = 0.0F;
			calibrateIndex = 0;
			calibrateObject = null;
		}
		else
		{
			if( calibrateObject == null )
			{
				calibrateObject = ( GameObject )Main.trackedFingers[ 0 ];
				calibrateValues.Add( findValue( calibrateObject.transform ) );
			}
			else if( Main.trackedFingers.Contains( calibrateObject ) )
			{
				calibrateValues.Add( findValue( calibrateObject.transform ) );
				calibrateTimer += Time.deltaTime;
				if( !ThresholdTest( calibrateValues ) )
				{
					calibrateTimer = 0.0F;
					calibrateValues.Clear();
					if( Main.trackedFingers.Count > calibrateIndex + 1 )
					{
						calibrateObject = ( GameObject )Main.trackedFingers[ calibrateIndex + 1 ];
						calibrateValues.Add( findValue( calibrateObject.transform ) );
					}
					else
					{
						calibrateObject = null;
					}
				}
			}
			else
			{
				calibrateObject = null;
			}
		}
		iconScript.ChangeColor( calibrateTimer / 2.0F );
	}
}
