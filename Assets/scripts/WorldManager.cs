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
    public int Level = 1;
    
    #region Singleton
    private static WorldManager instance;
    private object lockingObect = null;
    private WorldManager() { }

    public WorldManager Instance
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

    public void Awake()
    {
        DontDestroyOnLoad(this);    // survice scene loads
    }
}