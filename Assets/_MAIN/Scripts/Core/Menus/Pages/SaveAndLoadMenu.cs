using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VISUALNOVEL;

public class SaveAndLoadMenu : MenuPage
{
    public static SaveAndLoadMenu Instance { get; private set; }

    public const int MAX_FILES = 30;

    private int currentPage = 1;
    private bool loadedFilesForFirstTime = false;

    public enum MenuFunction { save, load }
    public MenuFunction menuFunction = MenuFunction.save;

    public SaveLoadSlot[] saveSlots;
    public int slotsPerPage => saveSlots.Length;

    public Texture emptyFileImage;

    public TextMeshProUGUI menuTitle;
    public Button Save;
    public Button Load;

    private void Awake()
    {
        Instance = this;
    }

    public override void Open()
    {
        base.Open();
        UpdateMenuTitleAndButtons();

        if (!loadedFilesForFirstTime)
            PopulateSaveSlotsForPage(currentPage);
    }
    public void UpdateMenuTitleAndButtons()
    {
        if (menuFunction == MenuFunction.save)
        {
            menuTitle.text = "Menu de Salvamento";
            //Save.gameObject.SetActive(true);
            //Load.gameObject.SetActive(false);
        }
        else if (menuFunction == MenuFunction.load)
        {
            menuTitle.text = "Menu de Carregamento";
            //Save.gameObject.SetActive(false);
            //Load.gameObject.SetActive(true);
        }

    }

    public void PopulateSaveSlotsForPage(int pageNumber)
    {
        currentPage = pageNumber;
        int startingFile = ((currentPage - 1) * slotsPerPage) + 1;
        int endingFile = startingFile + slotsPerPage - 1;

        for (int i = 0; i < slotsPerPage; i++)
        {
            int fileNum = startingFile + i;
            SaveLoadSlot slot = saveSlots[i];

            if (fileNum <= MAX_FILES)
            {
                slot.root.SetActive(true);
                string filePath = $"{FilePaths.gameSaves}{fileNum}{VNGameSave.FILE_TYPE}";
                slot.fileNumber = fileNum;
            }
        }
    }
}
