using System.Collections;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Tests.Datas;
using App.Scripts.Tests.Math;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CorrectAngleTest
{
    private IAngleCorrector _angleCorrector;
    
    public void SetUp(float minAngle)
    {
        BallFlyingSettings settings = new();
        settings.MinAngle = minAngle;
        
        _angleCorrector = new AngleCorrector(settings);
    }
    
    [Test]
    [TestCase(0, 25, 25)]
    [TestCase(10, 25, 25)]
    [TestCase(20, 25, 25)]
    [TestCase(30, 30, 25)]
    [TestCase(40, 40, 25)]
    [TestCase(50, 50, 25)]
    [TestCase(60, 60, 25)]
    [TestCase(70, 65, 25)]
    [TestCase(80, 65, 25)]
    [TestCase(90, 115, 25)]
    [TestCase(100, 115, 25)]
    [TestCase(110, 115, 25)]
    [TestCase(120, 120, 25)]
    [TestCase(130, 130, 25)]
    [TestCase(140, 140, 25)]
    [TestCase(150, 150, 25)]
    [TestCase(160, 155, 25)]
    [TestCase(170, 155, 25)]
    [TestCase(180, 205, 25)]
    [TestCase(190, 205, 25)]
    [TestCase(200, 205, 25)]
    [TestCase(210, -150, 25)]
    [TestCase(220, -140, 25)]
    [TestCase(230, -130, 25)]
    [TestCase(240, -120, 25)]
    [TestCase(250, -115, 25)]
    [TestCase(260, -115, 25)]
    [TestCase(270, -65, 25)]
    [TestCase(280, -65, 25)]
    [TestCase(290, -65, 25)]
    [TestCase(300, -60, 25)]
    [TestCase(310, -50, 25)]
    [TestCase(320, -40, 25)]
    [TestCase(330, -30, 25)]
    [TestCase(340, -25, 25)]
    [TestCase(350, -25, 25)]
    [TestCase(360, 25, 25)]
    public void CorrectAngleTestSimplePasses(float initialAngle, float targetAngle, float minAngle)
    {
        SetUp(minAngle);
        
        float answerAngle = GetAnswer(initialAngle);

        Assert.AreEqual(targetAngle, answerAngle, 0.01f);
    }

    private float GetAnswer(float initialAngle)
    {
        Vector2 direction = MathService.GetDirectionByAngle(initialAngle);
        float answerAngle = _angleCorrector.CorrectAngleByDirection(direction);
        return answerAngle;
    }
}
