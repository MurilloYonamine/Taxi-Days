using UnityEngine;

public class FilePaths
{
    /*
    Classe de utilidade para fornecer os caminhos de arquivos e diretórios.
    */

    // Define o caminho raiz para os arquivos de dados do jogo. Usa o caminho da pasta Assets do Unity.
    public static readonly string root = $"{Application.dataPath}/gameData/";

    // Resources Paths
    public static readonly string resources_graphics = "Graphics/";
    public static readonly string resources_backgroundImages = $"{resources_graphics}Background Images/";
    public static readonly string resources_backgroundVideos = $"{resources_graphics}Background Videos/";
    public static readonly string resources_blendTextures = $"{resources_graphics}Transition Effects/";
}
