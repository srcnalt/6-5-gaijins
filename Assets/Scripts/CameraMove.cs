using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField] private Transform idlePosition;
	[SerializeField] private Transform focusPosition;
	[SerializeField] private Transform endPosition;
	[SerializeField] private Canvas blackCanvas;
	private CanvasGroup canvasGroup;
	public int state; // 0 , 1, and 2

    void Start()
    {
    	Camera.main.transform.position = idlePosition.position;
    	canvasGroup = blackCanvas.GetComponent<CanvasGroup>();
    	canvasGroup.alpha = 0;
    	state = 0;
    }

    // Update is called once per frame
    void Update()
    {
    	Debug.Log(Camera.main.transform.position.x);
    	Debug.Log(focusPosition.position.x);
        if(Input.GetKey(KeyCode.Space) && state == 0) {
        	StartCoroutine(MoveToFocus());
        }
    }
	public void StartFadeTo(int sceneNumber) {
		StartCoroutine(MoveToEnd(sceneNumber));
	}

    private IEnumerator MoveToFocus() {
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, focusPosition.position, Time.deltaTime);
    		yield return null;
    	}
    	if(progress >=1) {
    		state = 1;
    	}
    	//
    }

    public IEnumerator MoveToEnd(int sceneNumber) {
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, endPosition.position, Time.deltaTime);
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