# Blazor HotKeys [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.HotKeys.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.HotKeys/)

## Summary

This is a class library that provides configuration-centric keyboard shortcuts for your Blazor apps.

![movie](https://raw.githubusercontent.com/jsakamoto/Toolbelt.Blazor.HotKeys/master/.assets/movie-001.gif)

- [Live demo](https://jsakamoto.github.io/Toolbelt.Blazor.HotKeys/)

You can declare associations of keyboard shortcut and callback action, like this code:

```csharp
// The method "OnSelectAll" will be invoked 
//  when the user typed Ctrl+A key combination.
this.HotKeysContext = this.HotKeys.CreateContext()
  .Add(ModKeys.Ctrl, Keys.A, OnSelectAll)
  .Add(...)
  ...;
```

This library was created inspired by ["angular-hotkeys"](https://github.com/chieffancypants/angular-hotkeys).

## Supported Blazor versions

"Blazor HotKeys" ver.9.x or later supports both Blazor WebAssembly and Blazor Server.

Supported Blazor versions are as below.

- v.3.1 (including previews and release candidates.)
- v.3.2 (including previews and release candidates.)
- v.5.0 (including previews and release candidates.)
- v.6.0 (including previews and release candidates.)

## How to install and use?

### 1. Installation and Registration

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.HotKeys
```

**Step.2** Register "HotKeys" service into the DI container.

If the Blazor version of the project is ver.3.1 preview 4 or earlier, you should add the code into the `ConfigureService` method in the `Startup` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // 1. Add this line
...
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddHotKeys(); // 2. Add this line
    ...
```

If the Blazor version of the project is ver.3.2 preview 1 or later, you should add the code into your `Main` method in the `Program` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // 1. Add this line
...
public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    ...
    builder.Services.AddHotKeys(); // 2. Add this line
    ...
```

### 2. Usage in your Blazor component (.razor)

**Step.1** Implement `IDisposable` interface to the component.

```razor
@implements IDisposable @* <- Add this at top of the component.  *@
...

@code {
  ...
  public void Dispose() // <- Add "Dispose" method.
  {
  }
}
```

**Step.2** Open the `Toolbelt.Blazor.HotKeys` namespace, and inject the `HotKeys` service into the component.

```razor
@implements IDisposable
@using Toolbelt.Blazor.HotKeys @* 1. Add this *@
@inject HotKeys HotKeys @* 2. Add this *@
...
```

**Step.3** Invoke `CreateContext()` method of the `HotKeys` service instance to create and activate hot keys entries at startup of the component such as `OnInitialized()` method.

You can add the combination with key and action to the `HotKeysContext` object that is returned from `CreateContext()` method, using `Add()` method.

Please remember that you have to keep the `HotKeys Context` object in the component field.

```csharp
@code {

  HotKeysContext HotKeysContext;

  protected override void OnInitialized()
  {
    this.HotKeysContext = this.HotKeys.CreateContext()
      .Add(ModKeys.Ctrl|ModKeys.Shift, Keys.A, FooBar, "do foo bar.")
      .Add(...)
      ...;
  }

  void FooBar() // <- This will be invoked when Ctrl+Shift+A typed.
  {
    ...
  }
}
```

> _Note.1:_ You can also specify the async method to the callback action argument.

> _Note.2:_ The method of the callback action can take an argument which is `HotKeyEntry` object.


**Step.4** Destroy the `HotKeysContext` when the component is disposing, in the `Dispose()` method of the component.

```csharp
@code {
  ...
  public void Dispose()
  {
    this.HotKeysContext.Dispose(); // 1. Add this
  }
}
```

The complete source code (.razor) of this component is bellow.

```csharp
@page "/"
@implements IDisposable
@using Toolbelt.Blazor.HotKeys
@inject HotKeys HotKeys

@code {

  HotKeysContext HotKeysContext;

  protected override void OnInitialized()
  {
    this.HotKeysContext = this.HotKeys.CreateContext()
      .Add(ModKeys.Ctrl|ModKeys.Shift, Keys.A, FooBar, "do foo bar.");
  }

  void FooBar()
  {
    // Do something here.
  }

  public void Dispose()
  {
    this.HotKeysContext.Dispose();
  }
}
```

### 3. Appendix - How to enable / disable hotkeys depending on which element has focus

You can specify enabling/disabling hotkeys depending on which element has focus when hotkeys registration via a combination of the `Exclude` flags that are optional 5th argument of the `HotKeysContext.Add()` method.

By default, the `Exclude` flags argument is the following combination.

```csharp
Exclude.InputText | Exclude.InputNonText | Exclude.TextArea
```

This means, by default, hotkeys are disabled when the focus is in an `<input>` (with any `type`) or `<textarea>` element.

If you want to enable hotkeys even when an `<input type="text"/>` is focused, you can do it as below.

```csharp
... this.HotKeys.CreateContext()
  .Add(ModKeys.None, Keys.A, OnKeyDownA, "...", 
    // 👇 Specify the 5th argument.
    exclude: Exclude.InputNonText | Exclude.TextArea)
  ...
```

And you can specify the `Exclude.ContentEditable` to register the unavailable hotkey when any "contenteditable" applied elements are focused.


## Limitations

### No "Cheat Sheet"

Unlike ["angular-hotkeys"](https://github.com/chieffancypants/angular-hotkeys), this library doesn't provide "cheat sheet" feature, at this time.

Instead, the `HotKeysContext` object provides `Keys` property, so you can implement your own "Cheat Sheet" UI, like this code:

```razor
<ul>
    @foreach (var key in this.HotKeysContext.Keys)
    {
        <li>@key</li>
    }
</ul>
```

The rendering result:

```
- Shift+Ctrl+A: do foo bar.
- ...
```

## Release Note

[Release notes](https://github.com/jsakamoto/Toolbelt.Blazor.HotKeys/blob/master/RELEASE-NOTES.txt)

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.HotKeys/blob/master/LICENSE)
