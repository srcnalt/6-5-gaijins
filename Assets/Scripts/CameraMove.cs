using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform idleLocation;
    [SerializeField] private Transform focusLocation;
    [SerializeField] private Transform finalLocation;
    [SerializeField] private Transform creditsLocation;
    
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject logo;
    [SerializeField] private Camera camera;

    private void Start()
    {
        camera.transform.position = idleLocation.position;
        camera.transform.rotation = idleLocation.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FocusToMenu());
        }
    }

    private IEnumerator FocusToMenu()
    {
        logo.SetActive(false);
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime / 3;

            camera.transform.position = Vector3.Lerp(camera.transform.position, focusLocation.position, progress);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, focusLocation.rotation, progress);

            yield return null;
        }
    }

    public void Credits()
    {
        StartCoroutine(FocusToCredits());
    }

    private IEnumerator FocusToCredits()
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime / 3;

            camera.transform.position = Vector3.Lerp(camera.transform.position, creditsLocation.position, progress);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, creditsLocation.rotation, progress);

            yield return null;
        }
    }
}