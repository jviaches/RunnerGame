using UnityEngine;

namespace MalbersAnimations
{
    public class FlyBehavior : StateMachineBehaviour
    {
        public float Drag = 5;
        public float DownAcceleration = 4;

        [Tooltip("If is changing from ")]
        public float DownInertia = 2;
        [Tooltip("If is changing from fall to fly this will smoothly ")]
        public float FallRecovery = 1.5f;
        [Tooltip("If Lock up is Enabled this apply to the dragon an extra Down Force")]
        public float LockUpDownForce = 4;

        float acceleration = 0;
        Rigidbody rb;
        Animal animal;

        float time;

        Vector3 FallVector;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb = animator.GetComponent<Rigidbody>();
            animal = animator.GetComponent<Animal>();
            acceleration = 0;

            animal.IsInAir = true;

            if (rb)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
 
                //Just recover if your coming from the fall animations
                FallVector = animal.CurrentAnimState == AnimTag.Fall ?   rb.velocity : Vector3.zero;
                  

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);     //Clean the Y velocity

                rb.drag = 0;
                rb.useGravity = false;
            }

            animator.applyRootMotion = true;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            time = animator.updateMode == AnimatorUpdateMode.AnimatePhysics ? Time.fixedDeltaTime : Time.deltaTime;     //Get the Time Right

            if (FallVector != Vector3.zero)                         //if last animation was falling 
            {
                animal.DeltaPosition += FallVector * time;          //Add Recovery from falling
                FallVector = Vector3.Lerp(FallVector, Vector3.zero, time * FallRecovery);
            }

            //Add more speed when going Down
            if (animal.MovementAxis.y < -0.1)
            {
                acceleration = Mathf.Lerp(acceleration, acceleration + DownAcceleration, time);
            }
            else if (animal.MovementAxis.y >= -0.1 || animal.MovementReleased)
            {
                float a = acceleration - DownInertia;
                if (a < 0) a = 0;

                acceleration = Mathf.Lerp(acceleration, a, time);  //Deacelerate slowly all the acceleration you earned..
            }

            animal.DeltaPosition += animator.velocity * (acceleration / 2) * time;

            if (animal.LockUp)
            {
                animal.DeltaPosition += Physics.gravity * LockUpDownForce * time * time;
            }

        }
    }
}