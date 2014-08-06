using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AceJumpTests
{
    using System.Collections.Generic;

    using AceJump;

    using Microsoft.VisualStudio.Text;

    using Moq;

    [TestClass]
    public class LetterReferenceDictionaryTests
    {
        private LetterReferenceDictionary letterReferenceDictionary;

        private Mock<ITextSnapshot> textsnapshotMock;

        [TestInitialize]
        public void BeforeEachTest()
        {
            this.letterReferenceDictionary = new LetterReferenceDictionary();
            this.textsnapshotMock = new Mock<ITextSnapshot>();
        }

        [TestMethod]
        public void if_found_locations_all_less_than_cursor_location_A_through_B()
        {
            List<int> foundKeyLocations = new List<int>();
            foundKeyLocations.Add(1);
            foundKeyLocations.Add(2);

            int cursorlocation = 3;
            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations, cursorlocation);

            Assert.AreEqual(2, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(1, dictionary.GetLetterPosition("B"));
        }

        [TestMethod]
        public void if_found_locations_all_more_than_cursor_location_A_through_B()
        {
            List<int> foundKeyLocations = new List<int>();
            foundKeyLocations.Add(4);
            foundKeyLocations.Add(5);

            int cursorlocation = 3;
            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations, cursorlocation);

            Assert.AreEqual(4, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(5, dictionary.GetLetterPosition("B"));
        }

        [TestMethod]
        public void if_found_Locations_span_alternate_A_B_From_curso_Location()
        {
            List<int> foundKeyLocations = new List<int>();
            foundKeyLocations.Add(1);
            foundKeyLocations.Add(2);
            foundKeyLocations.Add(4);
            foundKeyLocations.Add(5);

            int cursorlocation = 3;
            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations, cursorlocation);

            Assert.AreEqual(2, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(4, dictionary.GetLetterPosition("B"));
            Assert.AreEqual(1, dictionary.GetLetterPosition("C"));
            Assert.AreEqual(5, dictionary.GetLetterPosition("D"));
        }

        [TestMethod]
        public void First_Add_Returns_Key_A()
        {
            string key = letterReferenceDictionary.AddSpan(1);

            Assert.AreEqual("A", key);
        }

        [TestMethod]
        public void Third_Add_Returns_Key_C()
        {
            letterReferenceDictionary.AddSpan(1);
            letterReferenceDictionary.AddSpan(2);
            string key = letterReferenceDictionary.AddSpan(2);

            Assert.AreEqual("C", key);
        }

        [TestMethod]
        public void Add_Adds_Item_To_Dictionary()
        {
            letterReferenceDictionary.AddSpan(4);

            Assert.AreEqual(1, letterReferenceDictionary.Count);
        }

        [TestMethod]
        public void When_Two_Items_Added_Count_Is_two()
        {
            letterReferenceDictionary.AddSpan(4);
            letterReferenceDictionary.AddSpan(4);

             Assert.AreEqual(2, letterReferenceDictionary.Count);
        }

        [TestMethod]
        public void When_Z_Is_Reached_Key_Goes_To_XA()
        {
            // 26 letters in the english alphabet
            for (int i = 1; i <= 26; i++)
            {
                letterReferenceDictionary.AddSpan(4);
            }
            
            string key = letterReferenceDictionary.AddSpan(4);
            Assert.AreEqual("XA", key);
        }


        [TestMethod]
        public void When_ZZ_Is_Reached_Key_Goes_To_YA()
        {
            // 26 letters in the english alphabet
            for (int i = 1; i <= 26*2; i++)
            {
                letterReferenceDictionary.AddSpan(4);
            }

            string key = letterReferenceDictionary.AddSpan(4);
            Assert.AreEqual("YA", key);
        }
    }
}
