using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnpoints;

    void Awake()
    {
        Instance = this;

        if (spawnpoints.Length == 0)
            spawnpoints = GetComponentsInChildren<Transform>();
    }

    public Transform GetRandomSpawnpoint() => spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
}
