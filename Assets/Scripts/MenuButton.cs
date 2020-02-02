using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public UnityEvent OnClicked;
    public Text label;

    private void OnMouseDown()
    {
        OnClicked.Invoke();
    }

    private void OnMouseOver()
    {
        label.color = Color.white;
    }

    private void OnMouseExit()
    {
        label.color = Color.black;
    }
}
