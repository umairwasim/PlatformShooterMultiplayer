using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject startGameButton;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] RoomListItem roomListItemPrefab;
    [SerializeField] PlayerListItem PlayerListItemPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }
    public void CreateRoom()
    {
        //Prevent null or empty room name field for Room Creation
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return;

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.SwtichMenu(MenuType.Loading);
    }


    #region Function Overrides

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.SwtichMenu(MenuType.Title);
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.SwtichMenu(MenuType.Room);
        roomNameText.text = "Room : " + PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        //Delete previous content in children so that old ones are now shown 
        foreach (Transform child in playerListContent)
            Destroy(child.gameObject);

        //set up players
        for (int i = 0; i < players.Count(); i++)
            Instantiate(PlayerListItemPrefab, playerListContent).SetUp(players[i]);

        //Only Master Client can start the Game
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.SwtichMenu(MenuType.Error);
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.SwtichMenu(MenuType.Title);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
            Destroy(trans.gameObject);

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            Instantiate(roomListItemPrefab, roomListContent).SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).SetUp(newPlayer);
    }

    #endregion

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #region Join/Leave Room
    
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.SwtichMenu(MenuType.Loading);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.SwtichMenu(MenuType.Loading);
    }
    
    #endregion
}