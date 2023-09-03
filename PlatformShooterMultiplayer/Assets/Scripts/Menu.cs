using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType { Loading, Title, CreateRoom, Room, Error, FindRoom }

public class Menu : MonoBehaviour
{
	public MenuType menuType;
    private bool active;

	public bool IsActive() => active = true;

    public void Open() => gameObject.SetActive(active = true);

    public void Close() => gameObject.SetActive(active = false);
}