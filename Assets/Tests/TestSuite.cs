using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
namespace Tests
{
    
    public class TestSuite
    {
        [SetUp]
        public void ResetScene() {
            SceneManager.LoadScene("DemoScene");
            
        }

        /*[UnityTest]
        public IEnumerator CheckGeneratedCardCount()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return 10;
            Assert.True(CardManager.instance.generatedCardCount == 3);
        }

        [UnityTest]
        public IEnumerator placeHolder() {
            yield return 10;
            CardManager.instance.ResetGame();
            Assert.True(CardManager.instance.generatedCardCount == 3);
        }*/

        /*[UnityTest]
        public IEnumerator placeHolder2() {
            yield return 10;
            Assert.Fail();
        }*/

        [Test]
        public void test() {
            Assert.Fail();
        }
    }
}
