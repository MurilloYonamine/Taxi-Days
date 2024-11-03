using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace History
{
    /// <summary>
    /// A cache for all the assets that have been loaded in the game.
    /// </summary>
    public class HistoryCache
    {
        public static Dictionary<string, (object asset, int staleIndex)> loadedAssets = new Dictionary<string, (object asset, int staleIndex)>();

        public static T TryLoadObject<T>(string key)
        {
            object resource = null;

            if (loadedAssets.ContainsKey(key))
            {
                resource = (T)loadedAssets[key].asset;
            }
            else
            {
                resource = Resources.Load(key);
                if (resource != null)
                {
                    loadedAssets[key] = (resource, 0);
                }
            }

            if (resource != null)
            {
                if (resource is T) return (T)resource;
                Debug.LogWarning($"Objeto do cache não é do tipo esperado: '{key}'");
            }

            Debug.LogWarning($"Não pode carregar objeto do cache: '{key}'");
            return default(T);
        }
        public static TMP_FontAsset LoadFont(string key) => TryLoadObject<TMP_FontAsset>(key);
        public static AudioClip LoadAudio(string key) => TryLoadObject<AudioClip>(key);
        public static Texture2D LoadImage(string key) => TryLoadObject<Texture2D>(key);
        public static VideoClip LoadVideo(string key) => TryLoadObject<VideoClip>(key);
    }
}