namespace RCR

module public Adder =
    open ExcelDna.Integration
    open ExcelDna.IO

    let private validateArray = array1DAsArray validateFloat

    [<ExcelFunction>]
    let public MySum (elems : obj []) =
        validateArray elems
        |> Array.filter (fun x -> x.IsSome)
        |> Array.map (fun x -> x.Value)
        |> Seq.sum
