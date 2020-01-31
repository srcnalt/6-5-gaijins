using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ObjectOrientation orientation;

    private void Start()
    {
        orientation = (ObjectOrientation)Random.Range(0, 3);
        transform.position += new Vector3(0, meshRenderer.bounds.extents.y, 0);

        switch (orientation)
        {
            case ObjectOrientation.Left:
                transform.position += new Vector3(-1 + meshRenderer.bounds.extents.x, 0, 0);
                break;
            case ObjectOrientation.Right:
                transform.position += new Vector3(1 - meshRenderer.bounds.extents.x, 0, 0);
                break;
            case ObjectOrientation.Center:
                int offsetDirection = Random.Range(-1, 2);
                transform.position += new Vector3(0.2f * offsetDirection, 0, 0);
                break;
        }
    }
}
