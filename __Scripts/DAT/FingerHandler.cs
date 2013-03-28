using UnityEngine;
using System.Collections;

public class FingerHandler : MonoBehaviour {
	
	static public GameObject fingerSpot;
	static public LayerMask fingerSpotMask;
	
	public GameObject fingerSpotPrefab;
	public LayerMask spotMask;
	
	// Use this for initialization
	void Awake ()
	{
		fingerSpot = fingerSpotPrefab;
		fingerSpotMask = spotMask;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
