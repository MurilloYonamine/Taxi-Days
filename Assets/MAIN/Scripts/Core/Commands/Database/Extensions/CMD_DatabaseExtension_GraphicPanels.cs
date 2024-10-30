using System;
using System.Collections;
using System.Collections.Generic;
using COMMANDS;
using GRAPHICS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace COMMANDS
{
    public class CMD_DatabaseExtension_GraphicPanels : CMD_DatabaseExtension
    {
        /*
        Add commmands to dialogue files for
        graphic panel interactions
        */
        private static string[] PARAM_PANEL = new string[] { "-p", "-panel" };
        private static string[] PARAM_LAYER = new string[] { "-l", "-layer" };
        private static string[] PARAM_MEDIA = new string[] { "-m", "-media" };
        private static string[] PARAM_GRAPHIC = new string[] { "-g", "-graphic" };
        private static string[] PARAM_SPEED = new string[] { "-spd", "-speed" };
        private static string[] PARAM_IMMEDIATE = new string[] { "-i", "-immediate" };
        private static string[] PARAM_BLENDTEX = new string[] { "-b", "-blend" };
        private static string[] PARAM_USEVIDEOAUDIO = new string[] { "-aud", "-audio" };

        private const string HOMEDIRECTORY_SYMBOL = "~/";

        new public static void Extend(CommandDatabase database)
        {
            database.AddCommand("setlayermedia", new Func<string[], IEnumerator>(SetLayerMedia));
            database.AddCommand("clearlayermedia", new Func<string[], IEnumerator>(ClearLayerMedia));
        }
        private static IEnumerator ClearLayerMedia(string[] data)
        {
            string panelName = "";
            int layer = 0;
            float transitionSpeed = 0f;
            bool immediate = false;
            string blendTexName = "";

            Texture blendTex = null;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);

            if (panel == null)
            {
                Debug.LogError($"Incapaz de encontrar o painel: '{panelName}', porque não é um painél válido. Por favor cheque o nome do painel e ajuste o comando.");
                yield break;
            }
            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: -1);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
            if (!immediate) parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);

            if (!immediate && blendTexName != string.Empty) blendTex = Resources.Load<Texture>(FilePaths.resources_blendTextures + blendTexName);

            if (layer == -1) panel.Clear(transitionSpeed, blendTex, immediate);
            else
            {
                GraphicLayer graphicLayer = panel.GetLayer(layer);
                if (graphicLayer == null)
                {
                    Debug.LogError($"Não pode limpar a camada [{layer}] do painel '{panelName}'.");
                    yield break;
                }
                graphicLayer.Clear(transitionSpeed, blendTex, immediate);
            }
        }
        private static IEnumerator SetLayerMedia(string[] data)
        {
            string panelName = "";
            int layer = 0;
            string mediaName = "";
            float transitionSpeed = 0f;
            bool immediate = false;
            string blendTexName = "";
            bool useAudio = false;

            string pathToGraphic = "";
            UnityEngine.Object graphic = null;
            Texture blendTex = null;

            var parameters = ConvertDataToParameters(data);

            parameters.TryGetValue(PARAM_PANEL, out panelName);
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel(panelName);

            if (panel == null)
            {
                Debug.LogError($"Incapaz de encontrar o painel: '{panelName}', porque não é um painél válido. Por favor cheque o nome do painel e ajuste o comando.");
                yield break;
            }
            parameters.TryGetValue(PARAM_LAYER, out layer, defaultValue: 0);
            parameters.TryGetValue(PARAM_MEDIA, out mediaName);
            parameters.TryGetValue(PARAM_IMMEDIATE, out immediate, defaultValue: false);
            if (!immediate) parameters.TryGetValue(PARAM_SPEED, out transitionSpeed, defaultValue: 1f);
            parameters.TryGetValue(PARAM_BLENDTEX, out blendTexName);
            parameters.TryGetValue(PARAM_USEVIDEOAUDIO, out useAudio, defaultValue: false);

            pathToGraphic = GetPathToGraphic(FilePaths.resources_backgroundImages, mediaName);
            graphic = Resources.Load<Texture>(pathToGraphic);

            if (graphic == null)
            {
                pathToGraphic = GetPathToGraphic(FilePaths.resources_backgroundVideos, mediaName);
                graphic = Resources.Load<VideoClip>(pathToGraphic);
            }
            if (graphic == null)
            {
                Debug.LogError($"Incapaz de encontrar uma media com o nome: '{mediaName}' na pasta Resources. Por favor cheque o nome e veja se realmente existe!");
                yield break;
            }

            if (!immediate && blendTexName != string.Empty) blendTex = Resources.Load<Texture>(FilePaths.resources_blendTextures + blendTexName);

            GraphicLayer graphicLayer = panel.GetLayer(layer, createIfDoesNotExist: true);

            if (graphic is Texture)
            {
                yield return graphicLayer.SetTexture(graphic as Texture, transitionSpeed, blendTex, pathToGraphic, immediate);
            }
            else
            {
                yield return graphicLayer.SetVideo(graphic as VideoClip, transitionSpeed, useAudio, blendTex, pathToGraphic, immediate);
            }
        }
        private static string GetPathToGraphic(string defaultPath, string graphicName)
        {
            if (graphicName.StartsWith(HOMEDIRECTORY_SYMBOL)) return graphicName.Substring(HOMEDIRECTORY_SYMBOL.Length);
            return defaultPath + graphicName;
        }
    }
}