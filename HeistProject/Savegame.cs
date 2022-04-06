// Decompiled with JetBrains decompiler

using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HeistProject
{
  public class Savegame
  {
    public Savegame()
    {
      this.FinishedHeists = new List<string>();
      this.FinishedSetups = new List<string>();
      this.StartedHeists = new List<string>();
      this.DirtyMoney = 5000;
      this.MoneyTransferred = 5000;
    }

    public int DirtyMoney { get; set; }

    public int MoneyTransferred { get; set; }

    public int MoneyWithdrawn { get; set; }

    public string CurrentHeist { get; set; }

    public List<string> FinishedSetups { get; set; }

    public List<string> FinishedHeists { get; set; }

    public List<string> StartedHeists { get; set; }

    public static void SaveProgress(Savegame game, string path)
    {
      using (FileStream fileStream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.CreateNew))
      {
        new XmlSerializer(typeof (Savegame)).Serialize((Stream) fileStream, (object) game);
        fileStream.SetLength(fileStream.Length);
      }
    }

    public static Savegame LoadProgress(string path)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (Savegame));
      if (!File.Exists(path))
        return new Savegame();
      using (FileStream fileStream = File.OpenRead(path))
        return (Savegame) xmlSerializer.Deserialize((Stream) fileStream);
    }
  }
}
