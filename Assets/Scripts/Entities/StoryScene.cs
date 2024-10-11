using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
    [System.Serializable]
    public class StoryScene : ScriptableObject // Classe para armazenar informações sobre uma cena da história
    {
        public List<Sentence> sentences; // Lista de sentenças da cena
        public Sprite background; // Plano de fundo da cena
        public StoryScene nextScene; // Próxima cena da história

        [System.Serializable]
        public struct Sentence
        {
            public string text; // Texto da sentença
            public Speaker speaker; // Personagem que fala a sentença
        }
    }
    public class GameScene : ScriptableObject
    {

    }
}