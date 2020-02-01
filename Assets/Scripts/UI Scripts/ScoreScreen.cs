using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().enabled = true;
        this.GetComponent<Renderer>().material.color=Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp() {
      Application.LoadLevel(1);
    }
	
}
