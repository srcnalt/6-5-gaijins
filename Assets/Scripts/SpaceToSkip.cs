using UnityEngine;

public class SpaceToSkip : MonoBehaviour
{
    [SerializeField] private GameObject enable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
            enable.SetActive(true);
        }
    }
}
