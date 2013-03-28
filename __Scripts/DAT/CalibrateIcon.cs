using UnityEngine;
using System.Collections;

public class CalibrateIcon : MonoBehaviour {

	// Use this for initialization
	public void ChangeColor( float alpha ) {
		Color tempCol = this.renderer.materials[1].color;
		tempCol.a = alpha;
		this.renderer.materials[1].color = tempCol;
	}
}
