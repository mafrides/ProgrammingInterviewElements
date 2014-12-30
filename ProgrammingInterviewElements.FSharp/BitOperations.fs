module ProgrammingInterviewElements.FSharp.BitOperations

open System
open System.Collections.Generic
open System.Runtime.CompilerServices

type Bit = One | Zero

// I would remove the exception if this were library code
[<Struct>]
type BitPosition(i:int) =
    member x.Position = if (i < 0 || i > 63)
                        then invalidArg "Position" "Bit Position Range [0-63]."
                        else i

type ulong = UInt64

type UInt64 with
    member x.getBit (i:BitPosition) =
        if (x &&& (1UL <<< i.Position) = 0UL) then Zero else One            
    member x.setBitTo value (i:BitPosition) =
        match value with
        | One ->  x ||| (1UL <<< i.Position)
        | Zero -> x &&& ~~~(1UL <<< i.Position)
    member x.setBit (i:BitPosition) = x.setBitTo One
    member x.clearBit (i:BitPosition) = x.setBitTo Zero
    //Solution to Problem 5.2 as extension to UInt64
    member x.swapBits i j = 
        let y = x.setBitTo (x.getBit i) j
        y.setBitTo (x.getBit j) i
    //Solution to Problem 5.3 as extension to UInt64
    member x.reverseBits =
        let Masks = 
            [| 0x00000000ffffffffUL, 32;
               0x0000ffff0000ffffUL, 16;
               0x00ff00ff00ff00ffUL,  8;
               0x0f0f0f0f0f0f0f0fUL,  4;
               0x3333333333333333UL,  2;
               0x5555555555555555UL,  1|]
        let reverseBitsInternal() =
            let num = ref x
            let flip(mask,offset) = 
                num := ((!num >>> offset) &&& mask) ||| ((!num <<< offset) &&& (mask <<< offset))
            Masks |> Array.iter flip               
            !num
        reverseBitsInternal

let hasEvenParity (x:ulong) =
    let powersOf2_1To32 = Seq.unfold (fun i -> if i >= 64 then None else Some(i, i * 2)) 1
    let bitwiseXOrInHighBit = powersOf2_1To32 |> Seq.fold (fun acc offset -> acc ^^^ (acc <<< offset)) x 
    bitwiseXOrInHighBit.getBit(BitPosition 63) = Zero

let hasOddParity (x:ulong) =
    not <| hasEvenParity x

//Solutions to Prob 5.1 Expressed as extensions to IEnumberbale<ulong>
[<Extension>]
type IEnumerable() =
    [<Extension>]
    static member inline hasEvenParity(collection:IEnumerable<ulong>) =
        hasEvenParity <| Seq.reduce (fun x y -> x ^^^ y) collection
    //Could not figure out how to call one extension method from another
    //not sure why not <| collection.hasEvenParity() does not work
    //or why not << IEnumerable<ulong>.hasEvenParity shouldn't work
    [<Extension>]
    static member inline hasOddParity(collection:IEnumerable<ulong>) =
        hasOddParity <| Seq.reduce (fun x y -> x ^^^ y) collection 
    
    

    
    

