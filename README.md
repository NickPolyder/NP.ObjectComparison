# NP.ObjectComparison
[![CI Build](https://github.com/NickPolyder/NP.ObjectComparison/actions/workflows/CI.yml/badge.svg)](https://github.com/NickPolyder/NP.ObjectComparison/actions/workflows/CI.yml)
[![CD Build](https://github.com/NickPolyder/NP.ObjectComparison/actions/workflows/CD.yml/badge.svg)](https://github.com/NickPolyder/NP.ObjectComparison/actions/workflows/CD.yml)
![Nuget](https://img.shields.io/nuget/v/NP.ObjectComparison?color=blue&logo=nuget&style=flat)

The NP.ObjectComparison library provides object difference and patching functionality through .NET API.

## Installation


To install the package to a project:

```cmd
dotnet add {PROJECT} package NP.ObjectComparison
```

The package source for dev streams can be added through:

```cmd
dotnet nuget add source --name github "https://nuget.pkg.github.com/NickPolyder/index.json"
```

## Usage

The simplest way to start using the library is to initialize an ComparisonTracker object.

```CSharp
var sampleInstance = new SampleClass();
var comparisonTracker = new ComparisonTracker<SampleClass>(sampleInstance);
```

In order to properly use the `ComparisonTracker<T>` the `T` needs to be a class that inherits from `System.ICloneable` or make use of the `CloneFactory` delegate through the constructor.

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

### Ignoring properties

To ignore a property or a class there are two ways available:

1. By using the attribute `[Ignore]` from the namespace: `NP.ObjectComparison.Attributes` on a property or a class.

For a property:

```CSharp
using NP.ObjectComparison.Attributes;

public class ToBeAnalyzed
{
	[Ignore]
	public string Value { get; set; }
}
```


For a class:

```CSharp
using NP.ObjectComparison.Attributes;

[Ignore]
public class ToBeAnalyzed
{	
	public string Value { get; set; }
}
```


2. By providing an initialized instance of `AnalyzerSettings`

```CSharp
var analyzerSettings = new AnalyzerSettings();
var typeToIgnore = typeof(ToBeAnalyzed);

// Ignoring properties
var propertyInfo = typeToIgnore.GetProperty(nameof(ToBeAnalyzed.Value));
analyzerSettings.IgnoreSettings.Ignore(propertyInfo);

// OR
analyzerSettings
	.IgnoreSettings
	.Ignore<ToBeAnalyzed, string>(model => model.Value)


// Ignoring classes
analyzerSettings.IgnoreSettings.Ignore(typeToIgnore);

// OR
analyzerSettings.IgnoreSettings.Ignore<ToBeAnalyzed>();

// Set the new instance
AnalyzerSettings.DefaultSettings = () => analyzerSettings;

```
