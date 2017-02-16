using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class geoInstrument : MonoBehaviour {

	#region public vars
	public GameObject excitor;

	#endregion


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKeyDown("space")) {
			Instantiate(excitor);
		}

	}

}
