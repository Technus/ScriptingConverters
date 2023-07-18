using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScriptingConverters
{
  public class MultiScripting : MarkupExtension
  {
    public string Script { get; set; }

    public MultiScripting()
    {
    }

    public MultiScripting(string script) =>
      Script = script;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IMultiValueConverter)new ScriptingMultiConverter() : new ScriptMultiConverter(Script);
  }

  public class MultiScripting<T, V, P> : MarkupExtension
  {
    public string Script { get; set; }

    public MultiScripting()
    {
    }

    public MultiScripting(string script) =>
      Script = script;

    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IMultiValueConverter)new ScriptingMultiConverter<T, V>() : new ScriptMultiConverter<T, V, P>(Script);
  }
}
