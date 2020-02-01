using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] private BoxCollider[] buttons;

    private bool start;

    private void Start()
    {
        camera.transform.position = idleLocation.position;
        camera.transform.rotation = idleLocation.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !start)
        {
            start = true;

            buttons[0].enabled = true;
            buttons[1].enabled = true;

            StartCoroutine(FocusToMenu());
        }
    }

    private IEnumerator FocusToMenu()
    {
        logo.SetActive(false);
        float focus = 0;

        while (focus < 0.9f)
        {
            focus += Time.deltaTime / 3;

            camera.transform.position = Vector3.Lerp(camera.transform.position, focusLocation.position, focus);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, focusLocation.rotation, focus);

            yield return null;
        }
    }

    public void Credits()
    {
        StopAllCoroutines();
        StartCoroutine(FocusToCredits());
    }

    private IEnumerator FocusToCredits()
    {
        float progress = 0;

        while (progress < 0.9f)
        {
            progress += Time.deltaTime / 3;

            camera.transform.position = Vector3.Lerp(camera.transform.position, creditsLocation.position, progress);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, creditsLocation.rotation, progress);

            yield return null;
        }
    }

    public void Game()
    {
        StopAllCoroutines();
        StartCoroutine(FocusToGame());
    }

    private IEnumerator FocusToGame()
    {
        float progress = 0;

        while (progress < 0.9f)
        {
            progress += Time.deltaTime / 3;

            blackScreen.color = Color.Lerp(blackScreen.color, Color.black, progress);

            camera.transform.position = Vector3.Lerp(camera.transform.position, finalLocation.position, progress);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, finalLocation.rotation, progress);

            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}