using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace ScriptingConverters
{
  public abstract class BaseScripting
  {
    protected static ScriptOptions options = ScriptOptions.Default
      .AddReferences("System.Linq")
      .AddReferences("System.Linq.Expressions")
      .AddReferences("System.Collections.Generic")
      .AddReferences("System.Text")
      .AddReferences("System")
      .AddReferences("System.Xml")
      .AddReferences("System.Json")
      .AddReferences("System.Data")
      .AddReferences("System.ComponentModel")
      .AddReferences("System.Windows")
      .AddReferences("System.Windows.Controls")
      .AddReferences("System.Windows.Controls.Primitives")
      .AddReferences("System.Windows.Controls.DataVisualization")
      .AddReferences("System.Windows.Controls.DataVisualization.Charting")
      .AddReferences("System.Windows.Data")
      .AddReferences("System.Windows.Media")
      .AddReferences("System.Windows.Media.Imaging")
      .AddReferences("System.Windows.Media.Animations")
      .AddReferences("System.Windows.Media.Effects")
      .AddReferences("System.Windows.Media.Media3D")
      .AddReferences("System.Windows.Media.TextFormatting")
      .AddReferences("System.Windows.Shapes")
      .AddReferences("System.Windows.Input")
      .AddReferences("System.Windows.Navigation")
      .AddReferences("System.Windows.Threading")
      .AddReferences("System.Windows.Resources")
      .AddReferences("System.Windows.Markup")
      .AddReferences("System.Windows.Printing")
      .AddReferences("System.Windows.Interop")
      .AddReferences("System.Windows.Documents")
      .AddReferences("System.Windows.Ink")
      .AddReferences("System.Windows.Shell")
      .AddImports("System.Math")
      .AddReferences("System.Numerics")
      .AddImports("System.Numerics.Complex");

    protected static InteractiveAssemblyLoader assemblyLoader = new InteractiveAssemblyLoader();

    private static readonly IList<(string from, string to)> replaces = new List<(string from, string to)>
    {
      ( @"`_"   ," "  ),
      ( @"`\]"   ,"}" ),
      ( @"`\["   ,"{" ),
      ( @"`\)"   ,">" ),
      ( @"`\("   ,"<" ),
      ( @"`#"   ,"=" ),
      ( @"`@"   ,"&" ),
      ( @"`\*"   ,"'" ),
      ( @"`~"   ,"`" ),
      ( @"`[`\^]"  ,"\"" ),
      ( @"`\.?"   ,"," ),
    };
    private static readonly Regex regEx = new Regex("(" + string.Join(")|(", Enumerate<string>(replaces.Select(x => x.from))) + ")");

    private static IEnumerable<T> Enumerate<T>(IEnumerable collection)
    {
      foreach (var item in collection)
      {
        yield return (T)item;
      }
    }

    protected static string Sanitize(string script) => regEx.Replace(script, match =>
    {
      var enumerator = Enumerate<Group>(match.Groups).GetEnumerator();
      if (!enumerator.MoveNext() || !enumerator.Current.Success)
      {
        return match.Value;
      }
      int id = 0;
      while (enumerator.MoveNext())
      {
        if (enumerator.Current.Success)
        {
          return replaces[id].to;
        }

        id++;
      }
      return match.Value;
    });
  }

  public abstract class BaseScripting<T> : BaseScripting
  {
    protected static readonly IDictionary<string, T> scripts = new Dictionary<string, T>();
  }
}
