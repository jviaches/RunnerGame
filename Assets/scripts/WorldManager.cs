using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Assets.scripts.DialogModule;
using UnityEngine.Events;

public class WorldManager : MonoBehaviour
{ 
    public int Level = 4;

    public bool isMusicOn = true;
    public bool isMusicEffectsOn = true;
    public float Volume = 10;

    #region Singleton
    public static WorldManager Instance;
    private AudioSource audioSource;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = Volume / 100;
        audioSource.enabled = true;

        if (isMusicOn)
            audioSource.Play();
        else
            audioSource.Stop();
    }

    public void ChangeMusicVolume(float newVolume)
    {
        if (newVolume >= 1f)
            newVolume = newVolume / 100;

        audioSource.volume = newVolume;
    }

    public void MuteMusic()
    {
        audioSource.Stop();
        isMusicOn = false;
    }

    public void UnMuteMusic()
    {
        audioSource.Play();
        isMusicOn = true;
    }

    #endregion

}