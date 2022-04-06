// Decompiled with JetBrains decompiler

using System;
using System.IO;

namespace HeistProject
{
  public static class TempFileHosting
  {
    public const string FolderName = "HeistProject";

    public static void Clean()
    {
      string path = Path.GetTempPath() + Path.DirectorySeparatorChar.ToString() + "HeistProject";
      if (Directory.Exists(path))
      {
        foreach (string file in Directory.GetFiles(path))
        {
          try
          {
            File.Delete(file);
          }
          catch (Exception ex)
          {
          }
        }
      }
      else
        Directory.CreateDirectory(path);
    }

    public static string ConvertFilenameToTemp(string fn) => Path.GetTempPath() + Path.DirectorySeparatorChar.ToString() + "HeistProject" + Path.DirectorySeparatorChar.ToString() + fn;

    public static string GetRandomTempFilename()
    {
      string empty = string.Empty;
      string str;
      string[] strArray;
      do
      {
        str = "";
        for (int index = 0; index < 20; ++index)
        {
          int num = Util.SharedRandom.Next(65, 117);
          if (num >= 91)
            num += 6;
          str += ((char) num).ToString();
        }
        strArray = new string[5]
        {
          Path.GetTempPath(),
          Path.DirectorySeparatorChar.ToString(),
          "HeistProject",
          Path.DirectorySeparatorChar.ToString(),
          str
        };
      }
      while (File.Exists(string.Concat(strArray)));
      return Path.GetFullPath(Path.GetTempPath() + Path.DirectorySeparatorChar.ToString() + "HeistProject" + Path.DirectorySeparatorChar.ToString() + str);
    }
  }
}
