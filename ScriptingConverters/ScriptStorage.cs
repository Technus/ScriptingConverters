using System.Collections.Generic;

namespace ScriptingConverters
{
  /// <summary>
  /// Container for all build scripts
  /// </summary>
  /// <typeparam name="G">Type placeholder for Globals</typeparam>
  /// <typeparam name="T">Type of the Script variable to store</typeparam>
  public abstract class ScriptStorage<G, T>
  {
    /// <summary>
    /// Contains all built scripts easily findable by the script body key
    /// </summary>
    public static IDictionary<string, T> Scripts { get; } = new Dictionary<string, T>();
  }
}
