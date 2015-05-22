# Atrico.Lib.CommandLineParser
Command line parser project

## Why?
Having looked around, nothing I can find fits my requirements exactly.  My old library did exactly what I wanted so this is a refactored version using attributes to specify options

## Features

## Syntax


```
private class Options
{
    [Option]
    public bool Optional { get; set; }
    [Option, Required = true]
    public bool Mandatory { get; set; }
}
```


