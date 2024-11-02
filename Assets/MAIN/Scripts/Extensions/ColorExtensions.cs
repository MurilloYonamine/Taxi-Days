using UnityEngine;

/// <summary>
/// Classe de métodos de extensão para a estrutura <see cref="Color"/>.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Define o valor do canal alfa (transparência) de uma cor.
    /// </summary>
    /// <param name="original">A cor original.</param>
    /// <param name="alpha">O valor do canal alfa (0 a 1).</param>
    /// <returns>Uma nova cor com o valor alfa especificado.</returns>
    public static Color SetAlpha(this Color original, float alpha)
    {
        return new Color(original.r, original.g, original.b, alpha);
    }

    /// <summary>
    /// Obtém uma cor predefinida a partir de um nome.
    /// </summary>
    /// <param name="original">A cor original (não utilizada neste método).</param>
    /// <param name="colorName">O nome da cor desejada.</param>
    /// <returns>A cor correspondente ao nome fornecido, ou <see cref="Color.clear"/> se o nome não for reconhecido.</returns>
    /// <remarks>
    /// Nomes de cores reconhecidos (case insensitive): "red", "green", "blue", "yellow", "white", "black", "gray", "cyan", "magenta", "orange".
    /// </remarks>
    public static Color GetColorFromName(this Color original, string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red":
                return Color.red;
            case "green":
                return Color.green;
            case "blue":
                return Color.blue;
            case "yellow":
                return Color.yellow;
            case "white":
                return Color.white;
            case "black":
                return Color.black;
            case "gray":
                return Color.gray;
            case "cyan":
                return Color.cyan;
            case "magenta":
                return Color.magenta;
            case "orange":
                return new Color(1f, 0.5f, 0f); // Orange is not a predefined color, so we create it manually
            default:
                Debug.LogWarning("Nome de cor irreconhecível: " + colorName);
                return Color.clear;
        }
    }
}
