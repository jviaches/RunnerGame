using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    #region Unity Editor
    public Button OkMenuButton;

    public Button MusicMenuButtonOn;
    public Button MusicMenuButtonOff;

    public Button EffectsMenuButtonOn;
    public Button EffectsMenuButtonOff;

    public Slider VolumeLevelSlider;

    private float volumeLevel = 0;
    #endregion

    void Start()
    {
        OkMenuButton.GetComponent<Button>().onClick.AddListener(OkMenuButtonClick);

        MusicMenuButtonOn.GetComponent<Button>().onClick.AddListener(MusicOnMenuButtonClick);
        MusicMenuButtonOff.GetComponent<Button>().onClick.AddListener(MusicOffMenuButtonClick);

        EffectsMenuButtonOn.GetComponent<Button>().onClick.AddListener(EffectsOnMenuButtonClick);
        EffectsMenuButtonOff.GetComponent<Button>().onClick.AddListener(EffectsOffMenuButtonClick);

        volumeLevel = VolumeLevelSlider.value;
        VolumeLevelSlider.onValueChanged.AddListener(volumeLevelChanged);
    }

    private void volumeLevelChanged(float newVolumeLevel)
    {
        WorldManager.Instance.ChangeMusicVolume(newVolumeLevel);
        Debug.Log("volumeLevel = " + WorldManager.Instance.Volume);
    }

    private void OkMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void MusicOnMenuButtonClick()
    {
        WorldManager.Instance.UnMuteMusic();
        Debug.Log("Music is " + WorldManager.Instance.isMusicOn);
    }

    private void MusicOffMenuButtonClick()
    {
        WorldManager.Instance.MuteMusic();
        Debug.Log("Music is " + WorldManager.Instance.isMusicOn);
    }

    private void EffectsOnMenuButtonClick()
    {
        //WorldManager.Instance.isMusicEffectsOn = false;
        //Debug.Log("MusicEffectsOff is " + WorldManager.Instance.isMusicEffectsOn);
    }

    private void EffectsOffMenuButtonClick()
    {
        //WorldManager.Instance.isMusicEffectsOn = true;
        //Debug.Log("MusicEffectsOff is " + WorldManager.Instance.isMusicEffectsOn);
    }
}
