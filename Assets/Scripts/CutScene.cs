using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject[] pages;
    private int pageIndex = 0;

    private void Update()
    {
        switch (PlayerController.mode)
        {
            case GameMode.CutScene:
                StartCutScene();
                PlayerController.mode = GameMode.None;
                break;
            case GameMode.None:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangePages();
                }
                break;
        }
    }

    public void StartCutScene()
    {
        StartCoroutine(StartCutSceneAsync(true));
    }

    private IEnumerator StartCutSceneAsync(bool fade)
    {
        Color color = blackScreen.color;

        if (fade)
        {
            while (blackScreen.color.a < 1)
            {
                color.a += Time.deltaTime;
                blackScreen.color = color;
                yield return null;
            }

            ChangePages();
        }
        else
        {
            pages[pages.Length - 1].SetActive(false);

            var players = FindObjectsOfType<PlayerController>();
            players[0].StartRepairMode();
            players[1].StartRepairMode();

            while (blackScreen.color.a > 0)
            {
                color.a -= Time.deltaTime;
                blackScreen.color = color;
                yield return null;
            }

            PlayerController.mode = GameMode.Repair;
        }
    }

    private void ChangePages()
    {
        if (pageIndex < pages.Length)
        {
            if (pageIndex != 0)
                pages[pageIndex - 1].SetActive(false);

            pages[pageIndex].SetActive(true);

            pageIndex++;
        }
        else
        {
            StartCoroutine(StartCutSceneAsync(false));
        }
    }
}
