using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "New Speaker", menuName = "Data/New Speaker")]
    [System.Serializable]
    public class Speaker : ScriptableObject // Classe para armazenar informações sobre um personagem que fala
    {
        public string speakerName; // Nome do personagem
        public Color textColor; // Cor do texto do personagem
    }
}