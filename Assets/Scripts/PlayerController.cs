using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerType playerType;

    private float speed = 1;
    private float switchSpeed = 1;
    private bool isMoving;
    private Orientation orientation = Orientation.Center;

    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, Time.deltaTime * speed);

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
                transform.localPosition += new Vector3(moveDir * Time.deltaTime * switchSpeed, 0, 0);
                yield return null;
            }

            isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakable"))
        {
            other.transform.parent.GetComponent<BreakableObject>().SwitchState();
        }
    }
}
