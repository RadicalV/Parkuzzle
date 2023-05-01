using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TracerTests
{
    [Test]
    public void CreateTrace()
    {
        Vector3 startPos = new Vector3(0, 0, 0);
        Vector3 endPos = new Vector3(1, 1, 1);
        Vector3 zeroVector = Vector3.zero;
        float fraction = 0.2f;
        Collider collider = new Collider();

        Trace trace = new Trace()
        {
            startPos = startPos,
            endPos = endPos,
            fraction = fraction,
            hitCollider = collider,
            hitPoint = zeroVector,
            planeNormal = zeroVector
        };

        Assert.AreEqual(startPos, trace.startPos);
        Assert.AreEqual(endPos, trace.endPos);
        Assert.AreEqual(fraction, trace.fraction);
        Assert.AreEqual(collider, trace.hitCollider);
        Assert.AreEqual(zeroVector, trace.hitPoint);
        Assert.AreEqual(zeroVector, trace.planeNormal);
    }
}
