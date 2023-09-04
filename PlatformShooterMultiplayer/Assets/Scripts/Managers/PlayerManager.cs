using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// Manage Player Data
///	Receive Information from other players. 
///	Manage kills and deaths
/// </summary>
public class PlayerManager : MonoBehaviour
{
    private const string PREFABS = "Prefabs";
    private const string PLAYER_CONTROLLER = "PlayerController";
    private const string DEATHS = "deaths";
    private const string KILLS = "kills";

    private PhotonView pv;
    private GameObject playerController;

    private int kills;
    private int deaths;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        //IF photonView is local player, instantiate player controller prefab
        if (pv.IsMine)
            CreatePlayerController();
    }

    void CreatePlayerController()
    {
        Transform randomSpawnpoint = SpawnManager.Instance.GetRandomSpawnpoint();

        //Create Photon Instantiation menthod and pass in viewID so that it can be used in other scripts
        playerController = PhotonNetwork.Instantiate(Path.Combine(PREFABS, PLAYER_CONTROLLER),
            randomSpawnpoint.position,
            randomSpawnpoint.rotation,
            0,
            new object[] { pv.ViewID });
    }

    public void Die()
    {
        //On Death, destroy Player Controller from Photon Newtwork
        PhotonNetwork.Destroy(playerController);
        CreatePlayerController();

        deaths++;

        //Add to Deaths hash table and set local player property
        Hashtable hash = new();
        hash.Add(DEATHS, deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        //Add to Kills hash table and set local player property
        Hashtable hash = new();
        hash.Add(KILLS, kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player) => FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.pv.Owner == player);
}