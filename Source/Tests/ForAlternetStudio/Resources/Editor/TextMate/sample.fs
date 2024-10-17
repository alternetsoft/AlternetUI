module Integers =
    let sampleInteger = 176

    /// Do some arithmetic starting with the first integer
    let sampleInteger2 = (sampleInteger/4 + 5 - 7) * 4

    /// A list of the numbers from 0 to 99
    let sampleNumbers = [ 0 .. 99 ]

    /// A list of all tuples containing all the numbers from 0 to 99 and their squares
    let sampleTableOfSquares = [ for i in 0 .. 99 -> (i, i*i) ]

    // The next line prints a list that includes tuples, using %A for generic printing
    printfn "The table of squares from 0 to 99 is:\n%A" sampleTableOfSquares


module BasicFunctions =

    // Use 'let' to define a function that accepts an integer argument and returns an integer.
    let func1 x = x*x + 3

    // Parenthesis are optional for function arguments
    let func1a (x) = x*x + 3

    /// Apply the function, naming the function return result using 'let'.
    /// The variable type is inferred from the function return type.
    let result1 = func1 4573

    printfn "The result of squaring the integer 4573 and adding 3 is %d" result1

    // When needed, annotate the type of a parameter name using '(argument:type)'
    let func2 (x:int) = 2*x*x - x/5 + 3

    let result2 = func2 (7 + 4)

    printfn "The result of applying the 1st sample function to (7 + 4) is %d" result2

    let func3 x =
        if x < 100.0 then
            2.0*x*x - x/5.0 + 3.0
        else
            2.0*x*x + x/5.0 - 37.0

    let result3 = func3 (6.5 + 4.5)

    printfn "The result of applying the 2nd sample function to (6.5 + 4.5) is %f" result3
