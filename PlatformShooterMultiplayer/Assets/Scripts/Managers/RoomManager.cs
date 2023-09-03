using Photon.Pun;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private readonly int GameSceneIndex = 1;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    #region Photon OnEnable/OnDisable

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // if the loaded scene is the Game scene
        if (scene.buildIndex == GameSceneIndex)
            //Instantiate Player Manager from resrouces folder
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerManager")
                , Vector3.zero, Quaternion.identity);
    }
}