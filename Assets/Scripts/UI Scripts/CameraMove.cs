using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private Transform startTransf;
	public Transform mainTransf;
	[SerializeField] private Transform endTransf;
	public Transform creditsTransf;
	[SerializeField] private Canvas blackCanvas;
	private CanvasGroup canvasGroup;
	public int state; // 0 , 1, 2, and 3 correspond to different camera positions

    void Start()
    {
    	Camera.main.transform.position = startTransf.position;
    	canvasGroup = blackCanvas.GetComponent<CanvasGroup>();
    	canvasGroup.alpha = 0;
    	state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && state == 0) {  // checks if space bar pressed while in intro scene
        	StartCoroutine(MoveToFocus(mainTransf));
        }
    }
	public void StartFadeTo(int sceneNumber) {  // initializes fade to black on fridgeand loading of input scene
		StartCoroutine(MoveToBlack(sceneNumber));
	}

	public void StartMoveTo(Transform targetTransf) {
		StartCoroutine(MoveToFocus(targetTransf));
	}

    private IEnumerator MoveToFocus(Transform targetTransf) {  // moves camera to focused transform's position
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetTransf.position, progress);
    		yield return null;
    	}
    }

    public IEnumerator MoveToBlack(int sceneNumber) {  //fades to black (in fridge), then loads scene of specified index
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, endTransf.position, Time.deltaTime);
    		canvasGroup.alpha = progress;
    		yield return null;
    	}
    	if(progress >= 1) {
    		state = 2;
			if(sceneNumber >= 0) {
				Application.LoadLevel(sceneNumber);
			}
    	}
    }
}