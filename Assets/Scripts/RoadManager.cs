using UnityEngine;

public class RoadManager : MonoBehaviour
{
    private const int RoadCount = 10;
    private readonly Vector3 PieceLength = new Vector3(0, 0, 6);
    private readonly Vector3 PieceForwardOffset = new Vector3(0, 0, 2);

    [SerializeField] private GameObject roadPiecePrefab;
    [SerializeField] private Transform roadContainer;
    [SerializeField] private GameObject[] breakableObjects;

    private void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        for (int i = 0; i < RoadCount; i++)
        {
            GameObject instance = Instantiate(roadPiecePrefab, roadContainer);
            instance.transform.position += PieceLength * i;
        }

        for (int i = 0; i < RoadCount * 3 - 1; i++)
        {
            int index = Random.Range(0, breakableObjects.Length);
            GameObject instance = Instantiate(breakableObjects[index], roadContainer);
            instance.transform.position += PieceForwardOffset * i;
        }
    }
}
