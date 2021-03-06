using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{

    public Color itemNotSelectedColor;
    public Color itemSelectedColor;

    [Space(20f)]
    public Image themeImage;
    public Text themeName;
    public Text themePriceText;
    public Button themePurchaseButton;

    [Space(20f)]
    public Button itemButton;
    public Image itemImage;
    public Outline itemOutline;

    public void SetItemPosition(Vector2 pos)
    {
        GetComponent<RectTransform>().anchoredPosition += pos;
    }
    public void SetThemeImage(Sprite sprite)
    {
        themeImage.sprite = sprite;
    }
    public void SetThemeName(string name)
    {
        themeName.text = name; 
    }
    public void SetPriceText(int price)
    {
        themePriceText.text = "BUY: "+ price.ToString();
    }
    public void SetThemeAsPurchased()
    {
        themePurchaseButton.gameObject.SetActive(false);
        itemButton.interactable = true;

        itemImage.color = itemNotSelectedColor;
    }
    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        themePurchaseButton.onClick.RemoveAllListeners();
        themePurchaseButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }
    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        itemButton.interactable = true;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }
    public void SelectItem()
    {
        itemOutline.enabled = true;
        itemImage.color = itemSelectedColor;
        itemButton.interactable = false;
    }
    public void DeselectItem()
    {
        itemOutline.enabled = false;
        itemImage.color = itemNotSelectedColor;
        itemButton.interactable = true;
    }
}
