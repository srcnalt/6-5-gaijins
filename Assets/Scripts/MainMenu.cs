using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class MainMenu : MonoBehaviour
{
	public bool isStart;
	public bool isQuit;
	public bool isCredits;
	public GameObject mController;

	public bool isInvis = true;
	
	int numOfQuits;
    // Start is called before the first frame update
    void Start()
    {
		this.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//credits, main menu, sample scene
    void OnMouseUp() {
		if(isStart)
		{
			mController.GetComponent<CameraMove>().StartFadeTo(2);
			//Application.LoadLevel(2);
		}
		else if (isCredits) {
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
