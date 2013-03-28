using UnityEngine;
using System.Collections;

public class TestingManager : MonoBehaviour {
	
	static public float signalOffset;
	static public bool testing;
	
	public float delayDur = 2.0F, cueDur = 0.1F, constantWaitDur = 1.5F, reactDur = 1.5F, signalRadius;
	public GameObject centerTestObj, cue, signal;
	public Material[] cueMaterials;
	public Material invalidSignalMat;
	
	private CenterTest centerScript;
	private string state = "initialize";
	private Timer testTimer;
	private Variables varis;
	private GameObject thisCue, thisSig;
	private Quaternion cylQuat;
	private float rt, signalTime;
	
	// Use this for initialization
	void Start ()
	{
		signalOffset = Mathf.Atan( ( 0.5F * signal.transform.localScale.y ) / signalRadius );
		cylQuat = Quaternion.Euler( new Vector3( 90.0F, 0.0F, 0.0F ) );
	}
	
	// Update is called once per frame
	public void Run()
	{
		switch( state )
		{
		case "initialize":
			GameObject cent = Instantiate( centerTestObj, Main.center, cylQuat ) as GameObject;
			centerScript = cent.GetComponent<CenterTest>();
			state = "centering";
			break;
		case "centering":
			if( centerScript.Test() )
			{
				if( CallTimer( delayDur ) )
				{
					goto case "set variables";
				}
				else
				{
					break;
				}
			}
			else
			{
				testTimer = null;
				break;
			}
		case "set variables":
			varis = new Variables( signalRadius );
			thisCue = Instantiate( cue, Main.center, cylQuat ) as GameObject;
			thisCue.renderer.material = cueMaterials[varis.cueIndex];
			state = "cue";
			goto case "cue";
		case "cue":
			if( centerScript.Test() )
			{
				if( CallTimer( cueDur ) )
				{
					Destroy( thisCue );
					state = "wait";
					goto case "wait";
				}
				else
				{
					break;
				}
			}
			else
			{
				testTimer = null;
				Destroy( thisCue );
				state = "centering";
				break;
			}
		case "wait":
			if( centerScript.Test() )
			{
				if( CallTimer( constantWaitDur ) )
				{
					goto case "signal";
				}
				else
				{
					break;
				}
			}
			else
			{
				testTimer = null;
				state = "centering";
				break;
			}
		case "signal":
			thisSig = Instantiate(signal, varis.signalVec, Quaternion.identity) as GameObject;
			if( !varis.valid )
			{
				thisSig.renderer.material = invalidSignalMat;
			}
			signalTime = Time.time;
			string displayMessage = "Signal displayed at " + Mathf.RoundToInt( Time.time * 1000 ).ToString()
				+ System.Environment.NewLine;
			System.IO.File.AppendAllText( Main.path, displayMessage );
			state = "testing";
			goto case "testing";
		case "testing":
			testing = true;
			if( CallTimer( reactDur ) )
			{
				WriteData( true, -0.001F, -0.001F );
				Destroy( thisSig );
				Reset();
				break;
			}
			else if( !centerScript.Test() )
			{
				if( varis.valid )
				{
					rt = Time.time - signalTime;
					state = "RTFound";
					goto default;
				}
				else
				{
					WriteData( false, -0.001F, Time.time - signalTime );
					Destroy( thisSig );
					Reset();
					break;
				}
			}
			break;
		default:
			break;
		}
	}
	bool CallTimer( float dur )
	{
		if( testTimer == null )
		{
			testTimer = new Timer( Time.time, dur );
			return false;
		}
		else
		{
			if( testTimer.Check( Time.time ) )
			{
				testTimer = null;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
	public void SignalHit( float time )
	{
		WriteData( true, time - signalTime, rt );
		Reset();
	}
	public void Reset()
	{
		state = "centering";
		testing = false;
		testTimer = null;
	}
	private void WriteData( bool success, float hit, float thisRT )
	{
		string angleString = ( Mathf.RoundToInt( varis.angle * Mathf.Rad2Deg ) ).ToString();
		string rtString = ( Mathf.RoundToInt( thisRT * 1000F ) ).ToString();
		string hitTimeString = ( Mathf.RoundToInt( hit * 1000F )).ToString();
		string dataText = "Trial recorded at " + Mathf.RoundToInt( Time.time * 1000F ).ToString() + ":" + "\t" + 
			angleString + "\t" + varis.cueType.ToString() + "\t" + varis.dir.ToString() + "\t" + 
			System.Convert.ToInt16( varis.valid ) + "\t" + System.Convert.ToInt16( success ) + "\t" + hitTimeString + 
			"\t" + rtString + System.Environment.NewLine;
		System.IO.File.AppendAllText( Main.path, dataText );
	}
}
