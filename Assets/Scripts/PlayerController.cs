using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Score score;

    private Hashtable controlKeys = new Hashtable();

    private bool isReturning;
    public static GameMode mode = GameMode.Break;

    private float speed = 5;
    private float acceleration = 0.2f;
    private float xSpeed = 0;
    private float initialX = 2.5f;

    private float leftWall = -1;
    private float rightWall = 1;

    private void Start()
    {
        controlKeys = MapControls(playerType);
        Prepare();
    }

    private void Prepare()
    {
        float initialPos = transform.position.x;
        leftWall = -1 + transform.localScale.x / 2 + initialPos;
        rightWall = 1 - transform.localScale.x / 2 + initialPos;
    }

    private void Update()
    {
        switch (mode)
        {
            case GameMode.Break:
                MovePlayer();
                break;
            case GameMode.CutScene:
                EndCutScene();
                break;
            case GameMode.Repair:
                MovePlayer();
                break;
            case GameMode.GameOver:
                break;
        }
    }

    private void EndCutScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRepairMode();
        }
    }

    private void MovePlayer()
    {
        if (GetKey(MoveKey.Left))
            xSpeed -= acceleration;
        else if (GetKey(MoveKey.Right))
            xSpeed += acceleration;
        else
            xSpeed -= xSpeed / 10;
       
        xSpeed = Mathf.Clamp(xSpeed, -2, 2);
        float moveX = Time.deltaTime * xSpeed;
        float nextX = transform.localPosition.x + moveX;
        if ((nextX < leftWall) || (nextX > rightWall))
        {
            xSpeed = -xSpeed / 3;
            xSpeed = Mathf.Abs(xSpeed) < 0.2 ? 0 : xSpeed;
            moveX = Time.deltaTime * xSpeed;
        }

        transform.position += new Vector3(0, 0, Time.deltaTime * speed);
        transform.localPosition += new Vector3(moveX, 0, 0);
    }

    private void StartRepairMode()
    {
        mode = GameMode.Repair;
        speed = -speed;
        acceleration = -acceleration;
        Vector3 nextPos = transform.position;
        nextPos.x = nextPos.x < 0 ? initialX : -initialX;
        transform.position = nextPos;
        transform.rotation = new Quaternion(0, 90, 0, 0);
        Prepare();
    }

    private void GameOver()
    { 
        mode = GameMode.GameOver;
        CalculateScore();
    }

    private int CalculateScore()
    {
        return 0;
    }

    bool sentinel = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!sentinel)
        {
            sentinel = true;
            if (other.CompareTag("Breakable"))
            {
                BreakableObject breakable = other.transform.parent.GetComponent<BreakableObject>();

                if(!breakable.isBroken)
                {
                    if (isReturning)
                    {
                        score.Destroyed(-1);
                    }
                    else
                    {
                        score.Destroyed(1);
                    }
                }
                else if (breakable.isBroken && isReturning)
                {
                    score.Repaired(1);
                }

                breakable.SwitchState();
            }
            else if (other.CompareTag("CutSceneRoom"))
            {
                isReturning = true;
                transform.Rotate(new Vector3(0, 180, 0));

                mode = GameMode.CutScene;
                //if (orientation == Orientation.Left) orientation = Orientation.Right;
                //else if (orientation == Orientation.Right) orientation = Orientation.Left;

                //Vector3 position = transform.position;
                //if (playerType == PlayerType.PlayerOne) position.x = 5;
                //else position.x = 0;
                //transform.position = position; 
            }
            else if (other.CompareTag("EndWall"))
            {
                Debug.Log("Game Over");
                //gameOver = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sentinel)
        {
            sentinel = false;
        }
    }

    private Hashtable MapControls(PlayerType type)
    {
        Hashtable controlKeys = new Hashtable();
        if (type == PlayerType.PlayerOne)
        {
            controlKeys[MoveKey.Left] = KeyCode.A;
            controlKeys[MoveKey.Right] = KeyCode.D;
        }
        else
        {
            controlKeys[MoveKey.Left] = KeyCode.J;
            controlKeys[MoveKey.Right] = KeyCode.L;
        }
        return controlKeys;
    }

    private bool GetKey(MoveKey key)
    {
        return Input.GetKey((KeyCode)controlKeys[key]);
    }

}
