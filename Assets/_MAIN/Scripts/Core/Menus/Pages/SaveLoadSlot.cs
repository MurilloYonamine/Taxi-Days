using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VISUALNOVEL;
using System.IO;
using History;

public class SaveLoadSlot : MonoBehaviour
{
    public GameObject root;
    public RawImage previewImage;
    public TextMeshProUGUI titleText;

    [HideInInspector] public int fileNumber = 0;
    [HideInInspector] public string filePath = "";

    private UIConfirmationMenu uiChoiceMenu => UIConfirmationMenu.instance;

    public void PopulateDetails(SaveAndLoadMenu.MenuFunction function)
    {
        if (File.Exists(filePath))
        {
            VNGameSave file = VNGameSave.Load(filePath);
            PopulateDetailsFromFile(function, file);
        }
        else
        {
            PopulateDetailsFromFile(function, null);
        }
    }

    private void PopulateDetailsFromFile(SaveAndLoadMenu.MenuFunction function, VNGameSave file)
    {
        if (file == null)
        {
            titleText.text = $"{fileNumber}. Slot Vazio";
            previewImage.texture = SaveAndLoadMenu.Instance.emptyFileImage;
        }
        else
        {
            titleText.text = $"{fileNumber}. {file.timestamp}";

            byte[] data = File.ReadAllBytes(file.screenshotPath);
            Texture2D screenshotPreview = new Texture2D(1, 1);
            ImageConversion.LoadImage(screenshotPreview, data);
            previewImage.texture = screenshotPreview;
        }
    }

    public void OnSlotClick()
    {
        // Verifica se o menu está no modo de salvar
        if (SaveAndLoadMenu.Instance.menuFunction == SaveAndLoadMenu.MenuFunction.save)
        {
            if (File.Exists(filePath))
            {
                // Se houver um arquivo salvo, exibe opções de "Carregar" e "Excluir"
                uiChoiceMenu.Show(
                    "O que deseja fazer?",
                    new UIConfirmationMenu.ConfirmationButton("Carregar", Load),
                    new UIConfirmationMenu.ConfirmationButton("Excluir", OnConfirmDelete),
                    new UIConfirmationMenu.ConfirmationButton("Cancelar", null)
                );
            }
            else
            {
                // Se não houver arquivo salvo, exibe a opção de "Salvar"
                uiChoiceMenu.Show(
                    "O que deseja fazer?",
                    new UIConfirmationMenu.ConfirmationButton("Salvar", Save),
                    new UIConfirmationMenu.ConfirmationButton("Cancelar", null)
                );
            }
        }
        else if (SaveAndLoadMenu.Instance.menuFunction == SaveAndLoadMenu.MenuFunction.load)
        {
            // No modo "Load", só exibe as opções se houver um arquivo salvo
            if (File.Exists(filePath))
            {
                uiChoiceMenu.Show(
                    "O que deseja fazer?",
                    new UIConfirmationMenu.ConfirmationButton("Carregar", Load),
                    new UIConfirmationMenu.ConfirmationButton("Excluir", OnConfirmDelete),
                    new UIConfirmationMenu.ConfirmationButton("Cancelar", null)
                );
            }
            else
            {
                return;
            }
        }
    }

    /*public void Delete()
    {
        uiChoiceMenu.Show(
            "Excluir este arquivo? (<i>Isso não pode ser desfeito!</i>)",
            new UIConfirmationMenu.ConfirmationButton("Sim", () =>
                {
                    uiChoiceMenu.Show(
                        "Você tem certeza?",
                        new UIConfirmationMenu.ConfirmationButton("Tenho certeza", OnConfirmDelete),
                        new UIConfirmationMenu.ConfirmationButton("Deixa pra lá", null)
                    );
                },
                autoCloseOnClick: false
            ),
            new UIConfirmationMenu.ConfirmationButton("Não", null)
        );
    }*/

    private void OnConfirmDelete()
    {
        File.Delete(filePath);
        PopulateDetails(SaveAndLoadMenu.Instance.menuFunction);
    }

    public void Load()
    {
        VNGameSave file = VNGameSave.Load(filePath, false);
        SaveAndLoadMenu.Instance.Close(closeAllMenus: true);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == MainMenu.MAIN_MENU_SCENE)
        {
            MainMenu.instance.LoadGame(file);
        }
        else
        {
            file.Activate();
        }
    }

    public void Save()
    {
        if (HistoryManager.instance.isViewingHistory)
        {
            UIConfirmationMenu.instance.Show("Você não pode salvar enquanto visualiza o histórico.", new UIConfirmationMenu.ConfirmationButton("Ok", null));
            return;
        }

        var activeSave = VNGameSave.activeFile;
        activeSave.slotNumber = fileNumber;

        activeSave.Save();

        PopulateDetailsFromFile(SaveAndLoadMenu.Instance.menuFunction, activeSave);
    }
}