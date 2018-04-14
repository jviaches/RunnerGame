    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    //Controls the movement while underwater
    public class UnderWaterBehaviour : StateMachineBehaviour {

        [Range(0,90)]
        public float Bank;
        public float upDownSmoothness = 2;
        [Range(0, 90)]
        public float Ylimit = 87;

        protected Rigidbody rb;
        protected Animal animal;
        protected Transform transform;
        protected Quaternion DeltaRotation;
        protected float UnderWaterShift;
        protected float time;

        int WaterLayer;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();
            animal = animator.GetComponent<Animal>();

            animator.applyRootMotion = false;                   //No Root Motion
            transform = animator.transform;
            DeltaRotation = transform.rotation;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            if (!animal.CanGoUnderWater) return;
            rb.useGravity = false;

            WaterLayer = LayerMask.GetMask("Water");
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.Normal ? Time.deltaTime : Time.fixedDeltaTime;

            if (!animal.CanGoUnderWater || !animal.Underwater) return;
           

            if (animal.Jump) animal.Down = false;       //Cannot press at the same time Down and UP(Jump)

            transform.rotation = DeltaRotation;         //Store the Rotation before rotating

            UnderWaterShift = Mathf.Lerp(UnderWaterShift, animal.Shift ? 2f : 1f, animal.underWaterSpeed.lerpPosition * time);
          

            animal.YAxisMovement(upDownSmoothness, time);


            transform.RotateAround(animal.AnimalMesh.bounds.center,transform.up, Mathf.Clamp(animal.Direction, -1, 1) * animal.underWaterSpeed.rotation);         //Rotate while going forward

            float Up = animal.MovementAxis.y;

            if (animal.MovementAxis.z < 0) Up = 0;                                                      //Remove Rotations When going backwards

            Vector3 forward = (transform.forward * animal.Speed) + (transform.up * Up);                 //Calculate the Direction to Move

        
            if (forward.magnitude > 1) forward.Normalize();                                             //Remove extra Speed

            //transform.position = Vector3.Lerp(transform.position, transform.position + (forward * animal.underWaterSpeed.position * UnderWaterShift), Time.fixedDeltaTime * animal.underWaterSpeed.lerpPosition); //Move it
            rb.velocity = forward * animal.underWaterSpeed.position * UnderWaterShift;

            DeltaRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation; //Keep only Aligned Rotation the horizontal

            if (forward.magnitude > 0.001)
            {
                float angle = 90 - Vector3.Angle(Vector3.up, forward);

                float smooth = Mathf.Max(Mathf.Abs(animal.MovementAxis.y), Mathf.Abs(animal.Speed));

                transform.Rotate(Mathf.Clamp(-angle, -Ylimit, Ylimit) * smooth, 0, 0, Space.Self);          //Rotate to the direction is swimming
            }

            transform.Rotate(0, 0, -Bank * animal.Direction, Space.Self);                                   //Rotation Bank

            CheckExitUnderWater();

        }

        protected void CheckExitUnderWater()
        {
            //To Get Out of the Water---------------------------------
            RaycastHit UnderWaterHit;

            Vector3 origin = transform.position + new Vector3(0, (animal.height - animal.waterLine) * animal.ScaleFactor, 0);

            if (Physics.Raycast(origin, -Vector3.up, out UnderWaterHit, animal.ScaleFactor, WaterLayer))
            {
                if (!animal.Down)
                {
                    animal.Underwater = false;
                    animal.Anim.applyRootMotion = true;
                    rb.useGravity = true;
                    rb.drag = 0;

                    rb.constraints = Animal.Still_Constraints;

                    animal.MovementAxis = new Vector3(animal.MovementAxis.x, 0, animal.MovementAxis.z);
                }
            }
        }
    }
}