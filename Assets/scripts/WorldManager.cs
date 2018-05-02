using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Assets.scripts.DialogModule;
using UnityEngine.Events;

public class WorldManager
{ 
    public int Level = 4;

    public bool isMusicOn = true;
    public bool isMusicEffectsOn = true;
    public float Volume = 10;

    #region Singleton
    private static WorldManager instance;
    private static object lockingObect = new object();
    private WorldManager() { }

    public static WorldManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockingObect)
                {
                    if (instance == null)
                    {
                        instance = new WorldManager();
                    }
                }
            }
            return instance;
        }

       
    }

    #endregion

}