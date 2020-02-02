using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform laneOne;
    [SerializeField] private Transform laneTwo;

    public Stack<GameObject> obstacles1;
    public Queue<GameObject> obstacles2;

    public static GameMode mode = GameMode.Break;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("UpdateAI");
        if (mode == GameMode.Break)
        {
            BreakAI();
        } else
        {
            RepairAI();
        }
        if (player.tag == "Repair")
        {
            mode = GameMode.Repair;
        }
    }

    void RepairAI()
    {
        for (int i = 0; i < laneOne.childCount; i++)
        {
            Transform child = laneOne.GetChild(i);
            float diff = player.transform.position.z - child.transform.position.z;
            if (diff <= 2 && diff > 0)
            {
                float xPos = player.transform.position.x - child.transform.position.x;
                string moveLeft = (child.tag == "Fixable") ? "Left" : "Right";
                string moveRight = (child.tag == "Fixable") ? "Right" : "Left";
                player.tag = (xPos < 0) ? moveLeft : player.tag;
                player.tag = (xPos > 0) ? moveRight : player.tag;
                break;
            }

        }
    }

    void BreakAI()
    {
        for (int i = 0; i < laneOne.childCount; i++)
        {
            Transform child = laneTwo.GetChild(i);
            float diff = child.transform.position.z - player.transform.position.z;
            if (diff <= 2 && diff > 0)
            {
                float xPos = player.transform.position.x - child.transform.position.x;
                player.tag = (xPos < 0) ? "Right" : player.tag;
                player.tag = (xPos > 0) ? "Left" : player.tag;
                break;
            }

        }
    }

}
