using App.Scripts.Tests.Datas;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Tests.Math
{
    public class MathTests
    {
        private const string BasePathToConfig = "Assets/App/Scripts/Tests/Math/Configs/{0}.txt";
        
        [Test]
        [TestCase("angleToVector_0")]
        [TestCase("angleToVector_1")]
        [TestCase("angleToVector_2")]
        [TestCase("angleToVector_3")]
        [TestCase("angleToVector_4")]
        [TestCase("angleToVector_5")]
        [TestCase("angleToVector_6")]
        [TestCase("angleToVector_7")]
        public void AngleToVectorTest(string fileName)
        {
            string pathToConfig = string.Format(BasePathToConfig, fileName);
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToConfig);

            AngleToVectorTestData data = JsonConvert.DeserializeObject<AngleToVectorTestData>(textAsset.text);

            Vector2 gettedAnswer = MathService.GetDirectionByAngle(data.Angle);
            Vector vector = new Vector(gettedAnswer.x, gettedAnswer.y);
            
            Assert.AreEqual(data.Vector.X, vector.X, 0.01f);
            Assert.AreEqual(data.Vector.Y, vector.Y, 0.01f);
        }
    }
}