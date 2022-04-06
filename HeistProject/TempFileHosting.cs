// Decompiled with JetBrains decompiler
// Type: HeistProject.TempFileHosting
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

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
