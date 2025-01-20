using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioningBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] positioningPoints;
    [SerializeField] private Transform[] positioningPointsBack;
    [SerializeField] private Transform[] positioningPointsFront;
    [SerializeField] private Transform[] positioningPointsFlanks;

    public Transform GetRandomPositioningPoint()
    {
        return positioningPoints[Random.Range(0, positioningPoints.Length)];
    }
    
    public Transform GetPositioningPointBack()
    {
        return positioningPointsBack[Random.Range(0, positioningPointsBack.Length)];
    }
    
    public Transform GetPositioningPointFront()
    {
        return positioningPointsFront[Random.Range(0, positioningPointsFront.Length)];
    }
    
    public Transform GetPositioningPointFlanks()
    {
        return positioningPointsFlanks[Random.Range(0, positioningPointsFlanks.Length)];
    }
}
