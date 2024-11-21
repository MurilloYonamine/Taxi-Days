using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadPageNavigationBar : MonoBehaviour
{
    [SerializeField] private SaveAndLoadMenu menu;

    private bool initialized = false;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;

    private const int MAX_BUTTONS = 5;

    public int selectedPage { get; private set; } = 1;
    private int maxPages = 0;
    private List<CanvasGroup> buttonCanvasGroups = new List<CanvasGroup>();

    // Start is called before the first frame update
    void Start()
    {
        InitializeMenu();
    }

    private void InitializeMenu()
    {
        if (initialized)
            return;

        initialized = true;

        maxPages = Mathf.CeilToInt((float)SaveAndLoadMenu.MAX_FILES / menu.slotsPerPage);

        int pageButtonLimit = MAX_BUTTONS < maxPages ? MAX_BUTTONS : maxPages;

        for (int i = 1; i <= pageButtonLimit; i++)
        {
            GameObject ob = Instantiate(buttonPrefab.gameObject, buttonPrefab.transform.parent);
            ob.SetActive(true);

            Button button = ob.GetComponent<Button>();
            CanvasGroup canvasGroup = ob.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = ob.AddComponent<CanvasGroup>();
            }
            buttonCanvasGroups.Add(canvasGroup);

            ob.name = i.ToString();
            TextMeshProUGUI txt = button.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = i.ToString();
            int closureIndex = i;
            button.onClick.AddListener(() => SelectSaveFilePage(closureIndex));
        }

        previousButton.SetActive(pageButtonLimit <= maxPages);
        nextButton.SetActive(pageButtonLimit <= maxPages);

        nextButton.transform.SetAsLastSibling();

        UpdateButtonOpacities();
    }

    private void SelectSaveFilePage(int pageNumber)
    {
        selectedPage = pageNumber;
        menu.PopulateSaveSlotsForPage(pageNumber);
        UpdateButtonOpacities();
    }

    public void ToNextPage()
    {
        if (selectedPage < maxPages)
            SelectSaveFilePage(selectedPage + 1);
    }

    public void ToPreviousPage()
    {
        if (selectedPage > 1)
            SelectSaveFilePage(selectedPage - 1);
    }

    private void UpdateButtonOpacities()
    {
        for (int i = 0; i < buttonCanvasGroups.Count; i++)
        {
            buttonCanvasGroups[i].alpha = (i + 1 == selectedPage) ? 1f : 0.5f;
        }
    }
}