using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class ScriptingConverter : IValueConverter
  {
    public static IDictionary<string, (Script<object> script, ScriptRunner<object> runner)> Scripts { get; } =
      ScriptStorage<GlobalVariables<object>, (Script<object> script, ScriptRunner<object> runner)>.Scripts;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!Scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create(ScriptUtilities.ReplaceXmlEscapes(param), ScriptUtilities.Options, typeof(GlobalVariables<object>), ScriptUtilities.AssemblyLoader);
        script.runner = script.script.CreateDelegate();
        Scripts.Add(param, script);
      }

      return script.runner(new GlobalVariables<object>
      {
        value = value,
        targetType = targetType,
        culture = culture,
      }).Result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  public class ScriptingConverter<T, V> : IValueConverter
  {
    public static IDictionary<string, (Script<V> script, ScriptRunner<V> runner)> Scripts { get; } =
      ScriptStorage<GlobalVariables<T>, (Script<V> script, ScriptRunner<V> runner)>.Scripts;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!Scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create<V>(ScriptUtilities.ReplaceXmlEscapes(param), ScriptUtilities.Options, typeof(GlobalVariables<T>), ScriptUtilities.AssemblyLoader);
        script.runner = script.script.CreateDelegate();
        Scripts.Add(param, script);
      }

      return script.runner(new GlobalVariables<T>
      {
        value = (T)value,
        targetType = targetType,
        culture = culture,
      }).Result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
