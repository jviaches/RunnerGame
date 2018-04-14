namespace MalbersAnimations
{
    interface IWayPoint
    {
        UnityEngine.Transform NextTarget { get; }
        float StoppingDistance { get; }
    }
}