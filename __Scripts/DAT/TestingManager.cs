using UnityEngine;
using System.Collections;

public class TestingManager : MonoBehaviour {
	
	static public float signalOffset;
	
	public float delayDur = 2.0F, cueDur = 0.1F, constantWaitDur = 1.5F, reachDur = 1.5F, signalRadius;
	public GameObject centerTestObj, cue, signal;
	public Material[] cueMaterials;
	public Material invalidSignalMat;
	
	private CenterTest centerScript;
	private string state = "initialize";
	private Timer testTimer;
	private Variables varis;
	private GameObject thisCue;
	private Quaternion cylQuat;
	
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
			GameObject sig = Instantiate(signal, varis.signalVec, Quaternion.identity) as GameObject;
			if( !varis.valid )
			{
				sig.renderer.material = invalidSignalMat;
			}
			state = "testing";
			goto default;
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
	public void Reset()
	{
		state = "centering";
	}
}
