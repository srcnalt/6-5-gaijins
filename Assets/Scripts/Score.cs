using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text destroyed;
    [SerializeField] private Text repaired;

    public int destroyedVal = 0;
    public int repairedVal = 0;

    public void Destroyed(int value)
    {
        destroyedVal += value;
        destroyed.text = destroyedVal.ToString();
    }

    public void Repaired(int value)
    {
        repairedVal += value;
        repaired.text = repairedVal.ToString();
    }
}
