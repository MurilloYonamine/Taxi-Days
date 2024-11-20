using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using static VariableStore;

namespace VISUALNOVEL
{
    public class VNDatabaseGlobalVariables : MonoBehaviour
    {
        private void Awake()
        {
            string[] characters = { "Nicole", "Thiago", "Joseph", "Luana" };
            int days = 7;

            foreach (string character in characters)
            {
                if (!VariableStore.HasVariable($"{character}.count"))
                    VariableStore.CreateVariable($"{character}.count", (double)0);

                for (int day = 1; day <= days; day++)
                {
                    if (!VariableStore.HasVariable($"{character}.day{day}"))
                        VariableStore.CreateVariable($"{character}.day{day}", (bool)false);
                }
            }
        }
    }
}