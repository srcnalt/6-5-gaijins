using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject[] pages;
    private int pageIndex = 0;

    [SerializeField] private Score playerOneScore;
    [SerializeField] private Score playerTwoScore;

    [SerializeField] private GameObject winLose;
    [SerializeField] private Text playerLabel;
    [SerializeField] private Text scoreLabel;

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
            case GameMode.GameOver:
                StartCoroutine(ShowScore());
                break;
            case GameMode.Stop:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerController.mode = GameMode.Break;
                    SceneManager.LoadScene(1);
                }
                break;
        }
    }

    public void StartCutScene()
    {
        StartCoroutine(StartCutSceneAsync(true));
    }

    private IEnumerator ShowScore()
    {
        PlayerController.mode = GameMode.Stop;

        Color color = blackScreen.color;

        while (blackScreen.color.a < 1)
        {
            color.a += Time.deltaTime;
            blackScreen.color = color;
            yield return null;
        }

        winLose.SetActive(true);

        int p1 = playerOneScore.scoreVal;
        int p2 = playerTwoScore.scoreVal;

        if (p1 > p2)
        {
            playerLabel.text = "Player 1 Wins!";
            scoreLabel.text = "Score: " + p1;
        }
        else if (p1 < p2)
        {
            playerLabel.text = "Player 2 Wins!";
            scoreLabel.text = "Score: " + p2;
        }
        else
        {
            playerLabel.text = "Tie!";
            scoreLabel.text = "Score: " + p1;
        }
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
