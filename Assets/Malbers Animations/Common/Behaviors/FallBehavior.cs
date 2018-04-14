using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    public class FallBehavior : StateMachineBehaviour
    {
        RaycastHit FallRay;
        
        [Tooltip("The Lower Fall animation will set to 1 if this distance the current distance to the ground")]
        public float LowerDistance;
        Animal animal;
        Rigidbody rb;
        float MaxHeight; //this is to store the max Y heigth before falling

        float FallBlend;
        bool check_Water;

        int PivotsRayInterval;
        int FallRayInterval;
        int WaterRayInterval;

        int GroundLayer;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();
            rb = animator.GetComponent<Rigidbody>();

            GroundLayer = animal.GroundLayer;

            animal.SetIntID(1);
            animal.IsInAir = true;                                          //the  Animal is on the air (This also changes the RigidBody Constraints)
            animator.SetFloat(Hash.IDFloat, 1);                             //Is use to blend between the low and high fall animations 

            MaxHeight = 0; //Resets MaxHeight

            animator.applyRootMotion = false;

            rb.drag = 0;
            rb.useGravity = true;
            FallBlend = 1;


            check_Water = false;                                            //Reset the found water parameter..
            animal.Waterlevel = Animal.LowWaterLevel;                       //Reset the Water to the lowest level while is on the Fall state
            waterlevel = Animal.LowWaterLevel;

            animal.RaycastWater();                                          //Check for WaterCasting one more time
           
            PivotsRayInterval = animal.PivotsRayInterval;
            FallRayInterval = animal.FallRayInterval;
            WaterRayInterval = animal.WaterRayInterval;

            animal.PivotsRayInterval = animal.FallRayInterval = animal.WaterRayInterval = 1;    //Set the EveryFrame to 1 so have more presision
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.IsInTransition(layerIndex))
            {
                rb.useGravity = true;
                animator.applyRootMotion = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            Debug.DrawRay(animator.transform.position, -animal.transform.up * 100, Color.magenta);


            if (Physics.Raycast(animator.transform.position, -animal.transform.up, out FallRay, 100, GroundLayer))
            {
                if (MaxHeight < FallRay.distance)
                {
                    MaxHeight = FallRay.distance; //get the Highest Distance the first time you touch the ground
                }

                //Small blend in case there's a new ground found
                FallBlend = Mathf.Lerp(FallBlend, (FallRay.distance - LowerDistance) / (MaxHeight - LowerDistance), Time.deltaTime * 20);

                animator.SetFloat(Hash.IDFloat, FallBlend); //Blend between High and Low Fall
            }
                CheckforWater();
        }

        float waterlevel;

        private void CheckforWater()
        {
            if (waterlevel != animal.Waterlevel && animal.Waterlevel != Animal.LowWaterLevel)   //Store the Water 
            {
                waterlevel = animal.Waterlevel;
            }

            if (!check_Water && waterlevel > animal.Main_Pivot_Point.y)      //Means that we found water and with the fall it might get skipped the water
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);             //Reset the rigidbody velocity
                check_Water = true;                                                     //This will avoid to do it multiple times
                animal.Swim = true;
                animal.Waterlevel = waterlevel;
            }
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animal.AirMove) AirControl();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Restore the Intervals for the raycasting
            animal.PivotsRayInterval = PivotsRayInterval;
            animal.FallRayInterval = FallRayInterval;
            animal.WaterRayInterval =WaterRayInterval;

            //if (animal.CurrentState == State.Fall) animal.CurrentState = State.Unknown;
        }

        //If the jump can be controlled on air
        void AirControl()
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);         //Keep Only the Velocity from Y

            var forward = animal.transform.forward;                 //Store the Forward Direction
            forward.y = 0;                                          //Remove the Y Value
            forward.Normalize();                                    //Normalize the Vector  
                
            float movementForward = animal.MovementForward;         //Get the Input for the Forward Direction   

            if (animal.airSmoothness <= 0)
                animal.AirSmooth = movementForward;
            else
                animal.AirSmooth = Mathf.Lerp(animal.AirSmooth, movementForward, animal.airSmoothness * Time.deltaTime);

            rb.velocity += forward * animal.AirForwardMultiplier * animal.AirSmooth;
        }
    }
}