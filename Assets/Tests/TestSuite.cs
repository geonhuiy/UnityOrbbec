using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace Tests
{
    
    public class TestSuite
    {
        private HandManager handManager;
        private CardManager cardManager;
        [UnityTest]
        public IEnumerator GenerateCards() {
            CardManager.instance.AssignSprites();
            yield return CardManager.instance.currentCardName.Count;
            Assert.True(CardManager.instance.currentCardName != null);
        }
    }
}
