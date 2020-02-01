using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Score score;

    private Hashtable controlKeys = new Hashtable();

    private bool gameOver;
    private bool isReturning;

    private const float speed = 5;
    private const float acceleration = 0.2f;
    private float xSpeed = 0;


    private float leftWall = -1;
    private float rightWall = 1;

    private void Start()
    {
        controlKeys = MapControls(playerType);
        float initialPos = transform.position.x;
        leftWall += transform.localScale.x / 2 + initialPos;
        rightWall += -transform.localScale.x / 2 + initialPos;
    }

    private void Update()
    {
        if (gameOver) return;
        MovePlayer();
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
        float nextX = Time.deltaTime * xSpeed;
        if (IsHitWall(transform.localPosition.x + nextX))
        {
            xSpeed = -xSpeed / 3;
            xSpeed = Mathf.Abs(xSpeed) < 0.2 ? 0 : xSpeed;
            nextX = Time.deltaTime * xSpeed;
        }

        transform.position += new Vector3(0, 0, Time.deltaTime * speed);
        transform.localPosition += new Vector3(nextX, 0, 0);
    }

    private bool IsHitWall(float xPos)
    {
        return (xPos < leftWall) || (xPos > rightWall);
    }

    private bool GetKey(MoveKey key)
    {
        return Input.GetKey((KeyCode)controlKeys[key]);
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
            else if (other.CompareTag("EndWall"))
            {
                other.gameObject.SetActive(false);

                isReturning = true;
                transform.Rotate(new Vector3(0, 180, 0));

                //if (orientation == Orientation.Left) orientation = Orientation.Right;
                //else if (orientation == Orientation.Right) orientation = Orientation.Left;

                Vector3 position = transform.position;
                if (playerType == PlayerType.PlayerOne) position.x = 5;
                else position.x = 0;
                transform.position = position; 
            }
            else if (other.CompareTag("StartWall"))
            {
                Debug.Log("Game Over");
                gameOver = true;
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
}
