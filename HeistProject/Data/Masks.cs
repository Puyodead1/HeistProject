// Decompiled with JetBrains decompiler
// Type: HeistProject.Data.Masks
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeistProject.Data
{
  public static class Masks
  {
    public static Dictionary<string, Mask[]> MainDict;

    public static void Load(string path)
    {
      if (!File.Exists(path))
        return;
      Masks.MainDict = JsonConvert.DeserializeObject<MaskDictWrapper>(File.ReadAllText(path)).MainDict;
    }

    public static void Save(string path)
    {
      FileMode mode = !File.Exists(path) ? FileMode.CreateNew : FileMode.Truncate;
      using (FileStream fileStream = new FileStream(path, mode))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
          streamWriter.Write(JsonConvert.SerializeObject((object) new MaskDictWrapper()
          {
            MainDict = Masks.MainDict
          }));
      }
    }

    public static Dictionary<string, string[]> ToDictionary()
    {
      Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
      foreach (KeyValuePair<string, Mask[]> keyValuePair in Masks.MainDict)
        dictionary.Add(keyValuePair.Key, ((IEnumerable<Mask>) keyValuePair.Value).Select<Mask, string>((Func<Mask, string>) (m => m.Name)).ToArray<string>());
      return dictionary;
    }

    public static Mask GetMask(string category, string name) => ((IEnumerable<Mask>) Masks.MainDict[category]).FirstOrDefault<Mask>((Func<Mask, bool>) (m => m.Name == name));

    public static Mask GetRandomMaskFromCategory(string category) => ((IEnumerable<Mask>) Masks.MainDict[category]).ElementAt<Mask>(Util.SharedRandom.Next(Masks.MainDict[category].Length));
  }
}
