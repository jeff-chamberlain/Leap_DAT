using UnityEngine;
using System.Collections;

public class FingerSpot : MonoBehaviour {
	
	public bool tracked;
	
	private Light fingerLight;
	
	// Use this for initialization
	void Start () {
		fingerLight = this.gameObject.GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		if( CenterTest.tracking || TestingManager.testing )
		{
			if( tracked )
			{
				fingerLight.color = Color.green;
				fingerLight.enabled = true;
			}
			else
			{
				fingerLight.enabled = false;
			}
		}
		else
		{
			fingerLight.color = Color.white;
			fingerLight.enabled = true;
		}
		if( TestingManager.testing && tracked )
		{
			string positionMessage = "Position at " + Mathf.RoundToInt( Time.time * 1000 ).ToString() + " ms: x(" +
				this.transform.position.x.ToString() + "), y(" + this.transform.position.y.ToString() + ")"
				+ System.Environment.NewLine;
			System.IO.File.AppendAllText( Main.path, positionMessage );
		}
	}
}
