# ExpressionLanguage
Language that compiles at runtime.
This is basically a new visual programming language that enables you to write code and run it while at runtime. I was thinking about why we don’t have something like scratch in the business world, anyone can use it, simple intuitive and easy to learn. Sure, what I did till now isn’t even close to MIT scratch, but I will figure out how to continue and how we can do it.

![alt text](https://raw.githubusercontent.com/Mohamed-Ahmed-Abdullah/ExpressionLanguage/master/Expression_Script.gif "How it works")

### How It Works
* **WPF UI**: A canvas and a set of rectangles you can write your code with and it converted to simple string code
* **Irony**: Irony is a development kit for implementing languages on .NET platform
Grammar
* **Irony Tree**: Irony is giving a tree of your grammar
* **Expression Tree**: We use the irony tree and convert it to expression tree The idea here is simple. First, we need to create a grammar for Irony to use it for generating the code tree, then use the UI getting the code, pass it irony, in the result we are going to have the Code tree, taking this tree and convert it to C# Expression tree and getting the final result from it.

### Read more
https://www.codeproject.com/Articles/817854/Expression-Script-inspired-from-MIT-Scratch
