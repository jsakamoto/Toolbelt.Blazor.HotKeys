# Blazor WebAssembly (client-side) HotKeys [![NuGet Package](https://img.shields.io/nuget/v/Toolbelt.Blazor.HotKeys.svg)](https://www.nuget.org/packages/Toolbelt.Blazor.HotKeys/)

## Summary

This is a class library that provides configuration-centric keyboard shortcuts for your Blazor WebAssembly (client-side) apps.

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

"Blazor WebAssembly App (client-side) HotKeys" ver.8.x supports Blazor WebAssembly App version **3.1.0 preview 1, or later.**

## How to install and use?

### 1. Installation and Registration

**Step.1** Install the library via NuGet package, like this.

```shell
> dotnet add package Toolbelt.Blazor.HotKeys
```

**Step.2** Register "HotKeys" service into the DI container.

If the Blazor version of the project is ver.3.1 preview 4 or earlyer, you should add the code into  `ConfigureService` method in the `Startup` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this line, and...
...
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddHotKeys(); // <- Add this line.
    ...
```

If the Blazor version of the project is ver.3.2 preview 1 or later, you should add the code into  `Main` method in the `Program` class of your Blazor application.

```csharp
using Toolbelt.Blazor.Extensions.DependencyInjection; // <- Add this line, and...
...
public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    ...
    builder.Services.AddHotKeys(); // <!- Add this line.
    ...
```

### 2. Usage in your Blazor component (.razor)

**Step.1** Implement `IDisposable` interface to the component.

```html
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

```html
@implements IDisposable
@using Toolbelt.Blazor.HotKeys @* <- Add this, and ... *@
@inject HotKeys HotKeys @* <- And add this. *@
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
    this.HotKeysContext.Dispose(); // <- Add this.
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

## Limitations

### Server-side Blazor (Blazor Server App) is not supported

This library doesn't support Server-side Blazor (Blazor Server App), at this time.

### No "Cheat Sheet"

Unlike ["angular-hotkeys"](https://github.com/chieffancypants/angular-hotkeys), this library doesn't provide "cheat sheet" feature, at this time.

Instead, the `HotKeysContext` object provides `Keys` property, so you can implement your own "Cheat Sheet" UI, like this code:

```csharp
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

- **v.8.0.0**
    - BREAKING CHANGE: Support Blazor v.3.1.0 Preview 1 (not compatible with v.3.0.0 Preview 9 or before.)
    - Minify helper JavaScript file.
- **v.7.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 9 (not compatible with v.3.0.0 Preview 8 or before.)
- **v.6.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 8 (not compatible with v.3.0.0 Preview 7 or before.)
- **v.5.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 6 (not compatible with v.3.0.0 Preview 5 or before.)
- **v.4.0.0** - BREAKING CHANGE: Support Blazor v.3.0.0 Preview 4 (not compatible with v.0.9.0 or before.)
- **v.3.0.0** - BREAKING CHANGE: Support Blazor v.0.9.0 (not compatible with v.0.8.0 or before.)
- **v.2.0.0** - BREAKING CHANGE: Support Blazor v.0.8.0 (not compatible with v.0.7.0 or before.)
- **v.1.1.0** - Support Blazor v.0.6.0 - it was signed strong name.
- **v.1.0.0** - 1st release.

## License

[Mozilla Public License Version 2.0](https://github.com/jsakamoto/Toolbelt.Blazor.HotKeys/blob/master/LICENSE)