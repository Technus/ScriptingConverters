using System;
using System.Globalization;

namespace ScriptingConverters
{
  public class MultiGlobalVariables<T>
  {
    public T[] values;
    public Type targetType;
    public CultureInfo culture;
  }

  public class MultiGlobalVariables<T, P>
  {
    public T[] values;
    public P parameter;
    public Type targetType;
    public CultureInfo culture;
  }
}
