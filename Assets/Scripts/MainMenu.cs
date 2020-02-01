using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class MainMenu : MonoBehaviour
{
	public bool isStart;
	public bool isQuit;
	public bool isCredits;
	public GameObject mController;
	
	int numOfQuits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//credits, main menu, sample scene
    void OnMouseUp() {
		if(isStart)
		{
			GetComponent<Renderer>().material.color=Color.blue;
			mController.GetComponent<CameraMove>().StartFadeTo(2);
			//Application.LoadLevel(2);
		}
		else if (isCredits) {
			GetComponent<Renderer>().material.color=Color.blue;
			mController.GetComponent<CameraMove>().StartFadeTo(0);
			// while(mController.GetComponent<CameraMove>().state != 2){
			// 	//
			// }
			// Application.LoadLevel(0);
		}
		else if (isQuit)
		{
			Application.Quit();
		}
	}

}
