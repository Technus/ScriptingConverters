using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ScriptingConverters
{
  public class MultiGlobalVariables<T>
  {
    public T[] values;
    public Type targetType;
    public CultureInfo culture;
  }

  public class ScriptingMultiConverter : BaseScripting<(Script<object> script, ScriptRunner<object> runner)>, IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create(Sanitize(param), options, typeof(MultiGlobalVariables<object>), assemblyLoader);
        script.runner = script.script.CreateDelegate();
        scripts.Add(param, script);
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

  public class ScriptingMultiConverter<T, V> : BaseScripting<(Script<V> script, ScriptRunner<V> runner)>, IMultiValueConverter
  {

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      var param = parameter?.ToString() ?? string.Empty;

      if (!scripts.TryGetValue(param, out var script))
      {
        script.script = CSharpScript.Create<V>(Sanitize(param), options, typeof(MultiGlobalVariables<T>), assemblyLoader);
        script.runner = script.script.CreateDelegate();
        scripts.Add(param, script);
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
