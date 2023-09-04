using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MenuType { Loading, Title, CreateRoom, Room, Error, FindRoom }

public class Menu : MonoBehaviour
{
    public MenuType menuType;
  
    private bool active;
    private readonly float scaleMultipler = 0.5f;
    private readonly float scaleDuration = 0.25f;
    public bool IsActive() => active = true;

    public void Open()
    {
        gameObject.SetActive(IsActive());
        transform.DOPunchScale(Vector3.one * scaleMultipler, scaleDuration);
    }

    public void Close() => gameObject.SetActive(!IsActive());
}