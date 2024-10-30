using System.Collections.Generic;
using System.Globalization;

namespace COMMANDS
{
    public class CommandParameters
    {
        /*
        Classe usada como um helper para a database extension
        para permitir uma fácil localização de parametros sem
        ter que passar informação por comandos.
        */
        private const char PARAMETER_IDENTIFIER = '-';
        private Dictionary<string, string> parameters = new Dictionary<string, string>();
        private List<string> unlabledParameters = new List<string>();

        public CommandParameters(string[] parameterArray, int startingIndex = 0)
        {
            for (int i = startingIndex; i < parameterArray.Length; i++)
            {
                if (parameterArray[i].StartsWith(PARAMETER_IDENTIFIER) && !float.TryParse(parameterArray[i], out _))
                {
                    string parameterName = parameterArray[i];
                    string parameterValue = "";

                    if (i + 1 < parameterArray.Length && !parameterArray[i + 1].StartsWith(PARAMETER_IDENTIFIER))
                    {
                        parameterValue = parameterArray[i + 1];
                        i++;
                    }

                    parameters.Add(parameterName, parameterValue);
                }
                else
                {
                    unlabledParameters.Add(parameterArray[i]);
                }
            }
        }
        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default(T)) => TryGetValue(new string[] { parameterName }, out value, defaultValue);

        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default(T))
        {
            foreach (string parameterName in parameterNames)
            {
                if (parameters.TryGetValue(parameterName, out string parameterValue))
                {
                    if (TryCastParameter(parameterValue, out value)) return true;
                }
            }
            foreach (string parameterName in unlabledParameters)
            {
                if (TryCastParameter(parameterName, out value))
                {
                    unlabledParameters.Remove(parameterName);
                    return true;
                }
            }
            value = defaultValue;
            return false;
        }
        private bool TryCastParameter<T>(string parameterValue, out T value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(parameterValue, out bool boolValue))
                {
                    value = (T)(object)boolValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(parameterValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(parameterValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue))
                {
                    value = (T)(object)floatValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)parameterValue;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}