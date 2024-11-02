using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

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

        public static class Expressions
        {
            public static HashSet<string> OPERATORS = new HashSet<string>() { "=", "-=", "+", "+=", "*", "*=", "/", "/=", "=" };
            public static readonly string REGEX_ARITHMATIC = @"([-+*/=]=?)";
            public static readonly string REGEX_OPERATOR_LINE = @"^\$\w+\s*(=|\+=|-=|\*=|/=|)\s*";

            public static object CalculateValue(string[] expressionParts)
            {
                List<string> operandStrings = new List<string>();
                List<string> operatorStrings = new List<string>();
                List<object> operands = new List<object>();

                for (int i = 0; i < expressionParts.Length; i++)
                {
                    string part = expressionParts[i].Trim();

                    if (part == string.Empty) continue;

                    if (OPERATORS.Contains(part))
                    {
                        operatorStrings.Add(part);
                    }
                    else
                    {
                        operandStrings.Add(part);
                    }
                }
                foreach (string operandString in operandStrings)
                {
                    operands.Add(ExtractValue(operandString));
                }
                CalculateValue_DivisionAndMultiplication(operandStrings, operands);

                CalculateValue_AdditionAndSubstraction(operandStrings, operands);

                return operands[0];
            }
            private static void CalculateValue_DivisionAndMultiplication(List<string> operandStrings, List<object> operands)
            {
                for (int i = 0; i < operandStrings.Count; i++)
                {
                    if (operandStrings[i] == "*" || operandStrings[i] == "/")
                    {
                        double leftOperand = Convert.ToDouble(operands[i]);
                        double rightOperand = Convert.ToDouble(operands[i + 1]);

                        if (operandStrings[i] == "*") operands[i] = leftOperand * rightOperand;
                        else
                        {
                            if (rightOperand == 0)
                            {
                                if (rightOperand == 0)
                                {
                                    Debug.LogError("Não pode dizer por zero!");
                                    return;
                                }
                                operands[i] = leftOperand / rightOperand;
                            }
                        }
                        operands.RemoveAt(i + 1);
                        operandStrings.RemoveAt(i);
                        i--;
                    }
                }
            }
            private static void CalculateValue_AdditionAndSubstraction(List<string> operatorStrings, List<object> operands)
            {
                for (int i = 0; i < operatorStrings.Count; i++)
                {
                    string operatorString = operatorStrings[i];

                    if (operatorString == "+" || operatorString == "-")
                    {
                        double leftOperand = Convert.ToDouble(operands[i]);
                        double rightOperand = Convert.ToDouble(operands[i + 1]);

                        if (operatorString == "+") operands[i] = leftOperand + rightOperand;
                        else operands[i] = leftOperand - rightOperand;

                        operands.RemoveAt(i + 1);
                        operatorStrings.RemoveAt(i);
                        i--;
                    }
                }
            }
            public static object ExtractValue(string value)
            {
                bool negate = false;

                if (value.StartsWith('!'))
                {
                    negate = true;
                    value = value.Substring(1);
                }
                if (value.StartsWith(VariableStore.VARIABLE_ID))
                {
                    string variableName = value.TrimStart(VariableStore.VARIABLE_ID);
                    
                    if (!VariableStore.HasVariable(variableName))
                    {
                        Debug.LogError($"Variável {variableName} não existe!");
                        return null;
                    }
                    VariableStore.TryGetValue(variableName, out object val);

                    if (val is bool boolVal && negate) return !boolVal;

                    return val;
                }
                else if (value.StartsWith('\"') && value.EndsWith('\"'))
                {
                    value = TagManager.Inject(value, injectTags: true, injectVariables: true);
                    return value.Trim('"');
                }
                else
                {
                    if (int.TryParse(value, out int intValue))
                    {
                        return intValue;
                    }
                    else if (float.TryParse(value, out float floatValue))
                    {
                        return floatValue;
                    }
                    else if (bool.TryParse(value, out bool boolValue))
                    {
                        return negate ? !boolValue : boolValue;
                    }
                    else
                    {
                        return value;
                    }
                }
            }
        }
    }
}
