namespace Util

module public Extensions =
    module public Option =
        let getOrElse defaultValue = function
        | Some x -> x
        | _ -> defaultValue


    // Extension methods on System.DateTime
    type public System.DateTime with
        /// Override for the System.DateTime.tryParse that is more idiotmatic for F#
        static member tryParse (s : string) =
            let ok, res = System.DateTime.TryParse(s)
            if ok then Some(res) else None