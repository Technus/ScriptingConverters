using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class ScriptConverter : IValueConverter
  {
    public readonly (Script<object> script, ScriptRunner<object> runner) _script;

    public ScriptConverter(string script)
    {
      if (!Scripts.TryGetValue(script, out _script))
      {
        _script.script = CSharpScript.Create(ScriptUtilities.ReplaceXmlEscapes(script), ScriptUtilities.Options, typeof(GlobalVariables<object, object>), ScriptUtilities.AssemblyLoader);
        _script.runner = _script.script.CreateDelegate();
        Scripts.Add(script, _script);
      }
    }

    public static IDictionary<string, (Script<object> script, ScriptRunner<object> runner)> Scripts { get; } =
      ScriptStorage<GlobalVariables<object, object>, (Script<object> script, ScriptRunner<object> runner)>.Scripts;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      _script.runner(new GlobalVariables<object, object>
      {
        value = value,
        parameter = parameter,
        targetType = targetType,
        culture = culture,
      }).Result;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }

  public class ScriptConverter<T, V, P> : IValueConverter
  {
    public readonly (Script<V> script, ScriptRunner<V> runner) _script;

    public ScriptConverter(string script)
    {
      if (!Scripts.TryGetValue(script, out _script))
      {
        _script.script = CSharpScript.Create<V>(ScriptUtilities.ReplaceXmlEscapes(script), ScriptUtilities.Options, typeof(GlobalVariables<T, P>), ScriptUtilities.AssemblyLoader);
        _script.runner = _script.script.CreateDelegate();
        Scripts.Add(script, _script);
      }
    }

    public static IDictionary<string, (Script<V> script, ScriptRunner<V> runner)> Scripts { get; } =
      ScriptStorage<GlobalVariables<T, P>, (Script<V> script, ScriptRunner<V> runner)>.Scripts;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      _script.runner(new GlobalVariables<T, P>
      {
        value = (T)value,
        parameter = (P)parameter,
        targetType = targetType,
        culture = culture,
      }).Result;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
