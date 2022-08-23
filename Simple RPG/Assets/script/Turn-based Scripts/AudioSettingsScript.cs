using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingsScript : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private float musicFloat;
    public AudioSource musicAudio;

    private void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        musicFloat = PlayerPrefs.GetFloat(MusicPref);

        musicAudio.volume = musicFloat;
    }
}
