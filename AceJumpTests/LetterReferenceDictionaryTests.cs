using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AceJumpTests
{
    using AceJump;

    using Microsoft.VisualStudio.Text;

    [TestClass]
    public class LetterReferenceDictionaryTests
    {
        private LetterReferenceDictionary letterReferenceDictionary;

        [TestInitialize]
        public void BeforeEachTest()
        {
            this.letterReferenceDictionary = new LetterReferenceDictionary();
        }

        [TestMethod]
        public void First_Add_Returns_Key_A()
        {
            string key = letterReferenceDictionary.AddSpan(new SnapshotSpan());

            Assert.AreEqual("A", key);
        }

        [TestMethod]
        public void Third_Add_Returns_Key_C()
        {
            letterReferenceDictionary.AddSpan(new SnapshotSpan());
            letterReferenceDictionary.AddSpan(new SnapshotSpan());
            string key = letterReferenceDictionary.AddSpan(new SnapshotSpan());

            Assert.AreEqual("C", key);
        }

        [TestMethod]
        public void Add_Adds_Item_To_Dictionary()
        {
            letterReferenceDictionary.AddSpan(new SnapshotSpan());

            Assert.AreEqual(1, letterReferenceDictionary.Count);
        }

        [TestMethod]
        public void When_Two_Items_Added_Count_Is_two()
        {
            letterReferenceDictionary.AddSpan(new SnapshotSpan());
            letterReferenceDictionary.AddSpan(new SnapshotSpan());

             Assert.AreEqual(2, letterReferenceDictionary.Count);
        }

        [TestMethod]
        public void When_Z_Is_Reached_Key_Goes_To_AA()
        {
            // 26 letters in the english alphabet
            for (int i = 1; i <= 26; i++)
            {
                letterReferenceDictionary.AddSpan(new SnapshotSpan());
            }
            
            string key = letterReferenceDictionary.AddSpan(new SnapshotSpan());
            Assert.AreEqual("AA", key);
        }


        [TestMethod]
        public void When_ZZ_Is_Reached_Key_Goes_To_BA()
        {
            // 26 letters in the english alphabet
            for (int i = 1; i <= 26*2; i++)
            {
                letterReferenceDictionary.AddSpan(new SnapshotSpan());
            }

            string key = letterReferenceDictionary.AddSpan(new SnapshotSpan());
            Assert.AreEqual("BA", key);
        }
    }
}
