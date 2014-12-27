using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProgrammingInterviewElements.CSharp.Tests
{
    public class BitOperationsTests
    {
        public static readonly List<ulong> Evens4Bit =
            new List<ulong> {
                0UL, 3UL, 5UL, 6UL, 9UL, 10UL, 12UL, 15UL
            };

        public static readonly List<ulong> Odds4Bit =
           new List<ulong> {
                1UL, 2UL, 4UL, 7UL, 8UL, 11UL, 13UL, 14UL
            };


        #region Problem 5.1 Tests

        [Fact]
        public void EvenParity4BitNumsTest()
        {
            Evens4Bit.ForEach(num =>
                Assert.True(BitOperations.hasEvenParity(num)));
            Odds4Bit.ForEach(num =>
                Assert.False(BitOperations.hasEvenParity(num)));
        }

        [Fact]
        public void OddParity4BitNumsTest()
        {
            Odds4Bit.ForEach(num =>
                Assert.True(BitOperations.hasOddParity(num)));
            Evens4Bit.ForEach(num =>
                Assert.False(BitOperations.hasOddParity(num)));
        }

        [Fact]
        public void EvenParity4BitNumCollectionTest()
        {
            //Should be true no matter how many even parity nums are combined
            Assert.True(BitOperations.hasEvenParity(Evens4Bit));
            //8 total Odd Parity nums combined, so should be even
            Assert.True(BitOperations.hasEvenParity(Odds4Bit));
        }

        [Fact]
        public void OddParity4BitNumCollection()
        {
            Assert.False(BitOperations.hasOddParity(Evens4Bit));
            Assert.False(BitOperations.hasOddParity(Odds4Bit));
        }

        #endregion

        #region Problem 5.2 Tests

        [Fact]
        public void exchangeBitsIndexRangeTest()
        {
            ulong x = uint.MaxValue; //all ones in lower half
            ulong y = x;
            Assert.Equal(BitOperations.exchangeBits(x, 64, 0), y);
            Assert.Equal(BitOperations.exchangeBits(x, 0, 64), y);
            Assert.NotEqual(BitOperations.exchangeBits(x, 0, 63), y);
        }

        [Fact]
        public void exchangeBitsIGreaterThanJ()
        {
            ulong x = uint.MaxValue;
            ulong y = x;
            ulong result = (y - 1UL) | (1UL << 63);
            Assert.NotEqual(BitOperations.exchangeBits(x, 63, 0), y);
            Assert.Equal(BitOperations.exchangeBits(x, 63, 0), result);
        }

        [Fact]
        public void exchangeBitsILessThanJ()
        {
            ulong x = uint.MaxValue;
            ulong y = x;
            ulong result = (y - 1UL) | (1UL << 63);
            Assert.NotEqual(BitOperations.exchangeBits(x, 0, 63), y);
            Assert.Equal(BitOperations.exchangeBits(x, 0, 63), result);
        }

        [Fact]
        public void exchangeBitsIEqualsJ()
        {
            ulong x = uint.MaxValue;
            ulong y = x;
            Assert.Equal(BitOperations.exchangeBits(x, 0, 0), y);
        }

        #endregion
    }
}
