﻿module BitOperations.Tests

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
    Evens4Bit |> Seq.iter (Assert.True << hasEvenParity)

[<Fact>]
let hasEvenParityFailsForOdds() =
    Odds4Bit |> Seq.iter (Assert.False << hasEvenParity)  

[<Fact>]
let hasOddParityPassesForOdds() =
    Odds4Bit |> Seq.iter (Assert.True << hasOddParity)

[<Fact>]
let hasOddParityFailsForEvens() =
    Evens4Bit |> Seq.iter (Assert.False << hasOddParity)

[<Fact>]
let hasEvenParitySeqPassesForEvens() =
    Assert.True <| Seq.hasEvenParity Evens4Bit

[<Fact>]
let hasEvenParitySeqPassesForEvenNumberOfOdds() =
    Assert.True <| (Seq.length Odds4Bit |> isEven)
    Assert.True <| Seq.hasEvenParity Odds4Bit

[<Fact>]    
let hasEvenParitySeqFailsForOddNumberOfOdds() =
    Assert.True <| (Seq.length Odds |> isOdd)
    Assert.False <| Seq.hasEvenParity Odds

[<Fact>]
let hasOddParitySeqPassesForOddNumberOfOdds() =
    Assert.True <| (Seq.length Odds |> isOdd)
    Assert.True <| Seq.hasOddParity Odds

[<Fact>]
let hasOddParitySeqFailsForEvenNumberOfOdds() =
    Assert.True <| (Seq.length Odds4Bit |> isEven)
    Assert.False <| Seq.hasOddParity Odds4Bit

[<Fact>]
let hasOddParitySeqFailsForEvens() =
    Assert.False <| Seq.hasOddParity Evens4Bit
    
// Problem 5.2 Tests

[<Fact>]
let swapBitsUlongReturnsIntialValueWhenBitPositionsEqual =
    let x = uint64 UInt32.MaxValue
    Assert.Equal(x, x |> swapBits (BitPosition 0) (BitPosition 0))

[<Fact>]
let swapBitsUlongSwapsWhenArg1GreaterThanArg2 =
    let x = uint64 UInt32.MaxValue
    let expected = (x - 1UL) ||| (1UL <<< 63)
    Assert.Equal(expected, x |> swapBits (BitPosition 63) (BitPosition 0))

[<Fact>]
let swapBitsULongSwapsWhenArg1LessThanArg2 =
    let x = uint64 UInt32.MaxValue
    let expected = (x - 1UL) ||| (1UL <<< 63)
    Assert.Equal(expected, x |> swapBits (BitPosition 0) (BitPosition 63))

// Problem 5.3 Tests

type UInt64 with
    static member random =
        let r = Random()
        let bytes = Array.create sizeof<ulong> 0uy
        let internalRandom() =
            r.NextBytes bytes
            BitConverter.ToUInt64(bytes, 0)
        internalRandom
    static member randoms count =
        List.init count (fun _ -> ulong.random())   
                 
[<Fact>]
let reverseBitsReversesBits() = 
    let test x =
        let result = reverseBits x
        let resultString = Convert.ToString(int64 result,2).PadLeft(64,'0')
        let givenString = Convert.ToString(int64 x,2).PadLeft(64,'0')
        let givenStringReversed = String(Array.rev(givenString.ToCharArray()))
        Assert.Equal(givenStringReversed, resultString)
    ulong.randoms 30 |> List.iter test