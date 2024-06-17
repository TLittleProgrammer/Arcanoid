using App.Scripts.Scenes.GameScene.Features.Healthes;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace App.Scripts.Tests.Healthes
{
    public class ChangeHealthesTest
    {
        private IHealthContainer _healthContainer;
        
        [SetUp]
        public void SetUp()
        {
            IViewHealthPointService viewHealthPointService = new TestViewHealthPointService();
            _healthContainer = new HealthContainer(viewHealthPointService);
        }

        [Test]
        [TestCase(3, -1, 2)]
        [TestCase(3, -2, 1)]
        [TestCase(3, -3, 0)]
        [TestCase(3, -4, -1)]
        [TestCase(3, -5, -1)]
        [TestCase(3, -999, -1)]
        [TestCase(3, 999, 3)]
        [TestCase(5, 9, 5)]
        [TestCase(12, 1, 12)]
        public void ChangeHealthTest(int initialHealthes, int addHealth, int needResult)
        {
            _healthContainer.AsyncInitialize(initialHealthes).Forget();
            
            _healthContainer.UpdateHealth(addHealth);

            Assert.AreEqual(needResult, _healthContainer.CurrentHealthPoints);
        }
    }
}