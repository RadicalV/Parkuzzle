using UnityEngine;

public class Tracer
{
    public static Trace TraceCapsule(Vector3 point1, Vector3 point2, float radius, Vector3 start, Vector3 destination, float contactOffset, int layerMask)
    {
        var result = new Trace()
        {
            startPos = start,
            endPos = destination
        };

        var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
        radius *= (1f - contactOffset);
        var direction = (destination - start).normalized;
        var maxDistance = Vector3.Distance(start, destination) + longSide;

        if (Physics.CapsuleCast(
            point1: point1,
            point2: point2,
            radius: radius,
            direction: direction,
            hitInfo: out RaycastHit hit,
            maxDistance: maxDistance,
            layerMask: layerMask))
        {
            result.fraction = hit.distance / maxDistance;
            result.hitCollider = hit.collider;
            result.hitPoint = hit.point;
            result.planeNormal = hit.normal;
        }
        else
        {
            result.fraction = 1;
        }

        return result;
    }
}