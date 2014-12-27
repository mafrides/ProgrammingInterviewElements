using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgrammingInterviewElements.CSharp
{
    public static class BitOperations
    {
        //I thought this was cool: from ch. 5
        public static void swap(ref int a, ref int b)
        {
            a ^= b ^= a ^= b;
        }

        #region Problem 5.1 Methods

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

        #region Problem 5.2 Methods

        //Exchanges bits at index i and j of x, index 0 at right
        public static ulong exchangeBits(ulong x, uint i, uint j)
        {
            if (i > 63 || j > 63 || i == j) return x;
            if (i < j) return exchangeBits(x, j, i);
            int ii = (int)i;
            int jj = (int)j;
            int delta = ii - jj;
            ulong clearMask = ~((1UL << ii) | (1UL << jj));
            ulong setMask = ((x >> delta) & (1UL << jj)) |
                            ((x << delta) & (1UL << ii));
            return (x & clearMask) | setMask;
        }

        #endregion
    }
}
