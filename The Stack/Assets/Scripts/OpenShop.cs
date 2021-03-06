using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenShop : MonoBehaviour
{
    public GameObject shopMenu;

    public Text highSore;

    bool isOpened=false;

    private void Start()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            highSore.text = PlayerPrefs.GetInt("highScore").ToString();
        }
    }
    public void OpenShopMenu()
    {
        if(!isOpened)
            shopMenu.SetActive(true);
    }
}
