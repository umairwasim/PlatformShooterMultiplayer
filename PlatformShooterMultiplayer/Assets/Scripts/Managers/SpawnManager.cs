using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private UnityEngine.Transform[] spawnpoints;

    void Awake()
    {
        Instance = this;

        if (spawnpoints.Length == 0)
            spawnpoints = GetComponentsInChildren<UnityEngine.Transform>();
    }

    public UnityEngine.Transform GetRandomSpawnpoint() => spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
}
