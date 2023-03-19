using UnityEngine;

public struct Trace
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float fraction;
    public Collider hitCollider;
    public Vector3 hitPoint;
    public Vector3 planeNormal;
}
