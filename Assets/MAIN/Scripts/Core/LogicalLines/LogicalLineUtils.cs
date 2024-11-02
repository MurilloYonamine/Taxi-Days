using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DIALOGUE.LogicalLines
{
    public static class LogicalLineUtils
    {
        /*
        Provides shared variable and method
        acces to logical line classes.
        */
        public static class Encapsulation
        {
            public struct EncapsulatedData
            {
                public List<string> lines;
                public int startingIndex;
                public int endingIndex;
            }
            private const char ENCAPSULATION_START = '{';
            private const char ENCAPSULATION_END = '}';

            public static EncapsulatedData RipEncapsulationData(Conversation conversation, int startingIndex, bool ripHeaderAndEncapsualators = false)
            {
                int encapsulationDepth = 0;
                EncapsulatedData data = new EncapsulatedData { lines = new List<string>(), startingIndex = startingIndex, endingIndex = 0 };

                for (int i = startingIndex; i < conversation.Count; i++)
                {
                    string line = conversation.GetLines()[i];
                    if (ripHeaderAndEncapsualators || (encapsulationDepth > 0 && !isEncapsulationEnd(line))) data.lines.Add(line);

                    if (isEncapsulationStart(line))
                    {
                        encapsulationDepth++;
                        continue;
                    }
                    if (isEncapsulationEnd(line))
                    {
                        encapsulationDepth--;
                        if (encapsulationDepth == 0)
                        {
                            data.endingIndex = i;
                            break;
                        }
                    }
                }
                return data;
            }

            public static bool isEncapsulationStart(string line) => line.Trim().StartsWith(ENCAPSULATION_START);
            public static bool isEncapsulationEnd(string line) => line.Trim().StartsWith(ENCAPSULATION_END);
        }

    }
}