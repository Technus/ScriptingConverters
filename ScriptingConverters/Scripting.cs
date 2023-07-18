using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace ScriptingConverters
{
  /// <summary>
  /// Markup extension to create <see cref="ScriptingConverter"/> or <see cref="ScriptConverter"/>
  /// </summary>
  public class Scripting : MarkupExtension
  {
    /// <summary>
    /// Script text to use while creating converter, if null then converter parameter must be used instead
    /// </summary>
    public string Script { get; set; }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingConverter"/> or <see cref="ScriptConverter"/>
    /// </summary>
    public Scripting()
    {
    }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingConverter"/> or <see cref="ScriptConverter"/>
    /// </summary>
    public Scripting(string script) =>
      Script = script;

    /// <summary>
    /// Creates <see cref="ScriptingConverter"/> or if <see cref="Script"/> is not null then <see cref="ScriptConverter"/>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IValueConverter)new ScriptingConverter() : new ScriptConverter(Script);
  }

  /// <summary>
  /// Markup extension to create <see cref="ScriptingConverter{T, V}"/> or <see cref="ScriptConverter{T, V, P}"/>
  /// </summary>
  public class Scripting<T, V, P> : MarkupExtension
  {
    /// <summary>
    /// Script text to use while creating converter, if null then converter parameter must be used instead
    /// </summary>
    public string Script { get; set; }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingConverter{T, V}"/> or <see cref="ScriptConverter{T, V, P}"/>
    /// </summary>
    public Scripting()
    {
    }

    /// <summary>
    /// Markup extension to create <see cref="ScriptingConverter{T, V}"/> or <see cref="ScriptConverter{T, V, P}"/>
    /// </summary>
    public Scripting(string script) =>
      Script = script;

    /// <summary>
    /// Creates <see cref="ScriptingConverter{T, V}"/> or if <see cref="Script"/> is not null then <see cref="ScriptConverter{T, V, P}"/>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
      Script is null ? (IValueConverter)new ScriptingConverter<T, V>() : new ScriptConverter<T, V, P>(Script);
  }
}
