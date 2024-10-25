using TMPro;
using UnityEngine;

[System.Serializable]
public class NameContainer
{
    /*
    Logic controller for a Dialogue Container's speaker name field
    Control visibility and other logic indepently
    */
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI nameText;
    public void Show(string nameToShow = "")
    {
        root.SetActive(true);

        if (nameToShow != string.Empty) nameText.text = nameToShow;
    }
    public void Hide() => root.SetActive(false);
    public void SetNameColor(Color color) => nameText.color = color;
    public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
}
