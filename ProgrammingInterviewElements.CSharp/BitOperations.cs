using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace ProgrammingInterviewElements.CSharp
{
    public static class BitOperations
    {
        //I thought this was cool: from ch. 5
        public static void swap(ref int a, ref int b)
        {
            a ^= b ^= a ^= b;
        }

        #region Problem 5.1 Solution

        /* 
         * Saw a version on wikipedia using >>
         * << should avoid sign extension issue
         * and work for negative numbers
         * What about little v. big endian?
         */
        public static bool hasEvenParity(ulong num)
        {
            for (int offset = 1; offset < 64; offset *= 2)
            {
                num ^= (num << offset);
            }
            return 0 == (num & (1UL << 63));
        }

        public static bool hasOddParity(ulong num)
        {
            return !hasEvenParity(num);
        }

        public static bool hasEvenParity(IEnumerable<ulong> nums)
        {
            return hasEvenParity(nums.Fold(0UL, (val, acc) => val ^ acc));
        }

        public static bool hasOddParity(IEnumerable<ulong> nums)
        {
            return !hasEvenParity(nums);
        }

        #endregion

        #region Problem 5.2 Solution

        //Exchanges bits at index i and j of x, index 0 at right
        public static ulong exchangeBits(ulong x, int i, int j)
        {
            if (i > 63 || j > 63 || i == j) return x;
            if (i < j) return exchangeBits(x, j, i);
            int delta = i - j;
            ulong clearMask = ~((1UL << i) | (1UL << j));
            ulong setMask = ((x >> delta) & (1UL << j)) |
                            ((x << delta) & (1UL << i));
            return (x & clearMask) | setMask;
        }

        #endregion

        #region Problem 5.3 Solution

        public const ulong Low1  = 0x5555555555555555;
        public const ulong Low2  = 0x3333333333333333;
        public const ulong Low4  = 0x0f0f0f0f0f0f0f0f;
        public const ulong Low8  = 0x00ff00ff00ff00ff;
        public const ulong Low16 = 0x0000ffff0000ffff;
        public const ulong Low32 = 0x00000000ffffffff;

        public static ulong reverseBits(ulong x)
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

        public static ulong nearestEqualWeight(ulong x)
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
                    return exchangeBits(x, i, i - 1);
                }
                lastBit = currentBit;
            }

            return 0UL; //can never get here
        }

        #endregion
    }
}
