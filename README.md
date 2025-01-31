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

## License

Reflectless is licensed under the [MIT License](LICENSE).