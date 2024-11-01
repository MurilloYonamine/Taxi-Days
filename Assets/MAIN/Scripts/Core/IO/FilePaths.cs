using UnityEngine;

public class FilePaths
{
    /*
    Classe de utilidade para fornecer os caminhos de arquivos e diretórios.
    */

    // Define o caminho raiz para os arquivos de dados do jogo. Usa o caminho da pasta Assets do Unity.
    private const string HOME_DIRECTORY_SYMBOL = "~/";
    public static readonly string root = $"{Application.dataPath}/gameData/";

    // Resources Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}Background Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}Background Videos/";
    public static readonly string resources_blendTextures = $"{resources_graphics}Transition Effects/";

    public static readonly string resources_audio = "Audio/";
    public static readonly string resources_sfx = $"{resources_audio}SFX/";
    public static readonly string resources_voices = $"{resources_audio}Voices/";
    public static readonly string resources_music = $"{resources_audio}Music/";
    public static readonly string resources_ambience = $"{resources_audio}Ambience/";

    public static readonly string resources_dialogueFiles = $"Dialogue Files/";


    public static string GetPathToResource(string defaultPath, string resourceName)
    {
        if (resourceName.StartsWith(HOME_DIRECTORY_SYMBOL))
        {
            return resourceName.Substring(HOME_DIRECTORY_SYMBOL.Length);
        }
        return defaultPath + resourceName;
    }
}
