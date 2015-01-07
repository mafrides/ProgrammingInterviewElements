using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProgrammingInterviewElements.CSharp.Tests
{
    public class BitOperationsTests
    {
        public static ulong randomUlong(Random generator)
        {
            var bytes = new byte[sizeof(ulong)];
            generator.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static List<ulong> randomUlongs(int quantity)
        {
            quantity = quantity < 1 ? 1 : quantity;
            var result = new List<ulong>(quantity);
            Random generator = new Random();
            for(int i = 0; i < quantity; ++i)
            {
                result.Add(randomUlong(generator));
            }
            return result;
        }

        #region Problem 5.1 Tests

        public static readonly List<ulong> Evens4Bit =
            new List<ulong> {
                0UL, 3UL, 5UL, 6UL, 9UL, 10UL, 12UL, 15UL
            };

        public static readonly List<ulong> Odds4Bit =
           new List<ulong> {
                1UL, 2UL, 4UL, 7UL, 8UL, 11UL, 13UL, 14UL
            };

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

        #region Problem 5.3 Tests

        [Fact]
        public void reverseBitsTest()
        {
            var testRuns = 30;
            foreach(ulong testValue in randomUlongs(testRuns))
            {
                ulong result = BitOperations.reverseBits(testValue);
                string resultString = Convert.ToString((long)result, 2).PadLeft(64, '0');
                string testValueString = Convert.ToString((long)testValue, 2).PadLeft(64, '0');               
                Assert.Equal(testValueString.Reverse(), resultString);
            }
        }

        #endregion

        #region Problem 5.4 Tests

        [Fact]
        public void nearestEqualWeightWorksForZero()
        {
            Assert.Equal(0UL, BitOperations.nearestEqualWeight(0UL));
        }

        [Fact]
        public static void nearestEqualWeightWorksForAllOnes()
        {
            Assert.Equal(ulong.MaxValue, BitOperations.nearestEqualWeight(ulong.MaxValue));
        }

        private static ulong weight(ulong x)
        {
            ulong result = 0;
            for(int i = 0; i < 64; ++i)
            {
                result += x & 1UL;
                x >>= 1;
            }
            return result;
        }

        [Fact]
        public static void nearestEqualWeightFindsEqualWeightAnswer()
        {
            foreach(ulong i in randomUlongs(30))
            {
                Assert.Equal(weight(i), weight(BitOperations.nearestEqualWeight(i)));
            }
        }

        [Fact]
        public static void nearestEqualWeightFindsCloseValue()
        {
            foreach(ulong i in randomUlongs(30))
            {
                ulong test = BitOperations.nearestEqualWeight(i);
                ulong checkedDifference = test > i ? test - i : i - test;
                Assert.True(checkedDifference <= Math.Min(i, test));
            }
        }
        
        // Some specific cases, as don't know how to test in general
        // without repeating solution

        [Fact]
        public static void nearestEqualWeightTo12is10()
        {
            Assert.Equal(10UL, BitOperations.nearestEqualWeight(12UL));
        }

        [Fact]
        public static void nearestEqualWeightTo8is4()
        {
            Assert.Equal(4UL, BitOperations.nearestEqualWeight(8UL));
        }

        [Fact]
        public static void nearestEqualWeightToOneZeroAllOnesIsOneOneZeroAllOnes()
        {
            ulong expected = ulong.MaxValue & (1UL << 61);
            ulong testVal = ulong.MaxValue & (1UL << 62);
            Assert.Equal(expected, BitOperations.nearestEqualWeight(testVal));
        }

        [Fact]
        public static void nearestEqualWeightToZeroAllOnesIsOneZeroAllOnes()
        {
            ulong expected = ulong.MaxValue & (1UL << 62);
            ulong testVal = ulong.MaxValue & (1UL << 63);
            Assert.Equal(expected, BitOperations.nearestEqualWeight(testVal));
        }

        #endregion
    }
}
