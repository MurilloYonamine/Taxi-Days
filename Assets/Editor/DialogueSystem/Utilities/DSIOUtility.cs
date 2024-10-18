using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TaxiDays.Utility
{
    public static class DSIOUtility
    {
        #region Save Methods
        public static void Save()
        {
            CreateStaticFolders();
        }
        #endregion

        #region Creation Methods
        private static void CreateStaticFolders()
        {
            /*if (AssetDatabase.IsValidFolder("Assets/Editor/DialogueSystem/Graphs"))
            {

            }*/
        }
        #endregion
    }
}
