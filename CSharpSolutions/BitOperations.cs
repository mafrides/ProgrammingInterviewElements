﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace ProgrammingInterviewElements.CSharp
{
    public static class BitOperations
    {
        #region Problem 5.1 Solution

        // Calculate parity of a sequence of numbers

        public static bool hasEvenParity(this ulong x)
        {
            for (int offset = 1; offset < 64; offset *= 2)
            {
                x ^= (x << offset);
            }
            return 0 == (x & (1UL << 63));
        }

        public static bool hasOddParity(this ulong x)
        {
            return !hasEvenParity(x);
        }

        public static bool hasEvenParity(this IEnumerable<ulong> xs)
        {
            ulong acc = 0UL;
            foreach(ulong x in xs)
            {
                acc ^= x;
            }
            return hasEvenParity(acc);
        }

        public static bool hasOddParity(this IEnumerable<ulong> xs)
        {
            return !hasEvenParity(xs);
        }

        #endregion

        #region Problem 5.2 Solution

        // Swap bits by index

        public static ulong swapBits(this ulong x, int i, int j)
        {
            if (i < 0  || 
                i > 63 ||
                j < 0  || 
                j > 63 ||
                i == j)
            {
                return x;
            }
            if (i < j)
            {
                return x.swapBits(j, i);
            }
            int delta = i - j;
            ulong clearMask = ~((1UL << i) | (1UL << j));
            ulong setMask = ((x >> delta) & (1UL << j)) |
                            ((x << delta) & (1UL << i));
            return (x & clearMask) | setMask;
        }

        #endregion

        #region Problem 5.3 Solution

        // Reverse bits in a number

        private const ulong Low1  = 0x5555555555555555;
        private const ulong Low2 = 0x3333333333333333;
        private const ulong Low4 = 0x0f0f0f0f0f0f0f0f;
        private const ulong Low8 = 0x00ff00ff00ff00ff;
        private const ulong Low16 = 0x0000ffff0000ffff;
        private const ulong Low32 = 0x00000000ffffffff;

        public static ulong reverseBits(this ulong x)
        {
            x = ((x >> 32) & Low32) | ((x << 32) & (Low32 << 32));
            x = ((x >> 16) & Low16) | ((x << 16) & (Low16 << 16));
            x = ((x >> 8) & Low8) | ((x << 8) & (Low8 << 8));
            x = ((x >> 4) & Low4) | ((x << 4) & (Low4 << 4));
            x = ((x >> 2) & Low2) | ((x << 2) & (Low2 << 2));
            x = ((x >> 1) & Low1) | ((x << 1) & (Low1 << 1));
            return x;
        }

        #endregion

        #region Problem 5.4 Solution

        // Find nearest number with equal weight (equal number of 1's)

        public static ulong nearestEqualWeight(this ulong x)
        {
            if (x == 0UL || x == ulong.MaxValue)
            {
                return x;
            }
            
            ulong lastBit = x & 1UL;
            for (int i = 1; i < 64; ++i)
            {
                ulong currentBit = (x & (1UL << i)) == 0UL ? 0UL : 1UL;
                if (currentBit != lastBit)
                {
                    return swapBits(x, i, i - 1);
                }
                lastBit = currentBit;
            }

            //can never get here
            throw new InvalidOperationException("Problem 5.4 Solution is very broken");
        }

        #endregion

        #region Problem 5.5 Solution

        // Multiply 2 unsigned numbers without + or *

        //unchecked
        public static ulong bitAdd(this ulong x, ulong y)
        {
            ulong sum = x ^ y;
            ulong carry = (x & y) << 1;
            return carry == 0UL ? sum : sum.bitAdd(carry);
        }

        //unchecked
        public static ulong bitMult(this ulong x, ulong y)
        {
            ulong result = 0UL;
            for (int i = 0; i < 64; ++i)
            {
                if ((y & (1UL << i)) != 0UL)
                {
                    result = result.bitAdd(x << i);
                }
            }
            return result;
        }

        #endregion

        #region Problem 5.6 Solution

        // Divide 2 unsigned numbers with +, -, shift

        // Returns number of left shifts
        private static int initialAlign(ref ulong divisor, ulong with)
        {
            int leftShifts = 0;
            for (ulong next = divisor << 1; next > divisor && next <= with; ++leftShifts)
            {
                divisor = next;
                next = divisor << 1;
            }
            return leftShifts;
        }

        // Returns remain shifts or -1 if cannot align
        private static int rightAlign(ref ulong divisor, ulong with, int maxShifts)
        {
            for (ulong next = divisor >> 1; maxShifts > 0 && divisor > with; --maxShifts)
            {
                divisor = next;
                next = divisor >> 1;
            }
            return divisor > with ? -1 : maxShifts;
        }

        // Throw Invalid Operation Exception
        public static ulong bitDiv(this ulong x, ulong y)
        {
            if (y == 0UL)
            {
                throw new InvalidOperationException("Divide by Zero");
            }
            
            //truncate
            if (y > x)
            {
                return 0UL;
            }

            ulong result = 0UL;
            int shifts = initialAlign(ref y, with: x);
            do
            {
                result += (1UL << shifts);
                x -= y;
                shifts = rightAlign(ref y, with: x, maxShifts: shifts);
            } while (shifts > -1);

            return result;
        }

        #endregion

        #region Problem 5.7 Solution

        // Compute x^y, x double, y int
        // in time linear in the number of bits of y
        // assume primitives are constant time

        //constant time for the purposes of this problem
        private static bool bitIsOne(this ulong x, int i)
        {
            if (i < 0 || i > 63)
            {
                return false;
            }

            return (x & (1UL << i)) != 0UL;
        }

        public static double pow(this double x, ulong y)
        {
            double result = 1.0;
            
            if (y == 0UL)
            {
                return result;
            }
           
            for (int i = 0; i < 64; ++i)
            {
                if (y.bitIsOne(i))
                {
                    result *= x;
                }
                x *= x;
            }

            return result;
        }

        #endregion
    }
}
