using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class TagManager
{
    /*
    Injects data into tags contained
    within dialogue files
    */
    private readonly Dictionary<string, Func<string>> tags = new Dictionary<string, Func<string>>();
    private readonly Regex tagRegex = new Regex("<\\w+>");

    public TagManager()
    {
        InitializeTags();
    }
    private void InitializeTags()
    {
        tags["<mainChar>"] = () => "Avira";
        tags["<time>"] = () => DateTime.Now.ToString("HH:mm tt");
        tags["<playerLevel>"] = () => "15";
        tags["<tempVal1>"] = () => "42";
    }
    public string Inject(string text)
    {
        if(tagRegex.IsMatch(text))
        {
            foreach(Match match in tagRegex.Matches(text))
            {
                string tag = match.Value;
                if(tags.TryGetValue(match.Value, out var tagValueRequest))
                {
                    text = text.Replace(match.Value, tagValueRequest());
                }
            }
        }
        return text;
    }
}
