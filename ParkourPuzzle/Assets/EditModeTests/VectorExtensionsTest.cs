using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VectorExtensionsTest
{
    [Test]
    public void CreateTrace()
    {
        Vector3 start = new Vector3(0f, 0f, 0f);
        Vector3 direction = new Vector3(1f, 1f, 1f);
        float scale = 2f;

        Vector3 expectedResult = new Vector3(start.x + direction.x * scale, start.y + direction.y * scale, start.z + direction.z * scale);

        Vector3 result = VectorExtensions.VectorMa(start, scale, direction);

        Assert.AreEqual(expectedResult, result);
    }
}
