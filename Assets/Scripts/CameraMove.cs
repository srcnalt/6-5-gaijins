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

    void Start()
    {
    	Camera.main.transform.position = idlePosition.position;
    	canvasGroup = blackCanvas.GetComponent<CanvasGroup>();
    	canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && Camera.main.transform.position != focusPosition.position) {
        	StartCoroutine(MoveToFocus());
        }
        else if(Input.GetKey(KeyCode.Space) ) {
        	StartCoroutine(MoveToEnd());
        }
    }

    private IEnumerator MoveToFocus() {
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, focusPosition.position, Time.deltaTime);
    		print(progress);
    		yield return null;
    	}
    	//
    }

    private IEnumerator MoveToEnd() {
    	float progress = 0;
    	while(progress < 1) {
    		progress += Time.deltaTime;
    		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, endPosition.position, Time.deltaTime);
    		canvasGroup.alpha = progress;
    		yield return null;
    	}
    }
}