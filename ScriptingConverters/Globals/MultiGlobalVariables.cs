using System;
using System.Globalization;
using System.Windows.Data;

namespace ScriptingConverters.Globals
{
  /// <summary>
  /// Used to pack <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/> parameters to Global scope
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class MultiGlobalVariables<T>
  {
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public T[] values;
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public Type targetType;
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public CultureInfo culture;
  }

  /// <summary>
  /// Used to pack <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/> parameters to Global scope
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="P"></typeparam>
  public class MultiGlobalVariables<T, P>
  {
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public T[] values;
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public P parameter;
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public Type targetType;
    /// <summary>
    /// <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/>
    /// </summary>
    public CultureInfo culture;
  }
}
