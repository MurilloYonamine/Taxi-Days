using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace History
{
    /// <summary>
    /// A visual log of a previous point in dialogue.
    /// </summary>
    public class HistoryLog : MonoBehaviour
    {
        public GameObject container;
        
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;

        public float nameFontSize = 0;
        public float dialogueFontSize = 0;
    }
}