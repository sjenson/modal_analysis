using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneOnTrigger : MonoBehaviour {

	public string sceneToLoad;
	public int waitTime;

	void OnTriggerEnter(Collider c){
		StartCoroutine ("loadScene");
	}

	void OnTriggerExit(Collider c){
		StopCoroutine ("loadScene");
	}

	private IEnumerator loadScene(){
		yield return new WaitForSeconds (waitTime);
		SceneManager.LoadScene (sceneToLoad);
	}
}
