# MathEvaluator
A simple evaluator for expressions in infix notation.
## How it works
1. Expressions stored as strings are tokenized into a `LinkedList<Token>` via:
    ```csharp
    LinkedList<Token> TokenizeExpression(string expression)
    ```

1. This list of tokens is now assumed to be in infix notation, this will be converted to postfix notation using:
    ```csharp
    LinkedList<Token> ShuntingYard(LinkedList<Token> infixTokens)
    ```
    *This function is an implementation of Dijkstra's Shunting-Yard Algorithm.*

1. Lastly, the list of tokens in postfix notation is evaluated by:
    ```csharp
    double EvaluatePostFix(LinkedList<Token> postfixExpression)
    ```

## Example Process
1. Input string: `12+3*(4/3+(3^2)+3)-5`
1. Tokenize: `12 + 3 * ( 4 / 3 + ( 3 ^ 2 ) + 3 ) - 5`
1. Shunting-Yard: `12 3 4 3 / 3 2 ^ + 3 + * + 5 -`
    ```md
    1. 12 3 4 3 / 3 2 ^ + 3 + * + 5 - 
                ^
    2. 12 3 1.333~ 3 2 ^ + 3 + * + 5 -
                       ^
    3. 12 3 1.333~ 9 + 3 + * + 5 -
                     ^
    4. 12 3 10.333~ + 3 * + 5 -
                    ^
    5. 12 3 13.333~ * + 5 -
                    ^
    6. 12 3 13.333~ * + 5 -
                    ^
    7. 12 40 + 5 -
             ^
    8. 52 5 -
            ^
    9. 47
    > DONE
    ```
1. Result: `47`

## Getting Started
1. Clone the repository.
    ```shell
    git clone https://github.com/Xapier14/MathEvaluator.git
    cd ./MathEvaluator
    ```

1. Build the project.
    ```shell
    dotnet build
    ```
    **NOTE:** You can also use msbuild or any C#/.NET compiler or environment.

1. Run the executable.
    ```shell
    dotnet run
    ```
    **NOTE:** You can also run the executable in the `./bin/` directory.

## Requirements
* .NET 5+

## License
### The MIT License

Copyright (c) 2022 Lance Crisang

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.