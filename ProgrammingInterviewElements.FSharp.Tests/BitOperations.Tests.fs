module BitOperations.Tests

open Xunit
open ProgrammingInterviewElements.FSharp.BitOperations
open System
open System.Collections.Generic

let Evens4Bit = Seq.ofList [0UL;3UL;5UL;6UL;9UL;10UL;12UL;15UL]
let Odds4Bit = Seq.ofList [1UL;2UL;4UL;7UL;8UL;11UL;13UL;14UL]
let Odds = Seq.skip 1 Odds4Bit //an odd number of odds
let isEven x = x % 2 = 0
let isOdd x = not <| isEven x

// Problem 5.1 Tests

[<Fact>]
let hasEvenParityPassesForEvens() =
    Evens4Bit |> Seq.map (Assert.True << hasEvenParity)

[<Fact>]
let hasEvenParityFailsForOdds() =
    Odds4Bit |> Seq.map (Assert.False << hasEvenParity)  

[<Fact>]
let hasOddParityPassesForOdds() =
    Odds4Bit |> Seq.map (Assert.True << hasOddParity)

[<Fact>]
let hasOddParityFailsForEvens() =
    Evens4Bit |> Seq.map (Assert.False << hasOddParity)

[<Fact>]
let hasEvenParityIEnumerablePassesForEvens() =
    Assert.True <| Evens4Bit.hasEvenParity()

[<Fact>]
let hasEvenParityIEnumerablePassesForEvenNumberOfOdds() =
    Assert.True <| (Seq.length Odds4Bit |> isEven)
    Assert.True <| Odds4Bit.hasEvenParity()

[<Fact>]    
let hasEvenParityIEnumerableFailsForOddNumberOfOdds() =
    Assert.True <| (Seq.length Odds |> isOdd)
    Assert.False <| Odds.hasEvenParity()

[<Fact>]
let hasOddParityIEnumerablePassesForOddNumberOfOdds() =
    Assert.True <| (Seq.length Odds |> isOdd)
    Assert.True <| Odds.hasOddParity()

[<Fact>]
let hasOddParityIEnumerableFailsForEvenNumberOfOdds() =
    Assert.True <| (Seq.length Odds4Bit |> isEven)
    Assert.False <| Odds4Bit.hasOddParity()

[<Fact>]
let hasOddParityIEnumerableFailsForEvens() =
    Assert.False <| Evens4Bit.hasOddParity()
    

//        #region Problem 5.2 Tests
//
//        [Fact]
//        public void exchangeBitsIndexRangeTest()
//        {
//            ulong x = uint.MaxValue; //all ones in lower half
//            ulong y = x;
//            Assert.Equal(BitOperations.exchangeBits(x, 64, 0), y);
//            Assert.Equal(BitOperations.exchangeBits(x, 0, 64), y);
//            Assert.NotEqual(BitOperations.exchangeBits(x, 0, 63), y);
//        }
//
//        [Fact]
//        public void exchangeBitsIGreaterThanJ()
//        {
//            ulong x = uint.MaxValue;
//            ulong y = x;
//            ulong result = (y - 1UL) | (1UL << 63);
//            Assert.NotEqual(BitOperations.exchangeBits(x, 63, 0), y);
//            Assert.Equal(BitOperations.exchangeBits(x, 63, 0), result);
//        }
//
//        [Fact]
//        public void exchangeBitsILessThanJ()
//        {
//            ulong x = uint.MaxValue;
//            ulong y = x;
//            ulong result = (y - 1UL) | (1UL << 63);
//            Assert.NotEqual(BitOperations.exchangeBits(x, 0, 63), y);
//            Assert.Equal(BitOperations.exchangeBits(x, 0, 63), result);
//        }
//
//        [Fact]
//        public void exchangeBitsIEqualsJ()
//        {
//            ulong x = uint.MaxValue;
//            ulong y = x;
//            Assert.Equal(BitOperations.exchangeBits(x, 0, 0), y);
//        }

