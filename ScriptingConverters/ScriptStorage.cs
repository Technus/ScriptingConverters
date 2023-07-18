using System.Collections.Generic;

namespace ScriptingConverters
{
  public abstract class ScriptStorage<G, T>
  {
    public static IDictionary<string, T> Scripts { get; } = new Dictionary<string, T>();
  }
}
