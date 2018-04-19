
# MessageTemplates

An implementation of named string replacements, which allows formatting, parsing, and capturing properties. MessageTemplates is compatible with the [Message Templates Standard](https://messagetemplates.org/). The C# implementation was extracted from Serilog.

There is also a F# implementation, see the [messagetemplates-fsharp repository](https://github.com/messagetemplates/messagetemplates-fsharp).

[![Build status](https://ci.appveyor.com/api/projects/status/vqky5udqsjddgnx5/branch/master?svg=true)](https://ci.appveyor.com/project/adamchester/messagetemplates-csharp/branch/master)
[![NuGet](https://img.shields.io/nuget/v/MessageTemplates.svg?maxAge=2592000)](https://www.nuget.org/packages/MessageTemplates)

### Samples

### Format a C# Class

```csharp
class Chair {
    public string Back => "straight";
    public int[] Legs => new[] {1, 2, 3, 4};
    public override string ToString() => "a chair";
}

Assert.Equal(
    "I sat at Chair { Back: \"straight\", Legs: [1, 2, 3, 4] }",
    MessageTemplate.Format("I sat at {@Chair}", new Chair()));
```

### Message Template Syntax

Message templates are a superset of standard .NET format strings, so any format string acceptable to `string.Format()` will also be correctly processed by `MessageTemplates`.

* Property names are written between `{` and `}` brackets
* Brackets can be escaped by doubling them, e.g. `{{` will be rendered as `{`
* Formats that use numeric property names, like `{0}` and `{1}` exclusively, will be matched with the `Format` method's parameters by treating the property names as indexes; this is identical to `string.Format()`'s behaviour
* If any of the property names are non-numeric, then all property names will be matched from left-to-right with the `Format` method's parameters
* Property names may be prefixed with an optional operator, `@` or `$`, to control how the property is serialised
  * The destructuring operator (`@`) in front of will serialize the object passed in, rather than convert it using `ToString()`.
  * the stringification operator (`$`) will convert the property value to a string before any other processing takes place, regardless of its type or implemented interfaces.
* Property names may be suffixed with an optional format, e.g. `:000`, to control how the property is rendered; these format strings behave exactly as their counterparts within the `string.Format()` syntax

### Compiling

Install [dotnet core sdk 2.1.104](https://www.microsoft.com/net/download/dotnet-core/sdk-2.1.104) or compatible, and run `build.cmd` (windows) or `build.sh` (osx/linux).

### Rendering JSON data

_MessageTemplates_ can be used for offline rendering of log data. Often this is recorded in JSON format.

The example below shows how to take a message template and a JSON document, and render the template using values from the JSON.

JSON.NET is used for JSON parsing; to install dependencies:

```ps
Install-Package Newtonsoft.Json
Install-Package MessageTemplates -Pre
```

The example program prints the results of rendering the template out to the console:

```csharp
public class Program
{
    public static void Main()
    {
        var template = "Hello {Name}; see: {Data}";
        var json = @"{""Name"": ""Alice"", ""Data"": {""Counts"": [1, 2, 3]}}";

        var properties = (JObject) JsonConvert.DeserializeObject(json);

        var parser = new MessageTemplateParser();
        var parsed = parser.Parse(template);

        var templateProperties = new TemplatePropertyValueDictionary(new TemplatePropertyList(
            properties.Properties().Select(p => CreateProperty(p.Name, p.Value)).ToArray()));

        var rendered = parsed.Render(templateProperties);
        Console.WriteLine(rendered);
    }

    static TemplateProperty CreateProperty(string name, JToken value)
    {
        return new TemplateProperty(name, CreatePropertyValue(value));
    }

    static TemplatePropertyValue CreatePropertyValue(JToken value)
    {
        if (value.Type == JTokenType.Null)
            return new ScalarValue(null);

        var obj = value as JObject;
        if (obj != null)
        {
            var properties = obj.Properties()
                .Select(kvp => CreateProperty(kvp.Name, kvp.Value));

            return new StructureValue(properties);
        }

        var arr = value as JArray;
        if (arr != null)
        {
            return new SequenceValue(arr.Select(CreatePropertyValue));
        }

        return new ScalarValue(value.Value<object>());
    }
}
```
