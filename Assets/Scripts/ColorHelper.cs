using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHelper : MonoBehaviour {

	public static Material black;
	public static Material green;

	public Material blackMat;
	public Material greenMat;

	// Use this for initialization
	void Start () {
		ColorHelper.black = this.blackMat;
		ColorHelper.green = this.greenMat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
