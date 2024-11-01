using System.Collections;

namespace DIALOGUE.LogicalLines
{
    interface ILogicalLine
    {
        string keyword => throw new System.NotImplementedException();
        IEnumerator Execute(DIALOGUE_LINE line);
        bool Matches(DIALOGUE_LINE line);
    }
}