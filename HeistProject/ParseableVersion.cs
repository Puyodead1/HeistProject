// Decompiled with JetBrains decompiler
// Type: HeistProject.ParseableVersion
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HeistProject
{
  public struct ParseableVersion : IComparable<ParseableVersion>
  {
    public int Major { get; set; }

    public int Minor { get; set; }

    public int Revision { get; set; }

    public int Build { get; set; }

    public ParseableVersion(int major, int minor, int rev, int build)
    {
      this.Major = major;
      this.Minor = minor;
      this.Revision = rev;
      this.Build = build;
    }

    public override string ToString() => this.Major.ToString() + "." + (object) this.Minor + "." + (object) this.Build + "." + (object) this.Revision;

    public int CompareTo(ParseableVersion right) => this.CreateComparableInteger().CompareTo(right.CreateComparableInteger());

    public long CreateComparableInteger() => (long) ((double) this.Revision + (double) this.Build * Math.Pow(10.0, 4.0) + (double) this.Minor * Math.Pow(10.0, 8.0) + (double) this.Major * Math.Pow(10.0, 12.0));

    public static bool operator >(ParseableVersion left, ParseableVersion right) => left.CreateComparableInteger() > right.CreateComparableInteger();

    public static bool operator <(ParseableVersion left, ParseableVersion right) => left.CreateComparableInteger() < right.CreateComparableInteger();

    public static ParseableVersion Parse(string version)
    {
      string[] strArray = version.Split('.');
      if (strArray.Length < 2)
        throw new ArgumentException("Argument version is in wrong format");
      ParseableVersion parseableVersion = new ParseableVersion();
      parseableVersion.Major = int.Parse(strArray[0]);
      parseableVersion.Minor = int.Parse(strArray[1]);
      if (strArray.Length >= 3)
        parseableVersion.Build = int.Parse(strArray[2]);
      if (strArray.Length >= 4)
        parseableVersion.Revision = int.Parse(strArray[3]);
      return parseableVersion;
    }

    public static ParseableVersion FromWebsite(string modName, string category = "scripts")
    {
      using (ParseableVersion.ImpatientWebClient impatientWebClient = new ParseableVersion.ImpatientWebClient())
        return ParseableVersion.Parse(Regex.Match(impatientWebClient.DownloadString("https://www.gta5-mods.com/" + category + "/" + modName), "<span class=\\\"version\\\">(.+)</span>").Groups[1].Captures[0].Value);
    }

    public static ParseableVersion FromAssembly()
    {
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      return new ParseableVersion()
      {
        Major = version.Major,
        Minor = version.Minor,
        Revision = version.Revision,
        Build = version.Build
      };
    }

    public class ImpatientWebClient : WebClient
    {
      protected override WebRequest GetWebRequest(Uri address)
      {
        WebRequest webRequest = base.GetWebRequest(address);
        if (webRequest != null)
          webRequest.Timeout = 4000;
        return webRequest;
      }
    }
  }
}
