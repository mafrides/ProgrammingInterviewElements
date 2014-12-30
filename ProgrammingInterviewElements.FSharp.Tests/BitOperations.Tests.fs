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

// Tests for BitPosition Type range (uses ulong.getBit)

[<Fact>]
let bitPositionRejectsNegativePositionsWithInvalidArgs() =
    let action() = ulong.MaxValue.getBit(BitPosition -1) |> ignore 
    Assert.Throws<ArgumentException>(action) |> ignore

[<Fact>]
let bitPositionRejectsNegativePositionsWithInvalidArgs2() =
    let action() = ulong.MaxValue.getBit(BitPosition Int32.MinValue) |> ignore 
    Assert.Throws<ArgumentException>(action) |> ignore

[<Fact>]
let bitPositionRejectsPositionGreaterThan63WithInvalidArgs() =
    let action() = ulong.MaxValue.getBit(BitPosition 64) |> ignore 
    Assert.Throws<ArgumentException>(action) |> ignore

[<Fact>]
let bitPositionRejectsPositionGreaterThan63WithInvalidArgs2() =
    let action() = ulong.MaxValue.getBit(BitPosition Int32.MaxValue) |> ignore 
    Assert.Throws<ArgumentException>(action) |> ignore

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
    
// Problem 5.2 Tests

[<Fact>]
let swapBitsUlongReturnsIntialValueWhenBitPositionsEqual =
    let x = uint64 UInt32.MaxValue
    Assert.Equal(x, x.swapBits (BitPosition 0) (BitPosition 0))

[<Fact>]
let swapBitsUlongSwapsWhenArg1GreaterThanArg2 =
    let x = uint64 UInt32.MaxValue
    let expected = (x - 1UL) ||| (1UL <<< 63)
    Assert.Equal(expected, x.swapBits (BitPosition 63) (BitPosition 0))

[<Fact>]
let swapBitsULongSwapsWhenArg1LessThanArg2 =
    let x = uint64 UInt32.MaxValue
    let expected = (x - 1UL) ||| (1UL <<< 63)
    Assert.Equal(expected, x.swapBits (BitPosition 0) (BitPosition 63))

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
        Array.init count (fun _ -> ulong.random())   
                 
[<Fact>]
let reverseBitsUInt64ReversesBits() = 
    let test (x:ulong) =
        let result = x.reverseBits()
        let resultString = Convert.ToString(int64 result,2).PadLeft(64,'0')
        let testString = Convert.ToString(int64 x,2).PadLeft(64,'0')
        Assert.Equal(String(Array.rev(testString.ToCharArray())), resultString)
    ulong.randoms 30 |> Array.iter test