using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class ScriptMultiConverter : IMultiValueConverter
  {
    public readonly (Script<object> script, ScriptRunner<object> runner) _script;

    public ScriptMultiConverter(string script)
    {
      if (!Scripts.TryGetValue(script, out _script))
      {
        _script.script = CSharpScript.Create(ScriptUtilities.ReplaceXmlEscapes(script), ScriptUtilities.Options, typeof(MultiGlobalVariables<object, object>), ScriptUtilities.AssemblyLoader);
        _script.runner = _script.script.CreateDelegate();
        Scripts.Add(script, _script);
      }
    }

    public static IDictionary<string, (Script<object> script, ScriptRunner<object> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<object, object>, (Script<object> script, ScriptRunner<object> runner)>.Scripts;

    public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture) =>
      _script.runner(new MultiGlobalVariables<object, object>
      {
        values = value,
        parameter = parameter,
        targetType = targetType,
        culture = culture,
      }).Result;

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  public class ScriptMultiConverter<T, V, P> : IMultiValueConverter
  {
    public readonly (Script<V> script, ScriptRunner<V> runner) _script;

    public ScriptMultiConverter(string script)
    {
      if (!Scripts.TryGetValue(script, out _script))
      {
        _script.script = CSharpScript.Create<V>(ScriptUtilities.ReplaceXmlEscapes(script), ScriptUtilities.Options, typeof(MultiGlobalVariables<T, P>), ScriptUtilities.AssemblyLoader);
        _script.runner = _script.script.CreateDelegate();
        Scripts.Add(script, _script);
      }
    }

    public static IDictionary<string, (Script<V> script, ScriptRunner<V> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<T, P>, (Script<V> script, ScriptRunner<V> runner)>.Scripts;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
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

      return _script.runner(new MultiGlobalVariables<T, P>
      {
        values = tValues,
        parameter = (P)parameter,
        targetType = targetType,
        culture = culture,
      }).Result;
    }

    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
