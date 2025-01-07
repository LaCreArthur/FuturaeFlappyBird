using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject pipePrefab;
    [SerializeField] GameObject pipeEndPrefab;
    [SerializeField] GameObject pipePointTrigger;
    [SerializeField] GameObject pipeSetPrefab; // Empty prefab with a transform for grouping pipes

    [Header("Level Parameters")]
    [SerializeField] Vector2 pipeHeightGapRange, pipeMinMaxHeightRange, pipeXSpaceRange;
    [SerializeField] float pipeMinHeight = 1f;
    [SerializeField] float spawnDistance = 10f; // Distance between last pipeSet and spawn trigger
    [SerializeField] float despawnDistance = -10f;

    [Header("Level Movement")]
    [SerializeField] float levelSpeed = 2.0f;

    Transform _lastPipeSet;
    float LastPipeX => _lastPipeSet?.position.x ?? 0;

    void Start()
    {
        OnRestart();
        GameManager.OnGameRestart += OnRestart;
    }

    void Update()
    {
        MovePipeSets();
        HandlePipeSpawning();
        DespawnOldPipes();
    }

    void OnDestroy() => GameManager.OnGameRestart -= OnRestart;

    void OnRestart()
    {
        _lastPipeSet = null;
        DespawnAllPipes();
        GenerateInitialLevel();
    }

    void GenerateInitialLevel()
    {
        for (int i = 0; i < 3; i++)
        {
            GeneratePipeSet();
        }
    }

    void GeneratePipeSet()
    {
        float pipeX = LastPipeX + Random.Range(pipeXSpaceRange.x, pipeXSpaceRange.y);

        // Choose a random height for the gap between pipes
        float gapHeight = Random.Range(pipeHeightGapRange.x, pipeHeightGapRange.y);
        float halfGapHeight = gapHeight / 2f;

        // Choose a random center for the gap
        float gapCenter = Random.Range(pipeMinMaxHeightRange.x + halfGapHeight, pipeMinMaxHeightRange.y - halfGapHeight);

        // Calculate the top and bottom pipe positions
        float bottomPipeY = gapCenter - halfGapHeight;
        float topPipeY = gapCenter + halfGapHeight;

        // Calculate heights of pipes
        float bottomPipeHeight = Mathf.Abs(bottomPipeY - pipeMinMaxHeightRange.x) + pipeMinHeight;
        float topPipeHeight = Mathf.Abs(pipeMinMaxHeightRange.y - topPipeY) + pipeMinHeight;

        // Create a parent object for the pipe set
        GameObject pipeSet = Instantiate(pipeSetPrefab, new Vector3(pipeX, 0, 0), Quaternion.identity, transform);

        // Bottom Pipe
        SpawnPipe(pipeSet.transform, new Vector3(pipeX, bottomPipeY, 0), bottomPipeHeight);

        // Top Pipe
        SpawnPipe(pipeSet.transform, new Vector3(pipeX, topPipeY, 0), -topPipeHeight);

        // Pipe Trigger
        Instantiate(pipePointTrigger, new Vector3(pipeX, gapCenter, 0), Quaternion.identity, pipeSet.transform);

        _lastPipeSet = pipeSet.transform;
    }

    void SpawnPipe(Transform parent, Vector3 endPipePos, float height)
    {
        GameObject pipeEnd = Instantiate(pipeEndPrefab, endPipePos, Quaternion.identity, parent);
        // we need to flip the pipe end if the height is negative, for the sprite to be displayed correctly
        pipeEnd.transform.localScale = new Vector3(1, Mathf.Sign(height), 1);

        // The pipe pivot is at the center of the pipe, so we need to adjust the position by half the height
        Vector3 centeredPos = endPipePos - new Vector3(0, height / 2f, 0);
        GameObject pipe = Instantiate(pipePrefab, centeredPos, Quaternion.identity, parent);
        // We need to scale the pipe on Y to match the height
        pipe.transform.localScale = new Vector3(1, Mathf.Abs(height), 1); // Ensure height is positive
    }

    void MovePipeSets()
    {
        foreach (Transform pipeSet in transform)
        {
            pipeSet.position += Vector3.left * (levelSpeed * Time.deltaTime);
        }
    }

    void HandlePipeSpawning()
    {
        if (LastPipeX < spawnDistance)
        {
            GeneratePipeSet();
        }
    }

    void DespawnOldPipes()
    {
        foreach (Transform pipeSet in transform)
        {
            if (pipeSet.position.x < despawnDistance)
            {
                Destroy(pipeSet.gameObject);
            }
        }
    }

    void DespawnAllPipes()
    {
        foreach (Transform pipeSet in transform)
        {
            Destroy(pipeSet.gameObject);
        }
    }
}