using UnityEngine;
using System.Collections;
using MalbersAnimations.Events;
using MalbersAnimations.Utilities;

#if UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

namespace MalbersAnimations
{
   

    /// <summary>
    /// Control for the Animal using Nav Mesh Agent
    /// </summary>
    [RequireComponent(typeof(Animal))]
    public class AnimalAIControl : MonoBehaviour
    {
        protected static Vector3 NullVector = MalbersTools.NullVector; //the Way to know if a vector is invalid

        #region Components References
        private NavMeshAgent agent;                 //The NavMeshAgent
        protected Animal animal;                    //The Animal Script
        #endregion

        #region Target Verifications
        //protected Animal AnimalTarget;          //To check if the target is an animal

        /// <summary>
        /// Is the Target an Action Zone
        /// </summary>
        protected ActionZone isActionZone;  
        /// <summary>
        /// Is the Target a WayPoint
        /// </summary>
        protected MWayPoint isWayPoint;
        #endregion

        #region Internal Variables
        /// <summary>
        /// Desired Position to go to
        /// </summary>
        protected Vector3 targetPosition = NullVector;
        protected float RemainingDistance;
        protected float DefaultStopDistance;
        /// <summary>
        /// Used to Check if you enter once on a OffMeshLink
        /// </summary>
        protected bool EnterOFFMESH;
        protected bool OnAction;                            //Check if the animal is making an Action nimation  
        protected bool isFlying = false;                    //is the Animal Flying

        IWayPoint NextWayPoint;
        #endregion

        #region Public Variables
        [SerializeField]
        protected float stoppingDistance = 0.6f;
        [SerializeField]
        protected Transform target;                    //The Target
        public bool AutoSpeed = true;
        public float ToTrot = 6f;
        public float ToRun = 8f;
        public bool debug = false;                          //Debuging 
        #endregion

        #region Properties
        /// <summary>
        /// the navmeshAgent asociate to this GameObject
        /// </summary>
        public NavMeshAgent Agent
        {
            get
            {
                if (agent == null)
                {
                    agent = GetComponentInChildren<NavMeshAgent>();
                }
                return agent;
            }
        }

        public float StoppingDistance
        {
            get { return stoppingDistance; }

            set { Agent.stoppingDistance = stoppingDistance = value; }
        }
        #endregion

        public Vector3Event OnTargetPositionArrived = new Vector3Event();
        public TransformEvent OnTargetArrived = new TransformEvent();

        void Start() { StartAgent(); }

        void Update()
        {
            if (!Agent.isOnNavMesh || !Agent.enabled) return;               //No nothing if we are not on a Nav mesh or the Agent is disabled

            Agent.nextPosition = agent.transform.position;                  //Update the Agent Position to the Transform position

            if (targetPosition == NullVector) { agent.isStopped = true; }
            else { UpdateAgent(); }
        }

        /// <summary>
        /// Initialize the Ai Animal Control Values
        /// </summary>
        protected virtual void StartAgent()
        {
            animal = GetComponent<Animal>();

            animal.OnAnimationChange.AddListener(OnAnimationChanged);           //Listen when the animations changes..

            Agent.updateRotation = false;                                       //The animator will control the rotation and postion.. NOT THE AGENT
            Agent.updatePosition = false;
            DefaultStopDistance = Agent.stoppingDistance = StoppingDistance;    //Store the Started Stopping Distance

            SetTarget(target);                                                  //Set the first Target

        }

        protected bool EnterAction;
        /// <summary>
        /// This will be Called everytime the animal changes an animation (Via Unity Event)
        /// </summary>
        protected virtual void OnAnimationChanged(int animTag)
        {
            //if (isFlying) return;

             OnAction = (animTag == AnimTag.Action);                                //Check if the Animal is making an Action

            if (animTag == AnimTag.Jump) animal.MovementRight = 0;                   //Don't rotate if is in the middle of a jump


            if (animTag == AnimTag.Locomotion || animTag == AnimTag.Idle || animTag == AnimTag.Recover)           //Activate the Agent when the animal is moving
            {
                Agent.enabled = true;
                Agent.ResetPath();
                if (debug) Debug.Log("Enable Agent. Animal " + name + " is Moving");
                EnterOFFMESH = false;

                if (targetPosition != NullVector) //Resume the the path
                {
                    Agent.SetDestination(targetPosition);
                    Agent.isStopped = false;
                }
            }
            else   //Disable the Agent whe is not on Locomotion or Idling
            {
                Agent.enabled = false;
                if (debug) Debug.Log("Disable Agent. Animal " + name + " is doing an action, jumping or falling");
            }

            if (OnAction && !EnterAction)
            {
                EnterAction = true;
            }
            else if (!OnAction && EnterAction)
            {
                if (isActionZone)
                {
                    SetTarget(isActionZone.NextTarget);
                    EnterAction = false;
                }
            }
        }

        /// <summary>
        /// Updates the Agents using he animation root motion
        /// </summary>
        protected virtual void UpdateAgent()
        {
           
            Vector3 Direction = Vector3.zero;                               //Reset the Direction (THIS IS THE DIRECTION VECTOR SENT TO THE ANIMAL)       

            //Store the remaining distance -- but if navMeshAgent is still looking for a path Keep Moving
            RemainingDistance = Agent.remainingDistance;
            // RemainingDistance = Agent.remainingDistance <=0 ? float.PositiveInfinity : Agent.remainingDistance;


            if (Agent.pathPending || Mathf.Abs(RemainingDistance) <=0.1f)
            {
                RemainingDistance = float.PositiveInfinity;
                agent.SetDestination(targetPosition);
            }
            
            //  if (RemainingDistance == 0) RemainingDistance = float.PositiveInfinity;

            if (RemainingDistance > StoppingDistance)                   //if haven't arrived yet to our destination  
            {
                Direction = Agent.desiredVelocity.normalized;
                OnAction = false;
            }
            else  //if we get to our destination                                                          
            {

                OnTargetPositionArrived.Invoke(targetPosition);
                if (target) OnTargetArrived.Invoke(target);

                targetPosition = NullVector;                                //Clean the TargetPosition
                agent.isStopped = true;


                if (isActionZone && !OnAction)                           //If the Target is an Action Zone Start the Action
                {
                    OnAction = true;
                    animal.Action = true;                                //Activate the Action on the Animal
                }
               else if (isWayPoint)                                     //If the Next Target is a Waypoint
                {
                    SetTarget(isWayPoint ? isWayPoint.NextTarget.transform : null);
                }
            }

            animal.Move(Direction);                                     //Set the Movement to the Animal

            if (AutoSpeed) AutomaticSpeed();                            //Set Automatic Speeds
            CheckOffMeshLinks();                                        //Jump/Fall behaviour 
        }


        /// <summary>
        /// Manage all Off Mesh Links
        /// </summary>
        protected virtual void CheckOffMeshLinks()
        {
            if (isFlying) return;

            if (Agent.isOnOffMeshLink && !EnterOFFMESH)         //Check if the Agent is on a OFF MESH LINK
            {
                EnterOFFMESH = true;                                            //Just to avoid entering here again while we are on a OFF MESH LINK
                OffMeshLinkData OMLData = Agent.currentOffMeshLinkData;

                if (OMLData.linkType == OffMeshLinkType.LinkTypeManual)    //Means that it has a OffMesh Link component
                {
                    OffMeshLink CurrentOML = OMLData.offMeshLink;                           //Check if the OffMeshLink is a Custom Off Mesh Link
                
                    ActionZone OffMeshZone =
                        CurrentOML.GetComponentInParent<ActionZone>();

                    if (OffMeshZone && !OnAction)                                       //if the OffmeshLink is a zone and is not making an action
                    {
                        animal.Action = OnAction = true;                                //Activate the Action on the Animal
                        return;
                    }

                    var DistanceEnd = (transform.position - CurrentOML.endTransform.position).sqrMagnitude;
                    var DistanceStart = (transform.position - CurrentOML.startTransform.position).sqrMagnitude;
                    var NearTransform = DistanceEnd < DistanceStart ? CurrentOML.endTransform : CurrentOML.startTransform;

                    StartCoroutine(MalbersTools.AlignTransformsC(transform, NearTransform, 0.15f, false, true)); //Aling the Animal to the Link Position

                    if (CurrentOML.area == 2) animal.SetJump();                         //if the OffMesh Link is a Jump type

                    try
                    {
                        if (CurrentOML.CompareTag("Fly"))
                        {
                            animal.SetFly(true);
                            isFlying = true;

                            StartCoroutine(FlyTowardsTarget(CurrentOML.endTransform));
                            return;
                        }
                    }
                    catch
                    {}

                 
                }
                else if (OMLData.linkType == OffMeshLinkType.LinkTypeJumpAcross)             //Means that it has a OffMesh Link component
                {
                    animal.SetJump();
                }
            }
        }

        internal IEnumerator FlyTowardsTarget(Transform target)
        {
            float distance = float.MaxValue;
            agent.enabled = false;
            while (distance > Agent.stoppingDistance)
            {
                animal.Move((target.position - animal.transform.position));
                distance = Vector3.Distance(animal.transform.position, target.position);
                yield return null;
            }

            animal.SetFly(false);
            isFlying = false;
        }

        /// <summary>
        /// Change velocities
        /// </summary>
        protected virtual void AutomaticSpeed()
        {
            if (RemainingDistance < ToTrot)         //Set to Walk
            {
                animal.Speed1 = true;
            }
            else if (RemainingDistance < ToRun)     //Set to Trot
            {
                animal.Speed2 = true;
            }
            else if (RemainingDistance > ToRun)     //Set to Run
            {
                animal.Speed3 = true;
            }
        }

        /// <summary>
        /// Set to next Target
        /// </summary>
        public virtual void SetTarget(Transform target)
        {
            if (!Agent.isOnNavMesh) return;               //No nothing if we are not on a Nav mesh or the Agent is disabled
            if (target == null) return;             //If there's no target Skip the code
           
            this.target = target;
            targetPosition = target.position;       //Update the Target Position 

            isActionZone = target.GetComponent<ActionZone>();       
            isWayPoint = target.GetComponent<MWayPoint>();
            NextWayPoint = target.GetComponent<IWayPoint>(); //Check if the Next Target has Next Waypoints

            StoppingDistance = NextWayPoint != null ? NextWayPoint.StoppingDistance :  DefaultStopDistance;

            if (debug) Debug.Log("Target Updated: " + target.name);


            Agent.SetDestination(targetPosition);                       //If there's a position to go to set it as destination
            Agent.isStopped = false;

        }

        /// <summary>
        /// Use this for Targets that changes their position
        /// </summary>
        public virtual void UpdateTargetTransform()
        {
            if (!Agent.isOnNavMesh) return;         //No nothing if we are not on a Nav mesh or the Agent is disabled
            if (target == null) return;             //If there's no target Skip the code
            targetPosition = target.position;       //Update the Target Position 
            Agent.SetDestination(targetPosition);   //If there's a position to go to set it as destination
            Agent.isStopped = false;
        }

        /// <summary>
        /// Set a Vector Position Destination
        /// </summary>
        public virtual void SetDestination(Vector3 point)
        {
            targetPosition = point;
            target = null;                              //Clean the Target
            StoppingDistance = DefaultStopDistance;     //Reset the Stopping Distance

            if (!Agent.isOnNavMesh || !Agent.enabled) return;               //No nothing if we are not on a Nav mesh or the Agent is disabled
            Agent.SetDestination(targetPosition);                       //If there's a position to go to set it as destination
            Agent.isStopped = false;

            if (debug) Debug.Log("Target Position Updated: " + point);
        }

        //Toogle Off and On the Agent
       protected virtual IEnumerator ToogleAgent()
        {
            Agent.enabled = false;
            yield return null;
            Agent.enabled = true;
        }


#if UNITY_EDITOR
        [HideInInspector] public bool showevents;
        /// <summary>
        /// DebugOptions
        /// </summary>
        void OnDrawGizmos()
        {
            if (!debug) return;

            if (Agent == null) { return; }
            if (Agent.path == null) { return; }

            Color lGUIColor = Gizmos.color;

            Gizmos.color = Color.green;
            for (int i = 1; i < Agent.path.corners.Length; i++)
            {
                Gizmos.DrawLine(Agent.path.corners[i - 1], Agent.path.corners[i]);
            }


            if (AutoSpeed)
            {
                Vector3 pos = Agent ? Agent.transform.position : transform.position;
                Pivots P = GetComponentInChildren<Pivots>();
                pos.y = P.transform.position.y;

                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, ToRun);

                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, ToTrot);

                if (Agent)
                {
                    UnityEditor.Handles.color = Color.red;
                    UnityEditor.Handles.DrawWireDisc(pos, Vector3.up, Agent.stoppingDistance);
                }
            }
        }
#endif
    }
}
