using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Emotes", menuName = "ScriptableObjects/Emotes")]
public class EmoteSO : ScriptableObject
{
    public Emote[] emotes;

    [Serializable]
    public struct Emote
    {
        public string title;
        public bool isTwoHanded;
    }
}
