using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameWorldSpace : MonoBehaviour
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private TMP_Text nickNameText;

    private void Start()
    {
        if (pv.IsMine)
            gameObject.SetActive(false);

        nickNameText.text = pv.Owner.NickName;
    }
}
