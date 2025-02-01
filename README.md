# Reflectless

**Reflectless** is a library designed to access properties, fields, constructors and methods by name without using reflection. Reflectless is useful when members of given class discovered using reflection and then required to read or write. This is very useful if the number of operations are very high and possible uses include serialisation or ORM frameworks.

## Installation

Install the package via NuGet:

```bash
Install-Package Reflectless
```

Or use the .NET CLI:

```bash
dotnet add package Reflectless
```

## Getting Started

Here’s a quick example to demonstrate how to use Reflectless:

```csharp
var instance = new Example { Name = "John", Age = 30 };

var properties = typeof(Example).GetProperties();

foreach (var propertyInfo in properties)
{
    var getter = Reflectless.Reflectless.GetPropertyGetAccessor(typeof(Example), propertyInfo.Name);
    var value = getter(instance);
    Console.WriteLine($"Property {propertyInfo.Name}: {value}");
}

public class Example
{
    public string Name { get; set; }
    public int Age { get; set; }
}

//Output:
//Property Name: John
//Property Age: 30
```

## Cache

Accessing via `Reflectless.<method name>` caches the result so that the subsequent access is faster. For some reason the cache is not required or a custom cache is required `ReflectlessNoCache.<method name>` can be used. 

Note: Not recommanded to use without provided cache or a custom cache since it will have a significant performance impact.

## Non-Generic and Generic approaches

There are 2 different ways of doing the same operation. 

1. Non generic (method with parameters) approach:

    This approach is the probably most useful as it can be used with reflection discovered members of a class to manipulate values.

2. Generic appraoch

    With this approach the generic signatures required.

## Examples

### Read Fields

```csharp
var instance = new Example { _name = "John" };

//non generic approach
var nonGenericAccessor = Reflectless.Reflectless.GetFieldGetAccessor(typeof(Example), "_name");
Console.WriteLine($"Non generic : {nonGenericAccessor(instance)}");

//generic approach
var genericAccessor = Reflectless.Reflectless.GetFieldGetAccessor<Example,string>("_name");
Console.WriteLine($"Generic : {genericAccessor(instance)}");

public class Example
{
    public string _name;
}

//Output:
//Non generic : John
//Generic : John
```

### Write Fields

```csharp
var instance = new Example { _name = "John" };

//non generic approach
var nonGenericAccessor = Reflectless.Reflectless.GetFieldSetAccessor(typeof(Example), "_name");
nonGenericAccessor(instance, "Peter");
Console.WriteLine($"Non generic : {instance._name}");

//generic approach
var genericAccessor = Reflectless.Reflectless.GetFieldSetAccessor<Example,string>("_name");
genericAccessor(instance, "Steve");
Console.WriteLine($"Generic : {instance._name}");

public class Example
{
    public string _name;
}

//Output:
//Non generic : Peter
//Generic : Steve
```

### Read Properties

```csharp
var instance = new Example { Name = "John" };

//non generic approach
var nonGenericAccessor = Reflectless.Reflectless.GetPropertyGetAccessor(typeof(Example), "Name");
Console.WriteLine($"Non generic : {nonGenericAccessor(instance)}");

//generic approach
var genericAccessor = Reflectless.Reflectless.GetPropertyGetAccessor<Example, string>("Name");
Console.WriteLine($"Generic : {genericAccessor(instance)}");

public class Example
{
    public string Name { get; set; }
}

//Output:
//Non generic : John
//Generic : John
```

### Write Properties

```csharp
var instance = new Example { Name = "John" };

//non generic approach
var nonGenericAccessor = Reflectless.Reflectless.GetPropertySetAccessor(typeof(Example), "Name");
nonGenericAccessor(instance, "Peter");
Console.WriteLine($"Non generic : {instance.Name}");

//generic approach
var genericAccessor = Reflectless.Reflectless.GetPropertySetAccessor<Example,string>("Name");
genericAccessor(instance, "Steve");
Console.WriteLine($"Generic : {instance.Name}");

public class Example
{
    public string Name { get; set; }
}

//Output:
//Non generic : Peter
//Generic : Steve
```

### Create Instances

```csharp
//default constructor with non generic approach
var defaultNonGenericConstructorAccessor = Reflectless.Reflectless.GetDefaultConstructorAccessor(typeof(Example));
var instance1  = (Example)defaultNonGenericConstructorAccessor();
Console.WriteLine($"instance 1 : {instance1.Name}");

//default constructor with generic approach
var defaultGenericConstructorAccessor = Reflectless.Reflectless.GetDefaultConstructorAccessor<Example>();
var instance2 = defaultGenericConstructorAccessor();
Console.WriteLine($"instance 2 : {instance2.Name}");

//parameterised constructor with non generic approach
var nonGenericParameterConstructorAccessor = Reflectless.Reflectless.GetConstructorAccessor<Func<object,object>>(typeof(Example),typeof(string));
var instance3 = (Example)nonGenericParameterConstructorAccessor("Adipa");
Console.WriteLine($"instance 3 : {instance3.Name}");

//parameterised constructor with generic approach
var genericParameterConstructorAccessor = Reflectless.Reflectless.GetConstructorAccessor<Func<string,Example>>();
var instance4 = genericParameterConstructorAccessor("Steve");
Console.WriteLine($"instance 4 : {instance4.Name}");

public class Example
{
    public Example()
    {
        Name = "Peter";
    }

    public Example(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}

//Output:
//instance 1 : Peter
//instance 2 : Peter
//instance 3 : Adipa
//instance 4 : Steve
```

### Calling Methods

```csharp
var instance = new Example { Name = "John" };

//non generic approach
var nonGenericMethodAccessor = Reflectless.Reflectless.GetMethodAccessor<Func<object,object,object>>(typeof(Example), "AppendToName", typeof(string));
nonGenericMethodAccessor(instance, " Wick");
Console.WriteLine($"Non generic : {instance.Name}");

//generic approach
var genericMethodAccessor = Reflectless.Reflectless.GetMethodAccessor<Func<Example,string,string>>("AppendToName");
genericMethodAccessor(instance, "'s Dog");
Console.WriteLine($"Generic : {instance.Name}");


public class Example
{
    public string AppendToName(string suffix)
    {
        Name += suffix;
        return Name;
    }

    public string Name { get; set; }
}

//Output:
//Non generic : John Wick
//Generic : John Wick's Dog
```

## Benchmark Results

There is a benchmark project in the solution, and the benchmark run produced the following results. The comparison shows that using Reflectless is significantly faster than Reflection.

| Type                 |    Method                  | N     | Mean          | Error         | StdDev       | Gen0    | Allocated |
|--------------------- |--------------------------- |------ |--------------:|--------------:|-------------:|--------:|----------:|
| BenchmarkPropertyGet |    Reflection_GetProperty  | 1000  |  11,331.68 ns |    255.813 ns |   213.615 ns |  1.7090 |   24000 B |
| BenchmarkPropertyGet |    Reflectless_GetProperty | 1000  |   5,093.91 ns |    230.052 ns |   225.942 ns |  1.7090 |   24000 B |
| BenchmarkPropertyGet |    Reflection_GetProperty  | 10000 | 126,646.70 ns | 10,166.555 ns | 9,984.916 ns | 19.0430 |  240002 B |
| BenchmarkPropertyGet |    Reflectless_GetProperty | 10000 |  43,246.86 ns |  1,983.637 ns | 1,656.426 ns | 19.0430 |  240000 B |
| BenchmarkPropertySet |    Reflection_SetProperty  | 1000  |  19,736.30 ns |    852.026 ns |   796.986 ns |  4.3945 |   56000 B |
| BenchmarkPropertySet |    Reflectless_SetProperty | 1000  |   5,326.45 ns |    269.042 ns |   264.235 ns |  1.7090 |   24000 B |
| BenchmarkPropertySet |    Reflection_SetProperty  | 10000 | 188,452.03 ns |  4,535.729 ns | 4,242.724 ns | 44.4336 |  560002 B |
| BenchmarkPropertySet |    Reflectless_SetProperty | 10000 |  47,436.37 ns |    825.329 ns |   810.583 ns | 19.0430 |  240000 B |
| BenchmarkFieldGet    |    Reflection_GetField     | 1000  |  33,106.71 ns |    289.840 ns |   271.117 ns |  1.7090 |   24000 B |
| BenchmarkFieldGet    |    Reflectless_GetField    | 1000  |   4,913.20 ns |    198.384 ns |   194.839 ns |  1.7090 |   24000 B |
| BenchmarkFieldGet    |    Reflection_GetField     | 10000 | 332,773.66 ns |  5,139.859 ns | 5,048.029 ns | 19.0430 |  240002 B |
| BenchmarkFieldGet    |    Reflectless_GetField    | 10000 |  45,000.30 ns |  1,190.955 ns | 1,169.677 ns | 19.0430 |  240002 B |
| BenchmarkFieldSet    |    Reflection_SetField     | 1000  |  30,849.02 ns |    503.023 ns |   494.036 ns |  1.7090 |   24000 B |
| BenchmarkFieldSet    |    Reflectless_SetField    | 1000  |   5,419.15 ns |    392.748 ns |   385.731 ns |  1.7090 |   24000 B |
| BenchmarkFieldSet    |    Reflection_SetField     | 10000 | 315,256.09 ns |  6,052.070 ns | 5,365.003 ns | 19.0430 |  240002 B |
| BenchmarkFieldSet    |    Reflectless_SetField    | 10000 |  49,378.96 ns |    655.125 ns |   612.804 ns | 19.0430 |  240000 B |
| BenchmarkConstructor |    Reflection_Construct    | 1000  |      11.59 ns |      0.557 ns |     0.521 ns |       - |         - |
| BenchmarkConstructor |    Reflectless_Construct   | 1000  |      11.70 ns |      0.864 ns |     0.849 ns |       - |         - |
| BenchmarkConstructor |    Reflection_Construct    | 10000 |      11.50 ns |      0.531 ns |     0.497 ns |       - |         - |
| BenchmarkConstructor |    Reflectless_Construct   | 10000 |      11.37 ns |      0.553 ns |     0.518 ns |       - |         - |
| BenchmarkMethod      |    Reflection_Method       | 1000  |  23,515.63 ns |  1,655.880 ns | 1,626.296 ns |  6.3477 |   80000 B |
| BenchmarkMethod      |    Reflectless_Method      | 1000  |  10,081.17 ns |    699.538 ns |   687.040 ns |  3.6621 |   48000 B |
| BenchmarkMethod      |    Reflection_Method       | 10000 | 252,679.33 ns |  8,216.750 ns | 8,069.947 ns | 63.7207 |  800002 B |
| BenchmarkMethod      |    Reflectless_Method      | 10000 |  78,824.49 ns |  9,074.282 ns | 8,488.089 ns | 38.0859 |  480000 B |


## License

Reflectless is licensed under the [MIT License](LICENSE).