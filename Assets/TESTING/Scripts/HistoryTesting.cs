using System.Collections.Generic;
using History;
using UnityEngine;

namespace TESTING
{
    /// <summary>
    /// A simple script to test the history system.
    /// </summary>
    public class HistoryTesting : MonoBehaviour
    {
        public HistoryState state = new HistoryState();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                state = HistoryState.Capture();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                state.Load();
            }
        }
    }
}