using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour {
	
	private GameObject spot;
	private Ray fingerRay;
	private bool tracked;
	
	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if( tracked )
		{
			Ray fingerRay = new Ray( this.transform.position, this.transform.forward );
			switch( Main.state )
			{
			case "testing":
				RaycastHit hit;
				Vector3 spotLoc;
				if( Physics.Raycast( fingerRay, out hit, Mathf.Infinity, FingerHandler.fingerSpotMask  ) )
				{
					spotLoc = hit.point;
				}
				else
				{
					Vector3 spotEsc = fingerRay.GetPoint ( 20.0F );
					spotLoc = new Vector3( spotEsc.x, spotEsc.y, 0.0F );
				}
				spot.transform.position = spotLoc;
				break;
			default:
				break;
			}
		}
	}
	public void Found()
	{
		tracked = true;
		if( Main.state != "calibrating" )
		{
			RaycastHit hit;
			Vector3 spotLoc;
			if( Physics.Raycast( fingerRay, out hit, Mathf.Infinity, FingerHandler.fingerSpotMask  ) )
			{
				spotLoc = hit.point;
			}
			else
			{
				Vector3 spotEsc = fingerRay.GetPoint ( 20.0F );
				spotLoc = new Vector3( spotEsc.x, spotEsc.y, 0.0F );
			}
			spot = Instantiate( FingerHandler.fingerSpot, spotLoc, Quaternion.identity ) as GameObject;
		}
	}
	public void Lost()
	{
		tracked = false;
		if( Main.state != "calibrating" )
		{
			Destroy( spot );
		}
	}
}
