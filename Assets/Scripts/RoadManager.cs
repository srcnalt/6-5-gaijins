using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private const int RoadCount = 12;
    private readonly Vector3 PieceLength = new Vector3(0, 0, 6);
    private readonly Vector3 PieceForwardOffset = new Vector3(0, 0, 2);

    [SerializeField] private GameObject roadPiecePrefab;
    [SerializeField] private Transform laneOne;
    [SerializeField] private Transform laneTwo;
    [SerializeField] private GameObject endWallPrefab;
    [SerializeField] private GameObject startWallPrefab;
    [SerializeField] private GameObject[] breakableObjects;

    private void Start()
    {
        CreateLevel(laneOne);
        CreateLevel(laneTwo);
    }

    private void CreateLevel(Transform lane)
    {
        GameObject startWall = Instantiate(startWallPrefab, lane);
        startWall.transform.position += PieceLength * -0.5f;

        for (int i = 0; i < RoadCount; i++)
        {
            GameObject instance = Instantiate(roadPiecePrefab, lane);
            instance.transform.position += PieceLength * i;
        }

        for (int i = 3; i < RoadCount * 3 - 3; i++)
        {
            int index = Random.Range(0, breakableObjects.Length);
            GameObject instance = Instantiate(breakableObjects[index], lane);
            instance.transform.position += PieceForwardOffset * i;
        }

        GameObject endWall = Instantiate(endWallPrefab, lane);
        endWall.transform.position += PieceLength * (RoadCount - 0.5f);
    }
}
