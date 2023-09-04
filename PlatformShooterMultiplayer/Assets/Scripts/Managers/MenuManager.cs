using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private Menu[] menus;

    void Awake()
    {
        Instance = this;
    }

    public void SwtichMenu(MenuType menuType)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuType == menuType)
                menus[i].Open();
            else if (menus[i].IsActive())
                CloseMenu(menus[i]);
        }
    }

    public void OpenMenu(Menu menu)
    {
        //open selected menu, close rest
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].IsActive())
                CloseMenu(menus[i]);
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}