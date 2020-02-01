using System.Collections;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    private bool isCountdown;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject info;

    private void Start()
    {
        PlayerController.mode = GameMode.Instructions;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCountdown)
        {
            StartCoroutine(Countdown());
        }
    }

    private IEnumerator Countdown()
    {
        isCountdown = true;

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        countdown.SetActive(true);

        yield return new WaitForSeconds(4.5f);

        PlayerController.mode = GameMode.Break;

        info.SetActive(true);
    }
}
