using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AceJumpTests
{
    using AceJump;

    using Microsoft.VisualStudio.Text;

    [TestClass]
    public class LetterReferenceDictionaryTests
    {
        [TestMethod]
        public void First_Add_Returns_Key_A()
        {
            LetterReferenceDictionary letterReferenceDictionary = new LetterReferenceDictionary();

            string key = letterReferenceDictionary.AddSpan(new SnapshotSpan());

            Assert.AreEqual("A", key);
        }
    }
}
