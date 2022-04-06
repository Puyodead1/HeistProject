// Decompiled with JetBrains decompiler
// Type: HeistProject.Data.Outfits
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
  public static class Outfits
  {
    public static Dictionary<string, Outfit[]> MainDict;

    public static Dictionary<string, string[]> ToDictionary()
    {
      Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
      foreach (KeyValuePair<string, Outfit[]> keyValuePair in Outfits.MainDict)
        dictionary.Add(keyValuePair.Key, ((IEnumerable<Outfit>) keyValuePair.Value).Select<Outfit, string>((Func<Outfit, string>) (m => m.Name)).ToArray<string>());
      return dictionary;
    }

    public static Outfit GetOutfit(string category, string name) => ((IEnumerable<Outfit>) Outfits.MainDict[category]).FirstOrDefault<Outfit>((Func<Outfit, bool>) (m => m.Name == name));

    public static void Load(string path)
    {
      if (!File.Exists(path))
        return;
      Outfits.MainDict = JsonConvert.DeserializeObject<OutfitDictWrapper>(File.ReadAllText(path)).MainDict;
    }

    public static void Save(string path)
    {
      FileMode mode = !File.Exists(path) ? FileMode.CreateNew : FileMode.Truncate;
      using (FileStream fileStream = new FileStream(path, mode))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
          streamWriter.Write(JsonConvert.SerializeObject((object) new OutfitDictWrapper()
          {
            MainDict = Outfits.MainDict
          }));
      }
    }

    public static Outfit GetRandomOutfitFromCategory(string category) => ((IEnumerable<Outfit>) Outfits.MainDict[category]).ElementAt<Outfit>(Util.SharedRandom.Next(Outfits.MainDict[category].Length));
  }
}
