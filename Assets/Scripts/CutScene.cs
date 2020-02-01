using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Image blackScreen;

    private void Update()
    {
        switch (PlayerController.mode)
        {
            case GameMode.CutScene:
                StartCutScene();
                PlayerController.mode = GameMode.None;
                break;
        }
    }

    public void StartCutScene()
    {
        StartCoroutine(StartCutSceneAsync());
    }

    private IEnumerator StartCutSceneAsync()
    {
        Color color = blackScreen.color;
        while (blackScreen.color.a < 1)
        {
            color.a += Time.deltaTime;
            blackScreen.color = color;
            yield return null;
        }
    }
}
