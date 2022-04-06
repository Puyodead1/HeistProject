// Decompiled with JetBrains decompiler
// Type: HeistProject.Data.Outfit
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

namespace HeistProject.Data
{
  public struct Outfit
  {
    public static int UPPER_ID = 3;
    public static int LOWER_ID = 4;
    public static int SHOES_ID = 6;
    public static int ACCESSORY_ID = 8;
    public static int SHIRT_OVERLAY_ID = 11;

    public Outfit(
      string name,
      int upper,
      int lower,
      int shoes,
      int acc,
      int overlay,
      bool match)
    {
      this.Name = name;
      this.Upper = upper;
      this.Lower = lower;
      this.Shoes = shoes;
      this.Accessory = acc;
      this.ShirtOverlay = overlay;
      this.MatchColors = match;
      this.StaticTexture = false;
    }

    public int Upper { get; set; }

    public int Lower { get; set; }

    public int Accessory { get; set; }

    public int ShirtOverlay { get; set; }

    public int Shoes { get; set; }

    public bool MatchColors { get; set; }

    public bool StaticTexture { get; set; }

    public string Name { get; set; }
  }
}
