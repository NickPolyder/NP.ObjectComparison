# ObjectComparison
[![CI Build](https://github.com/NickPolyder/ObjectComparison/actions/workflows/CI.yml/badge.svg)](https://github.com/NickPolyder/ObjectComparison/actions/workflows/CI.yml)
[![CD Build](https://github.com/NickPolyder/ObjectComparison/actions/workflows/CD.yml/badge.svg)](https://github.com/NickPolyder/ObjectComparison/actions/workflows/CD.yml)

The ObjectComparison library provides object difference and patching functionality through .NET API.

## Installation

In order to install this package you first need to add the package source:

```cmd
dotnet nuget add source --username {USERNAME} --password {PAT} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/NickPolyder/index.json"
```

then you can install it to your project with:

```cmd
dotnet add {PROJECT} package ObjectComparison
```

## Usage

The simplest way to start using the library is to initialise an ComparisonTracker object.

```CSharp
var sampleInstance = new SampleClass();
var comparisonTracker = new ComparisonTracker<SampleClass>(sampleInstance);
```

In order to properly use the `ComparisonTracker<T>` the `T` needs to be a class that inherits from `System.ICloneable` or make use of the `CloneFactory` delegate through the constuctor.

```CSharp
var sampleInstance = new SampleClass();
var comparisonTracker = new ComparisonTracker<SampleClass>(sampleInstance, instance => new SampleClass
   {
	 FirstName = (string)instance.FistName.Clone(),
	 LastName = (string)instance.LastName.Clone(),
	 Age = instance.Age, 
         RegistrationDate = instance.RegistrationDate,		
   });
```

When the default Comparison capabilities do not cover the use case there is the possibility to create your own 
`IObjectAnalyzer<in T>`. You can find examples on how to create Object Analyzers in the Samples.


```CSharp
var sampleInstance = new SampleClass();
var sampleObjectAnalyzer = new SampleObjectAnalyzer();
var comparisonTracker = new ComparisonTracker<SampleClass>(sampleInstance, sampleObjectAnalyzer);
```


## Roadmap

- ~~Add PatchInfo as return value to include more detailed patch information.~~
- ~~Consider creating diff functionality as well ? (That can commit after ? )~~
- ~~Finish Depth functionality on Patch~~
- ~~Add Depth functionality on Diff code~~
- ~~Maybe make a Diff functionality that can apply the patch later by calling a method ?~~ 
- ~~Figure out how to publish alpha packages outside of nuget.org~~
- ~~Make a Decorator that will provide the full comparison functionality~~
- ~~Add AutoAnalyze in HasChanges~~
- ~~Make Factory extensions~~
- ~~Make the README better~~
- __Make Ignore Attributes etc (Active)__
    - Make analyzer settings take a Default factory.
- Make examples and samples 
- Unit Tests
- Make a Memento object that will keep the history of the changes ? 
- Add Contribute section
- 


