using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadBehavior : MonoBehaviour {

    public GameObject road;
    public GameObject coin;
    private Vector3 lastpos = new Vector3(0f, 0f, 0f);
    private Queue<GameObject> roadObjectQueue = new Queue<GameObject>(20);

    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject _platform = Instantiate(road) as GameObject;
            _platform.transform.position = lastpos + new Vector3(1f, 0f, 0f);
            lastpos = _platform.transform.position;

            roadObjectQueue.Enqueue(_platform);
        }

        InvokeRepeating("SpawnPlatform", 2f, 0.2f);

    }

    void SpawnPlatform()
    {
        GameObject _platform;
        GameObject _coin;        

        int random = Random.Range(0, 2);
        if (random == 0)
        { 
            _platform =  roadObjectQueue.Dequeue();
            _platform.transform.position = lastpos + new Vector3(1f, 0f, 0f); // set on X
            lastpos = _platform.transform.position;

        }
        else
        { 
            _platform = roadObjectQueue.Dequeue();
            _platform.transform.position = lastpos + new Vector3(0f, 0f, 1f); // set on Z
            lastpos = _platform.transform.position;
        }

        if (Random.value > 0.5) //%5
        {
            _coin = Instantiate(coin) as GameObject;
            _coin.transform.position = lastpos + new Vector3(0f, 1f, 0f);
        }

        roadObjectQueue.Enqueue(_platform);
    }
}