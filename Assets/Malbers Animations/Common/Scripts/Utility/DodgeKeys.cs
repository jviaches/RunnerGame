using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    /// <summary>
    /// This is for using the left and right key for acivating the Dodge animations
    /// </summary>
    public class DodgeKeys : MonoBehaviour
    {

        Animal animal;
        public float DoubleKeyTime = 0.3f;

        bool DodgeLeftPressOne;
        bool DodgeRightPressOne;

        public InputRow DodgeLeft = new InputRow(KeyCode.A);
        public InputRow DodgeRight = new InputRow(KeyCode.D);

        // Use this for initialization
        void Start()
        {
            animal = GetComponent<Animal>();
        }

        // Update is called once per frame
        void Update()
        {

            if (DodgeLeft.GetInput && !DodgeLeftPressOne)
            {
                DodgeLeftPressOne = true;
                Invoke("ResetDodgeKeys", DoubleKeyTime);
            }
            else if (DodgeLeft.GetInput && DodgeLeftPressOne)
            {
                animal.Dodge = true;
                Invoke("ResetDodgeKeys", 0.1f);
            }


            if (DodgeRight.GetInput && !DodgeRightPressOne)
            {
                DodgeRightPressOne = true;
                Invoke("ResetDodgeKeys", DoubleKeyTime);
            }
            else if (DodgeRight.GetInput && DodgeRightPressOne)
            {
                animal.Dodge = true;
                Invoke("ResetDodgeKeys", 0.1f);
            }

        }

        void ResetDodgeKeys()
        {
            DodgeLeftPressOne = false;
            DodgeRightPressOne = false;
            animal.Dodge = false;
        }
    }
}