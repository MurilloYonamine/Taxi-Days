using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class TagManager
{
    /*
    Injects data into tags contained
    within dialogue files
    */
    private static readonly Dictionary<string, Func<string>> tags = new Dictionary<string, Func<string>>()
    {
        { "<mainChar>", () => "Avira" },
        { "<time>", () => DateTime.Now.ToString("HH:mm tt") },
        { "<playerLevel>", () => "15" },
        { "<tempVal1>", () => "42" }
    };
    private static readonly Regex tagRegex = new Regex("<\\w+>");

    public static string Inject(string text)
    {
        if (tagRegex.IsMatch(text))
        {
            foreach (Match match in tagRegex.Matches(text))
            {
                string tag = match.Value;
                if (tags.TryGetValue(match.Value, out var tagValueRequest))
                {
                    text = text.Replace(match.Value, tagValueRequest());
                }
            }
        }
        return text;
    }
}
