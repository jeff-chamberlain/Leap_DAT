using UnityEngine;
using System.Collections;

public class CenterTest : MonoBehaviour {
	
	private ArrayList centeredFingers = new ArrayList();
	
	// Use this for initialization
	void Start () {
	
	}
	public bool Test()
	{
		switch( centeredFingers.Count )
		{
		case 0:
			return false;
		case 1:
			if( centeredFingers[0] != null)
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
				print( centeredFingers[i] );
				if( ( GameObject )centeredFingers[i] == null )
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
			centeredFingers.Add( col.gameObject );
		}
	}
	void OnCollisionExit( Collision col )
	{
		if( col.gameObject.tag == "FingerSpot" )
		{
			centeredFingers.Remove( col.gameObject );
		}
	}
}
