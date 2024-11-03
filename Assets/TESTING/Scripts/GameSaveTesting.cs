using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VISUALNOVEL;

namespace TESTING
{
    public class GameSaveTesting : MonoBehaviour
    {
        public VNGameSave save;
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
                try
                {
                    save = VNGameSave.Load($"{FilePaths.gameSaves}1{VNGameSave.FILE_TYPE}", activateOnLoad: true);
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
    }
}