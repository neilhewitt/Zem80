# Debug Output Markup Guide

The debugger output now supports rich text formatting using a simple markup syntax. This allows you to add color coding, bold, and italic formatting to debug messages.

## Supported Tags

### Color Tags
Use `<color:colorname>` or `<color:#RRGGBB>` to set text color:

```csharp
AddDebuggerOutput("<color:red>Error: Invalid address</color>");
AddDebuggerOutput("<color:#FF5733>Custom color message</color>");
```

**Supported Named Colors:**
- red, green, blue
- yellow, cyan, magenta
- orange, white
- gray, lightgray, darkgray
- lime, purple, pink

### Bold Tag
Use `<bold>` to make text bold:

```csharp
AddDebuggerOutput("<bold>Important message</bold>");
```

### Italic Tag
Use `<italic>` to make text italic:

```csharp
AddDebuggerOutput("<italic>Note: This is a hint</italic>");
```

## Nesting Tags

Tags can be nested to combine formatting:

```csharp
AddDebuggerOutput("<color:yellow><bold>Warning!</bold></color>");
AddDebuggerOutput("<color:cyan>Register A: <bold>0xFF</bold></color>");
AddDebuggerOutput("<bold><italic>Bold and italic text</italic></bold>");
```

## Examples

### Breakpoint Messages
```csharp
AddDebuggerOutput("<color:yellow><bold>Breakpoint reached!</bold></color>");
AddDebuggerOutput("<color:cyan>Address:</color> <color:orange>0x1234</color>");
```

### Register Display
```csharp
AddDebuggerOutput("<color:green>Registers:</color>");
AddDebuggerOutput("  <color:cyan>A:</color> <bold>0xFF</bold>");
AddDebuggerOutput("  <color:cyan>BC:</color> <bold>0x1234</bold>");
```

### Error Messages
```csharp
AddDebuggerOutput("<color:red><bold>ERROR:</bold> Invalid instruction at 0x5678</color>");
```

### Mixed Formatting
```csharp
AddDebuggerOutput("Executing <color:orange><bold>LD A, B</bold></color> at <color:cyan>0x0000</color>");
AddDebuggerOutput("<italic>Cycle count: <color:yellow>4</color></italic>");
```

## Implementation Details

The markup is parsed by `DebugMessageParser` which converts the tagged text into a `FormattedDebugMessage` containing multiple `FormattedTextSegment` objects. These are then rendered by the `FormattedDebugMessageConverter` which creates MAUI `FormattedString` and `Span` objects for display in the CollectionView.

