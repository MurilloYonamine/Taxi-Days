using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

namespace History
{
    [RequireComponent(typeof(HistoryNavigation))]
    public class HistoryManager : MonoBehaviour
    {
        public const int HISTORY_CACHE_LIMIT = 100;
        public static HistoryManager instance { get; private set; }
        public List<HistoryState> history = new List<HistoryState>();
        private HistoryNavigation historyNavigation;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            historyNavigation = GetComponent<HistoryNavigation>();
        }
        private void Start()
        {
            DialogueSystem.instance.onClear += LogCurrentState;
        }
        public void LogCurrentState()
        {
            HistoryState state = HistoryState.Capture();
            history.Add(state);

            if (history.Count > HISTORY_CACHE_LIMIT)
            {
                history.RemoveAt(0);
            }
        }
        public void LoadState(HistoryState state)
        {
            state.Load();
            
        }
        public void GoForward() => historyNavigation.GoForward();
        public void GoBack() => historyNavigation.GoBack();
    }
}