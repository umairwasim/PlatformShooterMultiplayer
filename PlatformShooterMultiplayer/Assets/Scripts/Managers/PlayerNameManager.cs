using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    private const string USERNNAME = "username";
    [SerializeField] TMP_InputField usernameInput;

	void Start()
	{
		if(PlayerPrefs.HasKey(USERNNAME))
		{
			usernameInput.text = PlayerPrefs.GetString(USERNNAME);
			PhotonNetwork.NickName = PlayerPrefs.GetString(USERNNAME);
		}
		else
		{
			usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
			OnUsernameInputValueChanged();
		}
	}

	public void OnUsernameInputValueChanged()
	{
		PhotonNetwork.NickName = usernameInput.text;
		PlayerPrefs.SetString(USERNNAME, usernameInput.text);
	}
}
