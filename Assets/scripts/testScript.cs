using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    Animal animal;
    // Use this for initialization
    void Start() {
        animal = GetComponentInParent<Animal>();
        animal.Speed3 = true;
    }

    // Update is called once per frame
    void Update() {

        animal.Speed = 30;
    }
}
