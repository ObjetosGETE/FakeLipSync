using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "000-Subtitle", menuName = "VisSubtitle")]
public class VisSubtitle : ScriptableObject
{
    public AudioClip audioClip;
    public TextAsset jsonFile;
}
