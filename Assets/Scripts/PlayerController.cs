using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;
    [SerializeField] private Score score;

    private float speed = 5;
    private float switchSpeed = 3;
    private bool isMoving;
    private bool isReturning;
    private Orientation orientation = Orientation.Center;

    private void Update()
    {
        transform.position += new Vector3(0, 0, Time.deltaTime * speed * transform.forward.z);

        if (!isMoving)
        {
            if (GetKey(MoveKey.Left))
            {
                StartCoroutine(MovePlayer(Orientation.Left));
            }
            else if (GetKey(MoveKey.Right))
            {
                StartCoroutine(MovePlayer(Orientation.Right));
            }
        }
    }

    private bool GetKey(MoveKey key)
    {
        if(playerType == PlayerType.PlayerOne)
        {
            if (key == MoveKey.Left) 
                return Input.GetKey(KeyCode.A);
            else
                return Input.GetKey(KeyCode.D);
        }
        else
        {
            if (key == MoveKey.Left)
                return Input.GetKey(KeyCode.J);
            else
                return Input.GetKey(KeyCode.L);
        }
    }

    private IEnumerator MovePlayer(Orientation direction)
    {
        if (direction == orientation)
        {
            yield return null;
        }
        else
        {
            isMoving = true;

            if (direction == Orientation.Left)
            {
                if (orientation != Orientation.Left)
                {
                    orientation--;
                }
            }
            else if (direction == Orientation.Right)
            {
                if (orientation != Orientation.Right)
                {
                    orientation++;
                }
            }

            float progress = 0;
            int moveDir = (direction == Orientation.Right) ? 1 : -1;

            while (progress < 0.8f)
            {
                progress += Time.deltaTime * switchSpeed;
                transform.localPosition += new Vector3(moveDir * Time.deltaTime * switchSpeed * transform.right.x, 0, 0);
                yield return null;
            }

            isMoving = false;
        }
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

                if (orientation == Orientation.Left) orientation = Orientation.Right;
                else if (orientation == Orientation.Right) orientation = Orientation.Left;

                Vector3 position = transform.position;
                if (playerType == PlayerType.PlayerOne) position.x = 5;
                else position.x = 0;
                transform.position = position; 
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
}
