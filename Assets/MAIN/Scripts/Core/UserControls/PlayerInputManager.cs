using System;
using System.Collections;
using System.Collections.Generic;
using History;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DIALOGUE
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInput input;
        private List<(InputAction action, Action<InputAction.CallbackContext> command)> actions = new List<(InputAction, Action<InputAction.CallbackContext>)>();
        private void Awake()
        {
            input = GetComponent<PlayerInput>();
            InitializeActions();
        }
        private void InitializeActions()
        {
            actions.Add((input.actions["Next"], OnNext));
            actions.Add((input.actions["HistoryBack"], OnHistoryBack));
            actions.Add((input.actions["HistoryForward"], OnHistoryForward));
        }
        private void OnEnable()
        {
            foreach (var InputAction in actions) InputAction.action.performed += InputAction.command;
        }
        private void OnDisable()
        {
            foreach (var InputAction in actions) InputAction.action.performed -= InputAction.command;
        }
        public void OnNext(InputAction.CallbackContext context)
        {
            DialogueSystem.instance.OnUserPrompt_Next();
        }
        public void OnHistoryBack(InputAction.CallbackContext context)
        {
            HistoryManager.instance.GoBack();
        }
        public void OnHistoryForward(InputAction.CallbackContext context)
        {
            HistoryManager.instance.GoForward();
        }
    }
}