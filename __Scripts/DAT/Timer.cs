using UnityEngine;
using System.Collections;

public class Timer {
	
	private float startTime, dur;
	
	public Timer( float time, float thisDur ){
		startTime = time;
		dur = thisDur;
	}
	public bool Check( float time )
	{
		if( time - startTime >= dur )
		{
			
			return true;
		}
		else
		{
			return false;
		}
	}
}
