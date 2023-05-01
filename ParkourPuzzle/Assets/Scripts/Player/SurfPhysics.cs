using UnityEngine;

public class SurfPhysics
{
    public static int groundLayerMask = LayerMask.GetMask(new string[] { "Default", "Ground", "Player clip" });
    private static Collider[] _colliders = new Collider[MaxCollisions];
    private static Vector3[] _planes = new Vector3[MaxClipPlanes];
    private const int MaxCollisions = 128;
    private const int MaxClipPlanes = 5;
    private const int NumBumps = 1;
    public const float SurfSlope = 0.7f;

    public static void ResolveCollisions(Collider collider, ref Vector3 origin, ref Vector3 velocity)
    {
        var capc = collider as CapsuleCollider;
        GetCapsulePoints(capc, origin, out Vector3 point1, out Vector3 point2);

        int numOverlaps = Physics.OverlapCapsuleNonAlloc(point1, point2, capc.radius, _colliders, groundLayerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < numOverlaps; i++)
        {
            if (Physics.ComputePenetration(collider, origin, Quaternion.identity, _colliders[i], _colliders[i].transform.position,
                _colliders[i].transform.rotation, out Vector3 direction, out float distance))
            {
                direction.Normalize();
                Vector3 penetrationVector = direction * distance;
                Vector3 velocityProjected = Vector3.Project(velocity, -direction);
                velocityProjected.y = 0;
                origin += penetrationVector;
                velocity -= velocityProjected;
            }
        }
    }

    public static int ClipVelocity(Vector3 input, Vector3 normal, ref Vector3 output, float overbounce)
    {
        var angle = normal[1];
        var blocked = 0x00;     // Assume unblocked.
        if (angle > 0)          // If the plane that is blocking us has a positive z component, then assume it's a floor.
            blocked |= 0x01;
        if (angle == 0)         // If the plane has no Z, it is vertical (wall/step)
            blocked |= 0x02;

        // Determine how far along plane to slide based on incoming direction.
        var backoff = Vector3.Dot(input, normal) * overbounce;

        for (int i = 0; i < 3; i++)
        {
            var change = normal[i] * backoff;
            output[i] = input[i] - change;
        }

        // iterate once to make sure we aren't still moving through the plane
        float adjust = Vector3.Dot(output, normal);
        if (adjust < 0.0f)
        {
            output -= (normal * adjust);
        }

        // Return blocking flags.
        return blocked;
    }

    public static int Reflect(ref Vector3 velocity, Collider collider, Vector3 origin, float deltaTime)
    {
        float d;
        var newVelocity = Vector3.zero;
        var blocked = 0;                  // Assume not blocked
        var numplanes = 0;                //  and not sliding along any planes
        var originalVelocity = velocity;  // Store original velocity
        var primalVelocity = velocity;

        var allFraction = 0f;
        var timeLeft = deltaTime;   // Total time for this movement operation.

        for (int bumpcount = 0; bumpcount < NumBumps; bumpcount++)
        {
            if (velocity.magnitude == 0f)
            {
                break;
            }

            // Assume we can move all the way from the current origin to the end point.
            var end = VectorExtensions.VectorMa(origin, timeLeft, velocity);
            CapsuleCollider capc = collider as CapsuleCollider;
            GetCapsulePoints(capc, origin, out Vector3 point1, out Vector3 point2);
            var trace = Tracer.TraceCapsule(point1, point2, capc.radius, origin, end, collider.contactOffset, groundLayerMask);

            allFraction += trace.fraction;

            if (trace.fraction > 0)
            {
                // actually covered some distance
                originalVelocity = velocity;
                numplanes = 0;
            }

            // If we covered the entire distance, we are done and can return.
            if (trace.fraction == 1)
            {
                break;      // moved the entire distance
            }

            // If the plane we hit has a high y component in the normal, then it's probably a floor
            if (trace.planeNormal.y > SurfSlope)
            {
                blocked |= 1;       // floor
            }

            // If the plane has a zero y component in the normal, then it's a step or wall
            if (trace.planeNormal.y == 0)
            {
                blocked |= 2;       // step / wall
            }

            // Reduce amount of timeL0eft by timeleft * fraction that we covered.
            timeLeft -= timeLeft * trace.fraction;

            // Did we run out of planes to clip against?
            if (numplanes >= MaxClipPlanes)
            {
                //  Stop our movement if so.
                velocity = Vector3.zero;
                break;
            }

            // Set up next clipping plane
            _planes[numplanes] = trace.planeNormal;
            numplanes++;

            // reflect player velocity 
            // Only give this a try for first impact plane because you can get yourself stuck in an acute corner by jumping in place and pressing forward
            if (numplanes == 1)
            {
                for (int i = 0; i < numplanes; i++)
                {
                    if (_planes[i][1] > SurfSlope)
                    {
                        // floor or slope
                        return blocked;
                    }
                    else
                    {
                        ClipVelocity(originalVelocity, _planes[i], ref newVelocity, 1f);
                    }
                }
                velocity = newVelocity;
                originalVelocity = newVelocity;
            }
            else
            {
                int i = 0;
                for (i = 0; i < numplanes; i++)
                {
                    ClipVelocity(originalVelocity, _planes[i], ref velocity, 1);

                    int j = 0;

                    for (j = 0; j < numplanes; j++)
                    {
                        if (j != i)
                        {
                            // Are we now moving against this plane?
                            if (Vector3.Dot(velocity, _planes[j]) < 0)
                                break;
                        }
                    }
                    if (j == numplanes)  // Didn't have to clip, so we're ok
                        break;
                }

                // Did we go all the way through plane set
                if (i != numplanes)
                {   // go along this plane
                    // velocity is set in clipping call, no need to set again.
                    ;
                }
                else
                {   // go along the crease
                    if (numplanes != 2)
                    {
                        velocity = Vector3.zero;
                        break;
                    }
                    var dir = Vector3.Cross(_planes[0], _planes[1]).normalized;
                    d = Vector3.Dot(dir, velocity);
                    velocity = dir * d;
                }

                // if original velocity is against the original velocity, stop dead to avoid tiny occilations in sloping corners
                d = Vector3.Dot(velocity, primalVelocity);
                if (d <= 0f)
                {
                    velocity = Vector3.zero;
                    break;
                }
            }
        }

        if (allFraction == 0f)
        {
            velocity = Vector3.zero;
        }

        return blocked;
    }

    public static void GetCapsulePoints(CapsuleCollider capc, Vector3 origin, out Vector3 p1, out Vector3 p2)
    {
        var distanceToPoints = capc.height / 2f - capc.radius;
        p1 = origin + capc.center + Vector3.up * distanceToPoints;
        p2 = origin + capc.center - Vector3.up * distanceToPoints;
    }
}