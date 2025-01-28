# Reflectless

**Reflectless** is a library designed to access properties and fields by name without using reflection. By avoiding the overhead of reflection, Reflectless provides significantly faster execution, making it an ideal choice for performance-critical applications.

## Features

- **Fast Property and Field Access**: Access members by name with minimal overhead.
- **No Reflection Overhead**: Achieve better performance by avoiding reflection entirely.
- **Easy to Use**: Simple and intuitive API for seamless integration.
- **Lightweight**: Designed with efficiency in mind, keeping your applications lean.

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

### Example

```csharp
using Reflectless;

public class Example
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var instance = new Example { Name = "John", Age = 30 };

// Access properties by name
var nameGetter = Reflectless.GetPropertyGetAccessor<Example, string>("Name");
var ageSetter = Reflectless.GetPropertySetAccessor<Example, int>("Age");

Console.WriteLine(nameGetter(instance)); // Output: John
ageSetter(instance, 35);
Console.WriteLine(instance.Age); // Output: 35
```

## License

Reflectless is licensed under the [MIT License](LICENSE).