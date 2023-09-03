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

    private PhotonView photonView;
    private GameObject controller;

    private int kills;
    private int deaths;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        //IF photonView is local player
        if (photonView.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine(PREFABS, PLAYER_CONTROLLER), 
            spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

        deaths++;

        Hashtable hash = new();
        hash.Add(DEATHS, deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
    {
        photonView.RPC(nameof(RPC_GetKill), photonView.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new();
        hash.Add(KILLS, kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player) => FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.photonView.Owner == player);
}