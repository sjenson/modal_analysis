using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class excitor : MonoBehaviour {

	public int timeTilDead = 5;

	void Start () {
		Destroy (gameObject, timeTilDead);
	}

}
