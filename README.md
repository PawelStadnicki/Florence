# Florence
F# DSL to evaluate places with a single function


<img src="https://wrometrcloud.blob.core.windows.net/florence/kafel.png" alt="Ballerina logo" height="200" />

## Name
In the F# community, there is a practice (though not an obligation) to name libraries with the letter "F". 

Florence (Firenze), as the city of the Renaissance, was an obvious choice.

## 🙏 Credits

I'm using a number of existing .NET and JavaScript libraries for geospatial analytics, abstracting away much of their complexity (a full list will be added shortly).

There’s still a lot of added value and creative work involved — including polyglot integration and metaprogramming features (though not *yet* the hardcore functional programming side 😉).

I was one of the early adopters of the **Polyglot Extension** for Visual Studio Code and had early communication with its creators (especially the [.NET Interactive](https://github.com/dotnet/interactive) team).

# 📄 README in Progress

Hi! 👋 This README is currently a work in progress — I'm in the process of writing it and adding more details.

🛠️ Expect updates in the coming hours or days as I document the project structure, usage, goals, and fix breaking changes of dependencies.

Thanks for your patience!


<img src="https://wrometrcloud.blob.core.windows.net/florence/f2.png" alt="City as a Function" style="width: 200px;" />

# The idea of "City as a Function" λ

This library serves many purposes, but perhaps the main one is to make F# accessible to newcomers — immediately! I believe there's no better way to start learning than by jumping in right away with no setup and, most importantly, within a tangible domain.

And what could be more tangible than the space where you live your everyday life — your home, work, and surroundings like your kid’s kindergarten?

Of course, there's more to it (like, say... finding the best place to live), but before we get too ambitious with the library's goals, let’s focus on how to actually use it.

It’s kind of self-explanatory, to be honest.


## Quick start
Florence is an interactive, polyglot library currently available via the Polyglot Extension in Visual Studio Code.
To use it, you’ll also need the latest .NET SDK (version 9) installed.

To get the library just type in F# cell the following:
<img src="https://wrometrcloud.blob.core.windows.net/florence/img1.png"/>

Florence can work with any data — unless your city is terrible at opening and publishing it 😅.
I’ll show you how to load your own data in a moment, but you can start right away with a few preloaded data sources included in the library for exploratory purposes.

<img src="https://wrometrcloud.blob.core.windows.net/florence/img2.png"/>

Let’s say these people are the classic four F’s: family, friends, and fools.
We’d like to have some places to evaluate, and we have two datasets: a few areas of Florence and some top points of interest.
<img src="https://wrometrcloud.blob.core.windows.net/florence/img3.png"/>

Now, how can we rank all city areas based on the average distance to the nearest 100 top city locations?
<img src="https://wrometrcloud.blob.core.windows.net/florence/img4.png"/>

What if instaed of the top locations we want to use distances to our family?
//TODO

## DSL for distances
//explanation is comming shortly
<img src="https://wrometrcloud.blob.core.windows.net/florence/img5.png"/>

## Entry your own lifespace
//explanation is comming shortly
<img src="https://wrometrcloud.blob.core.windows.net/florence/img6.png"/>

## Loading local data and files
//TODO: a lot of docs to come shortly
 



