using System;
using System.Collections;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    public GameObject AudioManager;
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Score score;
    [SerializeField] private Transform ram;
    [SerializeField] private Camera camera;

    [SerializeField] private GameObject[] traps;

    private Hashtable controlKeys = new Hashtable();

    private bool isReturning;
    public static GameMode mode = GameMode.Instructions;

    private float speed = 5;
    private float acceleration = 0.12f;
    private float xSpeed = 0;
    private float initialX = 2.5f;
    private float baseRotation = 20;

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
            case GameMode.Instructions:
                break;
            case GameMode.Break:
                MovePlayer();
                DropTrap();
                break;
            case GameMode.CutScene:
                break;
            case GameMode.Repair:
                MovePlayer();
                break;
            case GameMode.GameOver:
                break;
        }
    }

    private bool trapCooldown;
    private void DropTrap()
    {
        if (GetKey(MoveKey.Trap) && !trapCooldown)
        {
            trapCooldown = true;
            GameObject instance = Instantiate(traps[UnityEngine.Random.Range(0, traps.Length)]);
            instance.transform.position = transform.position;

            Invoke("TrapReady", 0.5f);
        }
    }

    private void TrapReady()
    {
        trapCooldown = false;
    }

    private void MovePlayer()
    {
        if (GetKey(MoveKey.Left))
        {
            xSpeed -= acceleration;
        }
        else if (GetKey(MoveKey.Right))
        {
            xSpeed += acceleration;
        }
        else
        {
            xSpeed -= xSpeed / 10;
        }
       
        xSpeed = Mathf.Clamp(xSpeed, -2, 2);
        float moveX = Time.deltaTime * xSpeed;
        float nextX = transform.localPosition.x + moveX;
        if ((nextX < leftWall) || (nextX > rightWall))
        {
            xSpeed = -xSpeed / 2;
            xSpeed = Mathf.Abs(xSpeed) < 0.2 ? 0 : xSpeed;
            moveX = Time.deltaTime * xSpeed;
        }

        ram.rotation = Quaternion.Lerp(ram.rotation, Quaternion.Euler(0, xSpeed * baseRotation, 0) * transform.rotation, Time.deltaTime * 20);
        transform.position += new Vector3(0, 0, Time.deltaTime * speed);
        transform.localPosition += new Vector3(moveX, 0, 0);
    }

    public void StartRepairMode()
    {
        speed = -speed;
        acceleration = -acceleration;
        baseRotation = -baseRotation;
        Vector3 nextPos = transform.position;
        nextPos.x = nextPos.x < 0 ? initialX : -initialX;
        transform.position = nextPos;
        transform.Rotate(new Vector3(0, 180, 0));
        Prepare();
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
                    AudioManager.GetComponent<AudioSource>().PlayOneShot(AudioManager.GetComponent<AudioLoader>().GetBreakingSound(),0.5f); // plays random breaking sound
                    Debug.Log(playerType);
                    CameraShaker.Instance.camera = camera;
                    CameraShaker.Instance.ShakeOnce(3f, 3f, 0.1f, .3f);
                    if (isReturning)
                    {
                        score.AddScore(-1);
                    }
                    else
                    {
                        score.AddScore(1);
                    }
                }
                else if (breakable.isBroken && isReturning)
                {
                    score.AddScore(1);
                }

                breakable.SwitchState();
            }
            else if (other.CompareTag("CutSceneRoom"))
            {
                isReturning = true;
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
                mode = GameMode.GameOver;
                //gameOver = true;
            }
            else if (other.CompareTag("Trap"))
            {
                score.AddScore(-1);
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
            controlKeys[MoveKey.Left]  = KeyCode.A;
            controlKeys[MoveKey.Right] = KeyCode.D;
            controlKeys[MoveKey.Trap]  = KeyCode.S;
        }
        else
        {
            controlKeys[MoveKey.Left]  = KeyCode.J;
            controlKeys[MoveKey.Right] = KeyCode.L;
            controlKeys[MoveKey.Trap]  = KeyCode.K;
        }
        return controlKeys;
    }

    private bool GetKey(MoveKey key)
    {
        return Input.GetKey((KeyCode)controlKeys[key]);
    }
    
}