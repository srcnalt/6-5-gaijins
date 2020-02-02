using UnityEngine;
using UnityEngine.UI;

public class TrapsUsed : MonoBehaviour
{
    [SerializeField] private Text trapsUsed;

    public int scoreVal = 0;

    public void setValue(int value)
    {
        trapsUsed.text = (5-value).ToString() + "/5";
    }
}
