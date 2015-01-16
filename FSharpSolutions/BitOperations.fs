module BitOperations

open System
open System.Collections.Generic

type Bit = One | Zero
type bitPosition = BitPosition of int
type ulong = UInt64

let getBit (BitPosition i) x =
    if (x &&& (1UL <<< i) = 0UL) then Zero else One

let setBitTo value (BitPosition i) x =
    match value with
    | One -> x ||| (1UL <<< i)
    | Zero -> x &&& ~~~(1UL <<< i)

let setBit = setBitTo One
let clearBit = setBitTo Zero
let getHighBit = getBit (BitPosition 63)
let getLowBit = getBit (BitPosition 0)

// Solution to Prob 5.1
// Calculate parity of a sequence of numbers

let hasEvenParity x =
    let powersOf2_1To32 = Seq.unfold (fun i -> if i >= 64 then None else Some(i, i * 2)) 1
    let bitwiseXOrInHighBit = powersOf2_1To32 |> Seq.fold (fun acc offset -> acc ^^^ (acc <<< offset)) x 
    getHighBit bitwiseXOrInHighBit = Zero

let hasOddParity = not << hasEvenParity

module Seq =
    let hasEvenParity xs = 
        hasEvenParity <| Seq.reduce (fun x y -> x ^^^ y) xs
    
    let hasOddParity xs =
        not <| hasEvenParity xs

// Solution to Problem 5.2
// Swap bits by index

let swapBits i j x =
    let newI = getBit j x
    let newJ = getBit i x
    let y = setBitTo newI i x
    setBitTo newJ j y

// Solution to Problem 5.3
// Reverse bits in a number

let reverseBits = 
    let Masks = 
        [ 0x00000000ffffffffUL, 32
          0x0000ffff0000ffffUL, 16
          0x00ff00ff00ff00ffUL,  8
          0x0f0f0f0f0f0f0f0fUL,  4
          0x3333333333333333UL,  2
          0x5555555555555555UL,  1]
    let reverseBitsInternal x =
        let flip num (mask,offset) = 
            ((num >>> offset) &&& mask) ||| ((num <<< offset) &&& (mask <<< offset))
        Masks |> List.fold flip x               
    reverseBitsInternal 

// Solution to Problem 5.4
// Find nearest number with equal weight (equal number of 1's)

let nearestEqualWeight x =
    let rec parse i acc =
        let current = getBit (BitPosition i) x
        if current = acc 
        then parse (i + 1) current
        else swapBits (BitPosition i) (BitPosition <| i - 1) x
    match x with
    | 0UL -> x
    | UInt64.MaxValue -> x
    | _ -> parse 1 (getBit (BitPosition 0) x)
        
// Solution to Problem 5.5
// Multiply 2 unsigned numbers without + or *  

// unchecked 
let bitAdd x y =
    let rec bitAddInternal x' y' =
        let sum = x' ^^^ y';
        let carry = (x' &&& y') <<< 1
        match carry with
        | 0UL -> sum
        | _ -> bitAddInternal sum carry
    bitAddInternal x y

// unchecked
let bitMult x y =
    let folder acc i =
        match getBit (BitPosition i) y with
        | Zero -> acc
        | One -> acc |> bitAdd <| (x <<< i)
    List.fold folder 0UL [0..63]
    
// Solution to Problem 5.6
// Divide 2 unsigned numbers with +, -, shift

let tryBitDiv x y =
    Some <| x + y + 0UL
    

