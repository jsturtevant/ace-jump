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

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }

            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual(2, letterDictionary.GetLetterPosition("B"));
            Assert.AreEqual(26, letterDictionary.GetLetterPosition("Z"));
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

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }

            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual(25, letterDictionary.GetLetterPosition("ZA"));
            Assert.AreEqual(26, letterDictionary.GetLetterPosition("ZB"));
            Assert.AreEqual(27, letterDictionary.GetLetterPosition("ZC"));
        }

        [TestMethod]
        public void if_found_location_52_then_ZZ()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(52);

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }

            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual(24, letterDictionary.GetLetterPosition("X"));
            Assert.AreEqual(25, letterDictionary.GetLetterPosition("ZA")); 
            Assert.AreEqual(26, letterDictionary.GetLetterPosition("ZB"));
            Assert.AreEqual(27, letterDictionary.GetLetterPosition("ZC"));
            Assert.AreEqual(50, letterDictionary.GetLetterPosition("ZZ"));
            Assert.AreEqual(51, letterDictionary.GetLetterPosition("YA"));
            Assert.AreEqual(52, letterDictionary.GetLetterPosition("YB"));
            Assert.AreEqual("YB", letterDictionary.LastKey);

        }

        [TestMethod]
        public void if_found_location_53_then_YB()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(53);

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }

            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual(24, letterDictionary.GetLetterPosition("ZA")); 
            Assert.AreEqual(25, letterDictionary.GetLetterPosition("ZB"));
            Assert.AreEqual(26, letterDictionary.GetLetterPosition("ZC"));
            Assert.AreEqual(50, letterDictionary.GetLetterPosition("YA"));
            Assert.AreEqual(51, letterDictionary.GetLetterPosition("YB"));
            Assert.AreEqual(52, letterDictionary.GetLetterPosition("YC"));
            Assert.AreEqual(53, letterDictionary.GetLetterPosition("YD"));
            Assert.AreEqual("YD", letterDictionary.LastKey);
        }

        [TestMethod]
        public void if_found_location_78_then_X()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(26*3+1);

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }
            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual(23, letterDictionary.GetLetterPosition("ZA"));  // would have been x
            Assert.AreEqual(24, letterDictionary.GetLetterPosition("ZB"));  
            Assert.AreEqual(51, letterDictionary.GetLetterPosition("YC"));
            Assert.AreEqual(75, letterDictionary.GetLetterPosition("XA"));
            Assert.AreEqual(76, letterDictionary.GetLetterPosition("XB"));
        }

        [TestMethod]
        public void if_found_location_IsGreater_than_offsetKeyof_B_Then_Limit_dictionary()
        {
            // one more than z
            List<int> foundKeyLocations = CreateLocations(26 * 26 + 1);

            var letterDictionary = new LetterReferenceDictionary(foundKeyLocations.Count);

            foreach (var keyLocation in foundKeyLocations)
            {
                letterDictionary.AddSpan(keyLocation);
            }
            Assert.AreEqual(1, letterDictionary.GetLetterPosition("A"));
            Assert.AreEqual('B',letterDictionary.OffsetKey);
            Assert.AreEqual("BZ", letterDictionary.LastKey);
        }

    }
}
