using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class ScriptingMultiConverter : IMultiValueConverter
  {
    public static IDictionary<string, (Script<object> script, ScriptRunner<object> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<object>, (Script<object> script, ScriptRunner<object> runner)>.Scripts;

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
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  public class ScriptingMultiConverter<T, V> : IMultiValueConverter
  {
    public static IDictionary<string, (Script<V> script, ScriptRunner<V> runner)> Scripts { get; } =
      ScriptStorage<MultiGlobalVariables<T>, (Script<V> script, ScriptRunner<V> runner)>.Scripts;

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
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
