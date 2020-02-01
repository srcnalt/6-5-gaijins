using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text score;

    public int scoreVal = 0;

    public void AddScore(int value)
    {
        scoreVal += value;
        score.text = scoreVal.ToString();
    }
}
