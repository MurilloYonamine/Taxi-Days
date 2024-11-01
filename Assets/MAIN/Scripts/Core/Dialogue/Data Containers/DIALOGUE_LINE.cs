
namespace DIALOGUE
{
    public class DIALOGUE_LINE
    {
        /*
        Storage container for dialogue information
        that has been parsed and ripped from a string
        */

        public string rawData { get; private set; } = string.Empty;
        
        public DL_SPEAKER_DATA speakerData;
        public DL_DIALOGUE_DATA dialogueData;
        public DL_COMMAND_DATA commandData;

        public bool hasSpeaker => speakerData != null;
        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandData != null;

        public DIALOGUE_LINE(string rawLine, string speaker, string dialogue, string commands)
        {
            rawData = rawLine;
            this.speakerData = string.IsNullOrWhiteSpace(speaker) ? null : new DL_SPEAKER_DATA(speaker);
            this.dialogueData = string.IsNullOrEmpty(dialogue) ? null : new DL_DIALOGUE_DATA(dialogue);
            this.commandData = string.IsNullOrWhiteSpace(commands) ? null : new DL_COMMAND_DATA(commands);
        }
    }
}
