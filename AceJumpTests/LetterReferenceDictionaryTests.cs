using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AceJumpTests
{
    using System.Collections.Generic;

    using AceJump;

    [TestClass]
    public class LetterReferenceDictionaryTests
    {
      [TestMethod]
        public void if_found_locations_than_Z_then_A_Z()
        {
            //26 is a-z
            List<int> foundKeyLocations = CreateLocations(26);

            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations);

            Assert.AreEqual(1, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(2, dictionary.GetLetterPosition("B"));
            Assert.AreEqual(26, dictionary.GetLetterPosition("Z"));
        }

        private static List<int> CreateLocations(int numberOflocations)
        {
            List<int> foundKeyLocations = new List<int>();
            for (int i = 1; i <= numberOflocations; i++)
            {
                foundKeyLocations.Add(i);
            }

            return foundKeyLocations;
        }

        [TestMethod]
        public void if_found_locations_greater_than_z_then_ZA()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(27);

            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations);

            Assert.AreEqual(1, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(25, dictionary.GetLetterPosition("Y"));
            Assert.AreEqual(26, dictionary.GetLetterPosition("ZA"));
            Assert.AreEqual(27, dictionary.GetLetterPosition("ZB"));
        }

        [TestMethod]
        public void if_found_location_52_then_ZZ()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(52);

            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations);

            Assert.AreEqual(1, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(25, dictionary.GetLetterPosition("Y"));
            Assert.AreEqual(26, dictionary.GetLetterPosition("ZA")); //would have been z
            Assert.AreEqual(27, dictionary.GetLetterPosition("ZB"));
            Assert.AreEqual(28, dictionary.GetLetterPosition("ZC"));
            Assert.AreEqual(51, dictionary.GetLetterPosition("ZZ"));
            Assert.AreEqual(52, dictionary.GetLetterPosition("YA"));
        }

        [TestMethod]
        public void if_found_location_53_then_YB()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(53);

            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations);

            Assert.AreEqual(1, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(24, dictionary.GetLetterPosition("X"));
            Assert.AreEqual(25, dictionary.GetLetterPosition("ZA"));  //would have been y
            Assert.AreEqual(26, dictionary.GetLetterPosition("ZB"));
            Assert.AreEqual(27, dictionary.GetLetterPosition("ZC"));
            Assert.AreEqual(28, dictionary.GetLetterPosition("ZD"));
            Assert.AreEqual(50, dictionary.GetLetterPosition("ZZ"));
            Assert.AreEqual(51, dictionary.GetLetterPosition("YA"));
            Assert.AreEqual(52, dictionary.GetLetterPosition("YB"));
            Assert.AreEqual(53, dictionary.GetLetterPosition("YC"));
        }

        [TestMethod]
        public void if_found_location_78_then_X()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(26*3+1);

            LetterReferenceDictionary dictionary = LetterReferenceDictionary.CreateJumps(foundKeyLocations);

            Assert.AreEqual(1, dictionary.GetLetterPosition("A"));
            Assert.AreEqual(23, dictionary.GetLetterPosition("W"));
            Assert.AreEqual(24, dictionary.GetLetterPosition("ZA"));  // would have been x
            Assert.AreEqual(25, dictionary.GetLetterPosition("ZB"));  
            Assert.AreEqual(52, dictionary.GetLetterPosition("YC"));
            Assert.AreEqual(76, dictionary.GetLetterPosition("XA"));
        }

    }
}
