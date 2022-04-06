// Decompiled with JetBrains decompiler
// Type: HeistProject.ScriptHandler
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using Ionic.Zip;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HeistProject
{
  public class ScriptHandler
  {
    private const string _mainFolder = "scripts\\HeistProject";
    public List<HeistDefinition> Scripts;

    public ScriptHandler()
    {
      this.Scripts = new List<HeistDefinition>();
      this.LoadScripts();
    }

    public void LoadScripts()
    {
      ScriptHandler.Log("LOOKING FOR HEIST FILES " + (object) DateTime.Now);
      TempFileHosting.Clean();
      foreach (string file in Directory.GetFiles("scripts\\HeistProject", "*.heist"))
      {
        Dictionary<string, MemoryStream> dictionary = new Dictionary<string, MemoryStream>();
        try
        {
          dictionary = ScriptHandler.UnzipFile(file);
          HeistDefinition heistDefinition = dictionary.ContainsKey("assembly.json") ? JsonConvert.DeserializeObject<HeistDefinition>(ScriptHandler.GetString(dictionary["assembly.json"])) : throw new FormatException("File assembly.json was not found.");
          if (heistDefinition == null)
            throw new FormatException("Json was not deserialized correctly.");
          if (!dictionary.ContainsKey(heistDefinition.ScriptFilename))
            throw new FormatException("Specified script filename not found.");
          heistDefinition.SetLogic(this.CompileScript(ScriptHandler.GetString(dictionary[heistDefinition.ScriptFilename]), heistDefinition.ScriptType, heistDefinition.ScriptFilename.EndsWith(".vb")) ?? throw new FormatException("Main heist script was evaluated to null."));
          foreach (SetupDefinition setup in heistDefinition.Setups)
          {
            ScriptedLogic logic = this.CompileScript(ScriptHandler.GetString(dictionary[setup.ScriptFilename]), setup.ScriptType, setup.ScriptFilename.EndsWith(".vb"));
            if (logic == null)
              throw new FormatException("Setup script " + setup.Name + " was null.");
            setup.Parent = heistDefinition;
            setup.SetLogic(logic);
          }
          heistDefinition.AssetTranslation = new Dictionary<string, string>();
          if (heistDefinition.Assets != null)
          {
            foreach (string asset in heistDefinition.Assets)
            {
              if (dictionary.ContainsKey(asset))
              {
                string randomTempFilename = TempFileHosting.GetRandomTempFilename();
                using (FileStream destination = new FileStream(randomTempFilename, FileMode.CreateNew))
                {
                  dictionary[asset].Seek(0L, SeekOrigin.Begin);
                  dictionary[asset].CopyTo((Stream) destination);
                }
                heistDefinition.AssetTranslation.Add(asset, randomTempFilename);
              }
            }
          }
          this.Scripts.Add(heistDefinition);
        }
        catch (Exception ex)
        {
          UI.Notify("Error while loading assembly " + file);
          UI.Notify(ex.Message);
          UI.Notify(ex.StackTrace);
          ScriptHandler.Log(ex.Message);
          ScriptHandler.Log(ex.StackTrace);
          ScriptHandler.Log(ex.Source);
        }
        finally
        {
          foreach (KeyValuePair<string, MemoryStream> keyValuePair in dictionary)
            keyValuePair.Value.Dispose();
        }
      }
    }

    public static string GetString(MemoryStream stream)
    {
      using (StreamReader streamReader = new StreamReader((Stream) stream, Encoding.UTF8, true, (int) stream.Length, true))
      {
        string end = streamReader.ReadToEnd();
        stream.Flush();
        stream.Position = 0L;
        return end;
      }
    }

    private ScriptedLogic CompileScript(
      string script,
      string typeName,
      bool visualBasic = false)
    {
      CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
      VBCodeProvider vbCodeProvider = new VBCodeProvider();
      CompilerParameters options = new CompilerParameters();
      options.ReferencedAssemblies.Add("System.Drawing.dll");
      options.ReferencedAssemblies.Add("System.Windows.Forms.dll");
      options.ReferencedAssemblies.Add("System.IO.dll");
      options.ReferencedAssemblies.Add("System.Linq.dll");
      options.ReferencedAssemblies.Add("System.Core.dll");
      options.ReferencedAssemblies.Add("ScriptHookVDotNet2.dll");
      options.ReferencedAssemblies.Add("scripts\\HeistProject.dll");
      options.ReferencedAssemblies.Add("scripts\\NativeUI.dll");
      options.GenerateInMemory = true;
      options.GenerateExecutable = false;
      try
      {
        CompilerResults compilerResults1;
        if (visualBasic)
          compilerResults1 = vbCodeProvider.CompileAssemblyFromSource(options, script);
        else
          compilerResults1 = csharpCodeProvider.CompileAssemblyFromSource(options, script);
        CompilerResults compilerResults2 = compilerResults1;
        if (compilerResults2.Errors.HasErrors)
        {
          StringBuilder stringBuilder = new StringBuilder();
          bool flag = true;
          foreach (CompilerError error in (CollectionBase) compilerResults2.Errors)
          {
            stringBuilder.AppendLine(string.Format("{3} ({0}) at {2}: {1}", (object) error.ErrorNumber, (object) error.ErrorText, (object) error.Line, error.IsWarning ? (object) "Warning" : (object) "Error"));
            flag = flag && error.IsWarning;
          }
          UI.Notify("Error/warning while compiling script " + script);
          ScriptHandler.Log(stringBuilder.ToString());
          if (!flag)
            return (ScriptedLogic) null;
        }
        return this.InstantiateScripts(compilerResults2.CompiledAssembly, typeName);
      }
      catch (Exception ex)
      {
        UI.Notify("Error while compiling assembly " + script);
        UI.Notify(ex.Message);
        UI.Notify(ex.StackTrace);
        ScriptHandler.Log(ex.Message);
        ScriptHandler.Log(ex.StackTrace);
        ScriptHandler.Log(ex.Source);
        return (ScriptedLogic) null;
      }
    }

    private ScriptedLogic InstantiateScripts(Assembly target, string name)
    {
      Type type = ((IEnumerable<Type>) target.GetTypes()).SingleOrDefault<Type>((Func<Type, bool>) (t => t.Name == name));
      return !(type == (Type) null) ? (ScriptedLogic) Activator.CreateInstance(type) : throw new KeyNotFoundException("Target type " + name + " was not found in assembly " + ((IEnumerable<Type>) target.GetTypes()).First<Type>().Namespace);
    }

    private static Dictionary<string, MemoryStream> UnzipFile(string filename)
    {
      Dictionary<string, MemoryStream> dictionary = new Dictionary<string, MemoryStream>();
      using (ZipFile zipFile = ZipFile.Read(filename))
      {
        foreach (ZipEntry zipEntry in zipFile)
        {
          MemoryStream memoryStream = new MemoryStream();
          zipEntry.Extract((Stream) memoryStream);
          memoryStream.Position = 0L;
          dictionary.Add(zipEntry.FileName, memoryStream);
        }
      }
      return dictionary;
    }

    public static void Log(string msg) => File.AppendAllText("scripts\\HeistProject\\scriptload.log", msg + "\r\n");
  }
}
