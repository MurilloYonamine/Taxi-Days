using System.Collections;
using System.Collections.Generic;
using CHARACTERS;
using COMMANDS;
using DIALOGUE.LogicalLines;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        public TextArchitect textArchitect = null;
        private bool userPrompt = false;

        private TagManager tagManager;
        private LogicalLineManager logicalLineManager;

        public Conversation conversation => conversationQueue.IsEmpty() ? null : conversationQueue.topConversation;
        public int conversationProgress => conversationQueue.IsEmpty() ? -1 : conversationQueue.topConversation.GetProgress();
        private ConversationQueue conversationQueue;
        public ConversationManager(TextArchitect textArchitect)
        {
            this.textArchitect = textArchitect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;

            tagManager = new TagManager();
            logicalLineManager = new LogicalLineManager();
            conversationQueue = new ConversationQueue();
        }
        public void Enqueue(Conversation conversation) => conversationQueue.Enqueue(conversation);
        public void EnqueuePriority(Conversation conversation) => conversationQueue.EnqueuePriority(conversation);

        // Event for when the user prompts the next line
        private void OnUserPrompt_Next() => userPrompt = true;

        public Coroutine StartConversation(Conversation conversation)
        {
            StopConversation();
            Enqueue(conversation);
            process = dialogueSystem.StartCoroutine(RunningConversation());

            return process;
        }
        public void StopConversation()
        {
            if (!isRunning) return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        // Run the conversation
        IEnumerator RunningConversation()
        {
            while (!conversationQueue.IsEmpty())
            {
                Conversation currentConversation = conversation;

                if (currentConversation.HasReachedEnd())
                {
                    conversationQueue.Dequeue();
                    continue;
                }
                string rawLine = currentConversation.CurrentLine();

                // Don't show any blank lines or try to run any logic of them
                if (string.IsNullOrWhiteSpace(rawLine))
                {
                    TryAdvanceConversation(currentConversation);
                    continue;
                }

                DIALOGUE_LINE line = DialogueParser.Parse(rawLine);
                if (logicalLineManager.TryGetLogic(line, out Coroutine logic))
                {
                    yield return logic;
                }
                else
                {
                    // Show dialogue
                    if (line.hasDialogue) yield return Line_RunDialogue(line);

                    // Run any commands
                    if (line.hasCommands) yield return Line_RunCommands(line);

                    if (line.hasDialogue)
                    {
                        // Wait for user input
                        yield return WaitForUserInput();

                        CommandManager.instance.StopAllProcesses();
                    }
                    TryAdvanceConversation(currentConversation);
                }
            }
            process = null;
        }
        private void TryAdvanceConversation(Conversation conversation)
        {
            conversation.IncrementProgress();

            if (conversation != conversationQueue.topConversation) return;

            if (conversation.HasReachedEnd()) conversationQueue.Dequeue();
        }
        // Run the dialogue
        private IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            // show or hide the speaker name if there is one present.
            if (line.hasSpeaker) HandleSpeakerLogic(line.speakerData);

            if (!dialogueSystem.dialogueContainer.isVisible) dialogueSystem.dialogueContainer.Show();

            // build dialogue
            yield return BuildLineSegments(line.dialogueData);
        }
        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);
            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing)) character.Show();

            dialogueSystem.ShowSpeakerName(tagManager.Inject(speakerData.displayName));
            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            if (speakerData.isCastingPosition) character.MoveToPosition(speakerData.castPosition);

            // Cast Expression
            if (speakerData.isCastingExpressions)
            {
                foreach (var ce in speakerData.CastExpressions) character.OnReceiveExpression(ce.layer, ce.expression);
            }

        }
        // Run any commands
        private IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

            foreach (DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion || command.name == "wait")
                {
                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while (!cw.IsDone)
                    {
                        if (userPrompt)
                        {
                            CommandManager.instance.StopCurrentProcess();
                            userPrompt = false;
                        }
                        yield return null;
                    }
                }
                else
                    CommandManager.instance.Execute(command.name, command.arguments);
            }
            yield return null;
        }
        // Build the line segments
        private IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];
                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }
        public bool isWaitingOnAutoTimer { get; private set; } = false;
        private IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            // Wait for the signal to be triggered
            switch (segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    isWaitingOnAutoTimer = true;
                    yield return new WaitForSeconds(segment.signalDelay);
                    isWaitingOnAutoTimer = false;
                    break;
                default:
                    break;
            }
        }
        private IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            dialogue = tagManager.Inject(dialogue);

            // build the dialogue
            if (!append) textArchitect.Build(dialogue);
            else textArchitect.Append(dialogue);

            // wait for the dialogue to complete
            while (textArchitect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!textArchitect.hurryUp)
                    {
                        textArchitect.hurryUp = true;
                    }
                    else
                    {
                        textArchitect.ForceComplete();
                    }
                    userPrompt = false;
                }
                yield return null;
            }
        }
        private IEnumerator WaitForUserInput()
        {
            dialogueSystem.prompt.Show();

            while (!userPrompt) yield return null;

            dialogueSystem.prompt.Hide();

            userPrompt = false;
        }
    }
}