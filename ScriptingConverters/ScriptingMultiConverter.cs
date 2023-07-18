using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ScriptingConverters.Globals;

namespace ScriptingConverters
{
  /// <summary>
  /// Converter using a converter parameter Script
  /// </summary>
  public class ScriptingMultiConverter : IMultiValueConverter
  {
    /// <summary>
    /// <see cref="ScriptStorage{G, T}.Scripts"/>
    /// </summary>
    public static IDictionary<string, (Script<object> script, ScriptRunner<object> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<object>, (Script<object> script, ScriptRunner<object> runner)>.Scripts;

    /// <summary>
    /// Converts using the script provided as parameter
    /// </summary>
    /// <param name="values"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!Scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create(ScriptUtilities.ReplaceXmlEscapes(param), ScriptUtilities.Options, typeof(MultiGlobalVariables<object>), ScriptUtilities.AssemblyLoader);
        script.runner = script.script.CreateDelegate();
        Scripts.Add(param, script);
      }

      return script.runner(new MultiGlobalVariables<object>
      {
        values = values,
        targetType = targetType,
        culture = culture,
      }).Result;
    }

    /// <summary>
    /// Throws <see cref="NotImplementedException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetTypes"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  /// <summary>
  /// Converter using a converter parameter Script
  /// </summary>
  public class ScriptingMultiConverter<T, V> : IMultiValueConverter
  {
    /// <summary>
    /// <see cref="ScriptStorage{G, T}.Scripts"/>
    /// </summary>
    public static IDictionary<string, (Script<V> script, ScriptRunner<V> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<T>, (Script<V> script, ScriptRunner<V> runner)>.Scripts;

    /// <summary>
    /// Converts using the script provided as parameter
    /// </summary>
    /// <param name="values"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!Scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create<V>(ScriptUtilities.ReplaceXmlEscapes(param), ScriptUtilities.Options, typeof(MultiGlobalVariables<T>), ScriptUtilities.AssemblyLoader);
        script.runner = script.script.CreateDelegate();
        Scripts.Add(param, script);
      }

      T[] tValues;

      if (values != null)
      {
        tValues = new T[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
          tValues[i] = (T)values[i];
        }
      }
      else
      {
        tValues = null;
      }

      return script.runner(new MultiGlobalVariables<T>
      {
        values = tValues,
        targetType = targetType,
        culture = culture,
      }).Result;
    }

    /// <summary>
    /// Throws <see cref="NotImplementedException"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetTypes"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
