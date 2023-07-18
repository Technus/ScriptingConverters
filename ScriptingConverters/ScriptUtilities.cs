using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace ScriptingConverters
{
  /// <summary>
  /// Helpers for script building
  /// </summary>
  public static class ScriptUtilities
  {
    /// <summary>
    /// Globally used <see cref="ScriptOptions"/> in <see cref="CSharpScript.Create{T}(string, ScriptOptions, System.Type, InteractiveAssemblyLoader)"/>
    /// </summary>
    public static ScriptOptions Options { get; set; } = ScriptOptions.Default
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

    /// <summary>
    /// Globally used <see cref="InteractiveAssemblyLoader"/> in <see cref="CSharpScript.Create{T}(string, ScriptOptions, System.Type, InteractiveAssemblyLoader)"/>
    /// </summary>
    public static InteractiveAssemblyLoader AssemblyLoader { get; set; } = new InteractiveAssemblyLoader();

    /// <summary>
    /// Replacements used in <see cref="ReplaceXmlEscapes"/>
    /// </summary>
    public static IList<(string from, string to)> Replacements { get; } = new List<(string from, string to)>
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

    private static readonly Regex _regEx = new Regex("(" + string.Join(")|(", Enumerate<string>(Replacements.Select(x => x.from))) + ")");

    /// <summary>
    /// Helper for enumeration of non generic items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IEnumerable<T> Enumerate<T>(IEnumerable collection)
    {
      foreach (var item in collection)
      {
        yield return (T)item;
      }
    }

    /// <summary>
    /// Globally used XML text escape for C# scripts
    /// </summary>
    /// <returns></returns>
    public static Func<string, string> ReplaceXmlEscapes { get; set; } = script => _regEx.Replace(script, match =>
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
          return Replacements[id].to;
        }

        id++;
      }
      return match.Value;
    });
  }
}
