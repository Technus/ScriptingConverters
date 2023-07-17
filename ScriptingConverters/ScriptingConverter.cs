using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class GlobalVariables<T>
  {
    public T value;
    public Type targetType;
    public CultureInfo culture;
  }

  public class ScriptingConverter : BaseScripting<(Script<object> script, ScriptRunner<object> runner)>, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create(Sanitize(param), options, typeof(GlobalVariables<object>), assemblyLoader);
        script.runner = script.script.CreateDelegate();
        scripts.Add(param, script);
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

  public class ScriptingConverter<T, V> : BaseScripting<(Script<V> script, ScriptRunner<V> runner)>, IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create<V>(Sanitize(param), options, typeof(GlobalVariables<T>), assemblyLoader);
        script.runner = script.script.CreateDelegate();
        scripts.Add(param, script);
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
