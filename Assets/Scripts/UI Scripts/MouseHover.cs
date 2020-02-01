using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
	bool isInvis = false;
    void Start() {
		//
	}

	void OnMouseEnter() {
		if(this.GetComponent<Renderer>().enabled == false) {
			isInvis = true;
		}
		else {
			GetComponent<Renderer>().material.color = Color.cyan;
		}
		this.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit() {
		if(isInvis == true) {
			this.GetComponent<Renderer>().enabled = false;
		}
		else {
			this.GetComponent<Renderer>().material.color=Color.white;
		}
	}
}
