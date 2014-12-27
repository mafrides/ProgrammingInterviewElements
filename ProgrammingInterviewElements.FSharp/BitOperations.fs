namespace ProgrammingInterviewElements.FSharp
    
    open System
    open System.Collections.Generic
    open System.Runtime.CompilerServices

    module BitOperations =

        type Bit = One | Zero

        [<Struct>]
        type BitPosition =
            val public Position : int
            new(i:int) =  
                { Position = if (i < 0 || i > 63)
                             then raise (InvalidOperationException("Can only access bits 0-63."))
                             else i
                }

        type ulong = UInt64

        type UInt64 with
            member x.getBit (i:BitPosition) =
                if (x &&& (1UL <<< i.Position) = 0UL) then Zero else One
            member x.setBit (i:BitPosition) = 
                x ||| (1UL <<< i.Position)
            member x.clearBit (i:BitPosition) =
                x &&& ~~~(1UL <<< i.Position)    
            member x.setBitTo (position:BitPosition) value =
                match value with
                | One -> x.setBit position
                | Zero -> x.clearBit position

            //Solution to Problem 5.2 as extensions to UInt64
            member x.swapBits i j =
                let newIValue, newJValue = x.getBit j, x.getBit i
                match newIValue, newJValue with
                | Zero,Zero -> x.clearBit(i).clearBit(j)
                | One,Zero -> x.setBit(i).clearBit(j)
                | Zero,One -> x.clearBit(i).setBit(j)
                | One,One -> x.setBit(i).setBit(j)

        let hasEvenParity (x:ulong) =
            let powersOf2_1To32 =
                Seq.unfold (fun i -> if i >= 64 then None else Some(i, i * 2)) 1
            let bitwiseXOrInHighBit = powersOf2_1To32 |> Seq.fold (fun acc offset -> acc ^^^ (acc <<< offset)) x 
            bitwiseXOrInHighBit.getBit(BitPosition 63) = Zero

        let hasOddParity (x:ulong) =
            not <| hasEvenParity x

        //Solutions to Prob 5.1 Expressed as extensions to IEnumberbale<ulong>
        [<Extension>]
        type IEnumerable() =
            [<Extension>]
            static member inline hasEvenParity(collection:IEnumerable<ulong>) =
                Seq.reduce (fun x y -> x ^^^ y) collection |> hasEvenParity
            //Could not figure out how to call one extension method from another
            //not sure why not <| collection.hasEvenParity() does not work
            //or why not << IEnumerable<ulong>.hasEvenParity shouldn't work
            [<Extension>]
            static member inline hasOddParity(collection:IEnumerable<ulong>) =
                Seq.reduce (fun x y -> x ^^^ y) collection |> hasEvenParity |> not
    

