namespace ExcelDna

module public IO =
    open ExcelDna.Integration
    open Util.Extensions

    let public valueToExcel value =
        value |> Option.map box
              |> Option.getOrElse (ExcelError.ExcelErrorNA :> obj)
    
    let (|ExcelError|_|) x =
        if x.GetType() = typeof<ExcelError> then Some() else None
    
    let (|ExcelEmpty|_|) x =
        if x.GetType() = typeof<ExcelEmpty> then Some() else None
   
    let (|ExcelMissing|_|) x =    
        if x.GetType() = typeof<ExcelMissing> then Some() else None
    let private wrapErrorValues x  =
        match x with
            | ExcelError | ExcelEmpty | ExcelMissing    ->  None
            | _                                         ->  Some(x)

    let private wrapprimitive x = if x.GetType().IsPrimitive then Some(x)
                                  elif x.GetType() = typeof<string> then Some(x) // Strings are not primitive types in .NET but for my purpose here I'll consider them to be... 
                                  elif x.GetType() = typeof<System.DateTime> then Some(x) // DateTimes are not primitive types in .NET but for my purpose here I'll consider them to be
                                  else None

    let public array1DAsArray validationfunction =
            Array.map (wrapErrorValues >> Option.bind validationfunction)
    
    let private validateType<'T> input =
        try Some(System.Convert.ChangeType(input, typeof<'T>) :?> 'T)
        with _ -> None //This error catching mechanism is slow, but shouldn't occur to frequently if the wrapErrorValues function is used to catch wrong input first

    let private validate<'T> input =
        input   |> wrapErrorValues
                |> Option.bind validateType<'T>

    let public validateFloat input = validate<float> input

    let public validateInt input = validate<int> input

    let public validateBool input =
        input   |> validate<bool>
                |> fun x -> if x.IsSome then x
                            else input  |> validateFloat
                                        |> Option.map (fun x -> x > 0.)
    /// Takes an obj option and tries to cast it into Some(DateTime). If not possible, None is returned. Numeric types are mapped to true if they are > 0                  
    let public validateDate input =
        input   |> validate<float> |> Option.map (fun x -> System.DateTime.FromOADate(x)) // First check for float rather than date because that's how obj type's typically come through.
                                                                                          // Makes a huge difference on performance, vs try-catching System.DateTime first, because catching
                                                                                          // errors is so slow.
                |> fun x -> if x.IsSome then x else input  |> validate<System.DateTime>
                |> fun x -> if x.IsSome then x else input  |> validate<string> |> Option.bind (fun x -> System.DateTime.tryParse(x))