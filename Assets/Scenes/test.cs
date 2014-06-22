using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("test path"), "time", 10));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
