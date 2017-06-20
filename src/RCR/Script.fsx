// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "../packages/ExcelDna.Integration.0.34.6/lib/ExcelDna.Integration.dll"
#load "Util.Extensions.fs"
#load "ExcelDna.IO.fs"
#load "RCR.fs"
open RCR

RCR.Adder.MySum [|1; 2; 3|]

