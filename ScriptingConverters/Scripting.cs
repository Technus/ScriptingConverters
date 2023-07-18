using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScriptingConverters
{
  public class Scripting : MarkupExtension
  {
    public string Script { get; set; }

    public Scripting()
    {
    }

    public Scripting(string script) =>
      Script = script;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IValueConverter)new ScriptingConverter() : new ScriptConverter(Script);
  }

  public class Scripting<T, V, P> : MarkupExtension
  {
    public string Script { get; set; }

    public Scripting()
    {
    }

    public Scripting(string script) =>
      Script = script;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IValueConverter)new ScriptingConverter<T, V>() : new ScriptConverter<T, V, P>(Script);
  }
}
