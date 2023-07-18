using System;
using System.Globalization;

namespace ScriptingConverters
{
  public class GlobalVariables<T>
  {
    public T value;
    public Type targetType;
    public CultureInfo culture;
  }

  public class GlobalVariables<T, P>
  {
    public T value;
    public P parameter;
    public Type targetType;
    public CultureInfo culture;
  }
}
