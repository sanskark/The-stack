using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ThemeShopUI : MonoBehaviour
{
    [Header("Layout settings")]
    public float itemSpacing;
    float itemHeight;

    [Header("UI Elements")]
    public Transform shopMenu;
    public Transform shopItemsContainer;
    public GameObject itemPrefab;

    [Space(20f)]
    public ThemeShopDatabase themeDB;

    [Header("Shop Events")]
    public GameObject shopMenuUI;
    public Button openShopButton;
    public Button closeShopButton;


    int newSelectedTheme = 0;
    int previousSelectedTheme = 0;
    private void Start()
    {
        AddShopEvents();
        GenerateShopItemsUI();
        SetSelectedTheme();
        SelectItemUI(GameDataManager.GetSelectedThemeIndex());
        CloseShop();
    }

    void SetSelectedTheme()
    {
        int index = GameDataManager.GetSelectedThemeIndex();
        GameDataManager.SetSelectedThemeIndex(index);
        GameDataManager.SetSelectedTheme(themeDB.GetTheme(index),index);
    }
    void GenerateShopItemsUI()
    {
        for(int i=0; i < GameDataManager.GetAllPurchasedThemes().Count; i++)
        {
            int purchasedThemeIndex = GameDataManager.GetPurchasedTheme(i);
            themeDB.PurchaseTheme(purchasedThemeIndex);
        }


        itemHeight = shopItemsContainer.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
        Destroy(shopItemsContainer.GetChild(0).gameObject);

        shopItemsContainer.DetachChildren();

        for(int i=0; i<themeDB.ThemesCount; i++)
        {
            Theme theme = themeDB.GetTheme(i);
            ShopItemUI uiItem = Instantiate(itemPrefab, shopItemsContainer).GetComponent<ShopItemUI>();

            uiItem.SetItemPosition(Vector2.down * i * (itemHeight + itemSpacing));
            uiItem.gameObject.name = "Theme " + i + "-" + theme.name;

            uiItem.SetThemeName(theme.name);
            uiItem.SetThemeImage(theme.image);
            uiItem.SetPriceText(theme.price);

            if (theme.isPurchased)
            {
                uiItem.SetThemeAsPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                uiItem.SetPriceText(theme.price);
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }

            shopItemsContainer.GetComponent<RectTransform>().sizeDelta = Vector2.up * (itemHeight + itemSpacing) * themeDB.ThemesCount;
        }
     }

    void OnItemSelected(int index)
    {
        SelectItemUI(index);
    }
    void SelectItemUI(int itemIndex)
    {
        previousSelectedTheme = newSelectedTheme;
        newSelectedTheme = itemIndex;

        ShopItemUI prevUiItem = GetItemUI(previousSelectedTheme);
        ShopItemUI newUiItem = GetItemUI(newSelectedTheme);

        prevUiItem.DeselectItem();
        newUiItem.SelectItem();
        GameDataManager.SetSelectedThemeIndex(newSelectedTheme);
        SetSelectedTheme();
    }

    ShopItemUI GetItemUI(int index)
    {
        return shopItemsContainer.GetChild(index).GetComponent<ShopItemUI>();
    }

    void OnItemPurchased(int index)
    {
        Theme theme = themeDB.GetTheme(index);
        ShopItemUI uiItem = GetItemUI(index);

        if (GameDataManager.CanPurchaseTheme(theme.price))
        {
            themeDB.PurchaseTheme(index);
            uiItem.SetThemeAsPurchased();
            uiItem.OnItemSelect(index, OnItemSelected);

            GameDataManager.AddPurchasedTheme(index);
        }
        else
        {
            Debug.Log("Cant enough high score");
        }
    }
    private void AddShopEvents()
    {
        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(OpenShop);

        closeShopButton.onClick.RemoveAllListeners();
        closeShopButton.onClick.AddListener(CloseShop);
    }

    private void OpenShop()
    {
        shopMenuUI.SetActive(true);
    }
    private void CloseShop()
    {
        shopMenuUI.SetActive(false);
    }
}
