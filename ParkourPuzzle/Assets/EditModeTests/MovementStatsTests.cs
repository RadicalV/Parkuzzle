using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementStatsTests
{
    [Test]
    public void MaxVelocity()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(50f, playerLocomotion.maxVelocity);
    }

    [Test]
    public void MaxSpeed()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(6f, playerLocomotion.maxSpeed);
    }

    [Test]
    public void Friction()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(6f, playerLocomotion.friction);
    }

    [Test]
    public void Gravity()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(20f, playerLocomotion.gravity);
    }

    [Test]
    public void Acceleration()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(14f, playerLocomotion.acceleration);
    }

    [Test]
    public void StopSpeed()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(1.905f, playerLocomotion.stopSpeed);
    }

    [Test]
    public void AutoBhop()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(true, playerLocomotion.autoBhop);
    }

    [Test]
    public void JumpForce()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(6.5f, playerLocomotion.jumpForce);
    }

    [Test]
    public void AirAcceleration()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(12f, playerLocomotion.airAcceleration);
    }

    [Test]
    public void AirCap()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(0.4f, playerLocomotion.airCap);
    }

    [Test]
    public void AirFriction()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(0.25f, playerLocomotion.airFriction);
    }

    [Test]
    public void CrouchSpeed()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(2.108f, playerLocomotion.crouchSpeed);
    }

    [Test]
    public void CrouchAcceleration()
    {
        PlayerLocomotion playerLocomotion = new PlayerLocomotion();
        Assert.AreEqual(8f, playerLocomotion.crouchAcceleration);
    }
}