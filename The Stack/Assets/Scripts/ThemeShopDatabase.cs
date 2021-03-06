using UnityEngine;

[CreateAssetMenu(fileName = "ThemeShopDatabase", menuName = "Shopping/Themes shop database")]
public class ThemeShopDatabase : ScriptableObject
{
    public Theme[] themes;

    public int ThemesCount
    {
        get
        {
            return themes.Length;
        }
    }

    public Theme GetTheme(int index)
    {
        return themes[index];
    }
    public void PurchaseTheme(int index)
    {
        themes[index].isPurchased = true;
    }
}
