using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class MainMenu : MonoBehaviour
{
	public bool isStart;
	public bool isQuit;
	public bool isCredits;
	public bool isMain;
	public GameObject mController;

	public bool isInvis = true;
	
	int numOfQuits;
    // Start is called before the first frame update
    void Start()
    {
		this.GetComponent<MeshRenderer>().enabled = false;
    }
    void Update()
    {
        
    }
    void OnMouseUp() {
		if(isStart)
		{
			mController.GetComponent<CameraMove>().StartFadeTo(0);
			//Application.LoadLevel(2);
		}
		else if (isCredits) {
			//mController.GetComponent<CameraMove>().StartFadeTo(0);
			// while(mController.GetComponent<CameraMove>().state != 2){
			// 	//
			// }
			// Application.LoadLevel(0);
			mController.GetComponent<CameraMove>().StartMoveTo(mController.GetComponent<CameraMove>().creditsTransf);
			mController.GetComponent<CameraMove>().state = 3;
		}
		else if(isMain) {
			mController.GetComponent<CameraMove>().StartMoveTo(mController.GetComponent<CameraMove>().mainTransf);
			mController.GetComponent<CameraMove>().state = 1;
		}
		else if (isQuit)
		{
			Application.Quit();
		}
	}

	void OnMouseEnter() {
		this.GetComponent<Renderer>().enabled = true;
	}

	void OnMouseExit() {
		this.GetComponent<Renderer>().enabled = false;
	}

}
