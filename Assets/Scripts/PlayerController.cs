using System;
using System.Collections;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    public GameObject AudioManager;
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Score score;
    [SerializeField] private TrapsUsed trapsUsed;
    [SerializeField] private Transform ram;
    [SerializeField] private Camera camera;

    [SerializeField] private GameObject[] traps;

    private Hashtable controlKeys = new Hashtable();

    private bool isReturning;
    public static GameMode mode = GameMode.Instructions;

    private float speed = 5;
    private float acceleration = 0.2f;
    private float xSpeed = 0;
    private float initialX = 2.5f;
    private float baseRotation = 20;

    private float leftWall = -1;
    private float rightWall = 1;

    private PlayerState state = PlayerState.Run;
    private float jumpCtr = 0;

    private Animator animator;

    private void Start()
    {
        controlKeys = MapControls(playerType);
        Prepare();
        animator = ram.GetComponent<Animator>();
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
                animator.SetBool("Run", true);
                animator.SetBool("Jump", false);
                MovePlayer();
                DropTrap();
                break;
            case GameMode.CutScene:
                animator.SetBool("Run", false);
                animator.SetBool("Jump", false);
                break;
            case GameMode.Repair:
                animator.SetBool("Run", true);
                animator.SetBool("Jump", false);
                MovePlayer();
                break;
            case GameMode.GameOver:
                animator.SetBool("Run", false);
                animator.SetBool("Jump", false);
                break;
        }
    }

    private int count;
    private bool trapCooldown;
    private void DropTrap()
    {
        if (GetKey(MoveKey.Trap) && !trapCooldown && count < 5)
        {
            count++;
            trapCooldown = true;
            GameObject instance = Instantiate(traps[UnityEngine.Random.Range(0, traps.Length)]);
            instance.transform.position = transform.position;

            trapsUsed.setValue(count);

            Invoke("TrapReady", 0.5f);
        }
    }

    private void TrapReady()
    {
        trapCooldown = false;
    }

    private void MovePlayer()
    {
        Debug.Log("Player" + playerType.ToString() + tag);
        if (tag != "Untagged")
        {
            if (tag == "Left")
            {
                state = PlayerState.TurnLeft;
            }
            else if (tag == "Right")
            {
                state = PlayerState.TurnRight;
            }
            else if (tag == "Jump")
            {
                state = PlayerState.Jump;
            }
            tag = "Untagged";
        }
        float yJump = 0;
        if (GetKeyDown(MoveKey.Jump) || state == PlayerState.Jump)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
            state = PlayerState.Jump;
            jumpCtr += Time.deltaTime * 5;
            yJump = Mathf.Sin(jumpCtr) * 0.6f;
            if (yJump < 0)
            {
                yJump = 0;
                jumpCtr = 0;
                state = PlayerState.Run;
                animator.SetBool("Run", true);
                animator.SetBool("Jump", false);
            }
        }
        else if(GetKey(MoveKey.Left) || state == PlayerState.TurnLeft)
        {
            xSpeed -= acceleration;
            state = PlayerState.Run;
        }
        else if (GetKey(MoveKey.Right) || state == PlayerState.TurnRight)
        {
            xSpeed += acceleration;
            state = PlayerState.Run;
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
        Vector3 localPos = transform.localPosition + new Vector3(moveX, 0, 0);
        localPos.y = yJump;
        transform.localPosition = localPos;
    }

    public void StartRepairMode()
    {
        speed = -7;
        acceleration = -2.2f;
        baseRotation = -baseRotation;
        Vector3 nextPos = transform.position;
        nextPos.x = nextPos.x < 0 ? initialX : -initialX;
        transform.position = nextPos;
        transform.Rotate(new Vector3(0, 180, 0));
        tag = "Repair";
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
                    CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, .3f);
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
                    AudioManager.GetComponent<AudioSource>().PlayOneShot(AudioManager.GetComponent<AudioLoader>().GetFixingSound(),0.5f); // plays random fixing sound
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
            controlKeys[MoveKey.Jump] = KeyCode.W;
        }
        else
        {
            controlKeys[MoveKey.Left]  = KeyCode.J;
            controlKeys[MoveKey.Right] = KeyCode.L;
            controlKeys[MoveKey.Trap]  = KeyCode.K;
            controlKeys[MoveKey.Jump] = KeyCode.I;
        }
        return controlKeys;
    }

    private bool GetKey(MoveKey key)
    {
        return Input.GetKey((KeyCode)controlKeys[key]);
    }

    private bool GetKeyDown(MoveKey key)
    {
        return Input.GetKeyDown((KeyCode)controlKeys[key]);
    }

}