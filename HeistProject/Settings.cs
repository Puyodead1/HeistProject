// Decompiled with JetBrains decompiler
// Type: HeistProject.Settings
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace HeistProject
{
  public class Settings
  {
    public Settings()
    {
      this.SaveCurrentCharacterKey = Keys.F11;
      this.LoadSavedCharacterKey = Keys.F12;
      this.SavedCharacter = (CharacterData) null;
    }

    public Keys SaveCurrentCharacterKey { get; set; }

    public Keys LoadSavedCharacterKey { get; set; }

    public CharacterData SavedCharacter { get; set; }

    public static void Save(Settings data)
    {
      string str = JsonConvert.SerializeObject((object) data);
      using (FileStream fileStream = File.OpenWrite("scripts\\SavedCharacterData.json"))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          streamWriter.Write(str);
          fileStream.SetLength(fileStream.Length);
        }
      }
    }

    public static Settings Load()
    {
      Settings data = new Settings();
      if (File.Exists("scripts\\SavedCharacterData.json"))
        data = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("scripts\\SavedCharacterData.json"));
      else
        Settings.Save(data);
      return data;
    }
  }
}
