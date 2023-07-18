using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScriptingConverters
{
  /// <summary>
  /// Markup extension to create <see cref="ScriptingMultiConverter"/> or <see cref="ScriptMultiConverter"/>
  /// </summary>
  public class MultiScripting : MarkupExtension
  {
    /// <summary>
    /// Script text to use while creating converter, if null then converter parameter must be used instead
    /// </summary>
    public string Script { get; set; }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingMultiConverter"/> or <see cref="ScriptMultiConverter"/>
    /// </summary>
    public MultiScripting()
    {
    }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingMultiConverter"/> or <see cref="ScriptMultiConverter"/>
    /// </summary>
    public MultiScripting(string script) =>
      Script = script;

    /// <summary>
    /// Creates <see cref="ScriptingMultiConverter"/> or if <see cref="Script"/> is not null then <see cref="ScriptMultiConverter"/>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IMultiValueConverter)new ScriptingMultiConverter() : new ScriptMultiConverter(Script);
  }

  /// <summary>
  /// Markup extension to create <see cref="ScriptingMultiConverter{T, V}"/> or <see cref="ScriptMultiConverter{T, V, P}"/>
  /// </summary>
  public class MultiScripting<T, V, P> : MarkupExtension
  {
    /// <summary>
    /// Script text to use while creating converter, if null then converter parameter must be used instead
    /// </summary>
    public string Script { get; set; }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingMultiConverter{T, V}"/> or <see cref="ScriptMultiConverter{T, V, P}"/>
    /// </summary>
    public MultiScripting()
    {
    }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingMultiConverter{T, V}"/> or <see cref="ScriptMultiConverter{T, V, P}"/>
    /// </summary>
    public MultiScripting(string script) =>
      Script = script;

    /// <summary>
    /// Creates <see cref="ScriptingMultiConverter{T, V}"/> or if <see cref="Script"/> is not null then <see cref="ScriptMultiConverter{T, V, P}"/>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IMultiValueConverter)new ScriptingMultiConverter<T, V>() : new ScriptMultiConverter<T, V, P>(Script);
  }
}
