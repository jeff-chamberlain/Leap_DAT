using UnityEngine;
using System.Collections;

public class CenterTest : MonoBehaviour {
	
	static public bool tracking;
	
	private ArrayList centeredFingers = new ArrayList();
	
	// Use this for initialization
	void Start () {
	
	}
	public bool Test()
	{
		switch( centeredFingers.Count )
		{
		case 0:
			tracking = false;
			return false;
		case 1:
			GameObject trackedFinger = ( GameObject )centeredFingers[0];
			if( (trackedFinger != null ) )
			{
				return true;
			}
			else {
				centeredFingers.RemoveAt( 0 );
				return false;
			}
		default:
			for( int i = 0; i < centeredFingers.Count; i ++ )
			{
				GameObject thisFinger = ( GameObject )centeredFingers[i];
				if( (thisFinger == null ) )
				{
					centeredFingers.RemoveAt( i );
				}
			}
			return false;
		}
	}
	void OnCollisionEnter( Collision col )
	{
		if( col.gameObject.tag == "FingerSpot" )
		{
			tracking = true;
			col.gameObject.GetComponent<FingerSpot>().tracked = true;
			centeredFingers.Add( col.gameObject );
		}
	}
	void OnCollisionExit( Collision col )
	{
		if( col.gameObject.tag == "FingerSpot" )
		{
			if( !TestingManager.testing )
			{
				col.gameObject.GetComponent<FingerSpot>().tracked = false;
			}
			centeredFingers.Remove( col.gameObject );
		}
	}
}
