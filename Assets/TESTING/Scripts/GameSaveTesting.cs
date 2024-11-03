using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

namespace TESTING
{
    public class GameSaveTesting : MonoBehaviour
    {
        void Start()
        {
            VNGameSave.activeFile = new VNGameSave();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                VNGameSave.activeFile.Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                VNGameSave.activeFile = FileManager.Load<VNGameSave>($"{FilePaths.gameSaves}1{VNGameSave.FILE_TYPE}");
                VNGameSave.activeFile.Load();
            }
        }
    }
}