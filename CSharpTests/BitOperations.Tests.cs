using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProgrammingInterviewElements.CSharp.Tests
{
    public class BitOperationsTests
    {
        private static readonly Random generator = new Random();

        private static ulong randomUlong()
        {
            var bytes = new byte[sizeof(ulong)];
            generator.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        private static List<ulong> randomUlongs(int quantity)
        {
            quantity = quantity < 1 ? 1 : quantity;
            var result = new List<ulong>(quantity);
            for (int i = 0; i < quantity; ++i)
            {
                result.Add(randomUlong());
            }
            return result;
        }

        private static double randomDouble()
        {
            return generator.NextDouble();
        }

        private static List<double> randomDoubles(int quantity)
        {
            quantity = quantity < 1 ? 1 : quantity;
            var result = new List<double>(quantity);
            for (int i = 0; i < quantity; ++i)
            {
                result.Add(randomDouble());
            }
            return result;
        }

        #region Problem 5.1 Tests

        // Calculate parity of a sequence of numbers

        private static readonly List<ulong> Evens4Bit =
            new List<ulong> {
                0UL, 3UL, 5UL, 6UL, 9UL, 10UL, 12UL, 15UL
            };

        private static readonly List<ulong> Odds4Bit =
           new List<ulong> {
                1UL, 2UL, 4UL, 7UL, 8UL, 11UL, 13UL, 14UL
            };

        [Fact]
        public void EvenParity4BitNumsTest()
        {
            foreach(ulong x in Evens4Bit)
            {
                Assert.True(x.hasEvenParity());
            }
            foreach(ulong x in Odds4Bit)
            {
                Assert.False(x.hasEvenParity());
            }
        }

        [Fact]
        public void OddParity4BitNumsTest()
        {
            foreach(ulong x in Odds4Bit)
            {
                Assert.True(x.hasOddParity());
            }
            foreach(ulong x in Evens4Bit)
            {
                Assert.False(x.hasOddParity());
            }
        }

        [Fact]
        public void EvenParity4BitNumCollectionTest()
        {
            //Should be true no matter how many even parity nums are combined
            Assert.True(Evens4Bit.hasEvenParity());
            //8 total Odd Parity nums combined, so should be even
            Assert.True(Odds4Bit.hasEvenParity());
        }

        [Fact]
        public void OddParity4BitNumCollection()
        {
            Assert.False(Evens4Bit.hasOddParity());
            Assert.False(Odds4Bit.hasOddParity());
        }

        #endregion

        #region Problem 5.2 Tests

        // Swap bits by index

        [Fact]
        public void swapBitsArgOneOverRange()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            Assert.Equal(x.swapBits(64, 0), x);
        }

        [Fact]
        public void swapBitsArgOneUnderRange()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            Assert.Equal(x, x.swapBits(-1, 63));
        }

        [Fact]
        public void swapBitsArgTwoOverRange()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            Assert.Equal(x, x.swapBits(1, 64));
        }

        [Fact]
        public void swapBitsArgTwoUnderRange()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            Assert.Equal(x, x.swapBits(1, -1));
        }

        [Fact]
        public void swapBitsArgsEqual()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            Assert.Equal(x, x.swapBits(0, 0));
        }

        [Fact]
        public void swapBitsIGreaterThanJ()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            ulong expected = (x - 1UL) | (1UL << 63);
            Assert.NotEqual(x.swapBits(63, 0), x);
            Assert.Equal(expected, x.swapBits(63, 0));
        }

        [Fact]
        public void swapBitsILessThanJ()
        {
            ulong x = uint.MaxValue; //0x00000000ffffffff
            ulong expected = (x - 1UL) | (1UL << 63);
            Assert.NotEqual(x.swapBits(0, 63), x);
            Assert.Equal(expected, x.swapBits(0, 63));
        }

        #endregion

        #region Problem 5.3 Tests

        // Reverse bits in a number

        [Fact]
        public void reverseBitsTest()
        {
            var testRuns = 30;
            foreach(ulong x in randomUlongs(testRuns))
            {
                ulong result = x.reverseBits();
                string resultString = Convert.ToString((long)result, 2).PadLeft(64, '0');
                string xString = Convert.ToString((long)x, 2).PadLeft(64, '0');               
                Assert.Equal(xString.Reverse(), resultString);
            }
        }

        #endregion

        #region Problem 5.4 Tests

        // Find nearest number with equal weight (equal number of 1's)

        [Fact]
        public void nearestEqualWeightWorksForZero()
        {
            ulong x = 0UL;
            Assert.Equal(x, x.nearestEqualWeight());
        }

        [Fact]
        public static void nearestEqualWeightWorksForAllOnes()
        {
            ulong x = ulong.MaxValue;
            Assert.Equal(x, x.nearestEqualWeight());
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
            foreach (ulong x in randomUlongs(30))
            {
                Assert.Equal(weight(x), weight(x.nearestEqualWeight()));
            }
        }

        [Fact]
        public static void nearestEqualWeightFindsCloseValue()
        {
            foreach(ulong x in randomUlongs(30))
            {
                ulong result = x.nearestEqualWeight();
                ulong checkedDifference = result > x ? result - x : x - result;
                Assert.True(checkedDifference <= Math.Min(x, result));
            }
        }
        
        // Some specific cases, as don't know how to test in general
        // without repeating solution

        [Fact]
        public static void nearestEqualWeightTo12is10()
        {
            ulong x = 12UL;
            ulong expected = 10UL;
            Assert.Equal(expected, x.nearestEqualWeight());
        }

        [Fact]
        public static void nearestEqualWeightTo8is4()
        {
            ulong x = 8UL;
            ulong expected = 4UL;
            Assert.Equal(expected, x.nearestEqualWeight());
        }

        [Fact]
        public static void nearestEqualWeightToOneZeroAllOnesIsOneOneZeroAllOnes()
        {
            ulong expected = ulong.MaxValue & (1UL << 61);
            ulong x = ulong.MaxValue & (1UL << 62);
            Assert.Equal(expected, x.nearestEqualWeight());
        }

        [Fact]
        public static void nearestEqualWeightToZeroAllOnesIsOneZeroAllOnes()
        {
            ulong expected = ulong.MaxValue & (1UL << 62);
            ulong x = ulong.MaxValue & (1UL << 63);
            Assert.Equal(expected, x.nearestEqualWeight());
        }

        #endregion

        #region Problem 5.5 Tests

        // Multiply 2 unsigned numbers without + or *

        [Fact]
        public static void bitAddAdds()
        {
            var xs = randomUlongs(10);
            var ys = randomUlongs(10);
            foreach(ulong x in xs)
            {
                foreach(ulong y in ys)
                {
                    ulong expected = x + y;
                    Assert.Equal(expected, x.bitAdd(y));
                    Assert.Equal(expected, y.bitAdd(x));
                }
            }
        }

        [Fact]
        public static void bitMultMultiplies()
        {
            var xs = randomUlongs(10);
            var ys = randomUlongs(10);
            foreach (ulong x in xs)
            {
                foreach (ulong y in ys)
                {
                    ulong expected = x * y;
                    Assert.Equal(expected, x.bitMult(y));
                    Assert.Equal(expected, y.bitMult(x));
                }
            }
        }

        #endregion

        #region Problem 5.6 Tests

        // Divide 2 unsigned numbers with +, -, shift

        [Fact]
        public static void bitDivDivides()
        {
            var xs = randomUlongs(10);
            var ys = randomUlongs(10);
            foreach (ulong x in xs)
            {
                foreach(ulong y in ys)
                {
                    if (y == 0UL)
                    {
                        continue;
                    }
                    ulong expected = x / y;
                    Assert.Equal(expected, x.bitDiv(y));
                }
            }
        }

        [Fact]
        public static void bitDivThrowsInvalidOperationExceptionOnDivideByZero()
        {
            var xs = randomUlongs(30);
            foreach (ulong x in xs)
            {
                Assert.Throws<InvalidOperationException>(() => x.bitDiv(0UL));
            }
        }

        #endregion

        #region Problem 5.7 Tests

        // Compute x^y, x double, y int
        // in time linear in the number of bits of y
        // assume primitives are constant time

        [Fact]
        public static void powCalculatesExponents()
        {
            int sample = 30;
            double epsilon = 1e-6;
            var xs = randomDoubles(sample);
            var ys = randomUlongs(sample);
            for (int i = 0; i < sample; ++i)
            {
                double x = xs[i];
                ulong y = ys[i];
                double expected = Math.Pow(x, (double)y);
                double actual = x.pow(y);

                Assert.True(Math.Abs(expected - actual) < epsilon);
            }
        }

        [Fact]
        public static void powExponentZeroIsOne()
        {
            int sample = 10;
            double epsilon = 1e-6;
            double expected = 1.0;
            var xs = randomDoubles(sample);
            foreach (double x in xs)
            {
                double actual = x.pow(0UL);
                Assert.True(Math.Abs(expected - actual) < epsilon);
            }            
        }

        #endregion
    }
}
