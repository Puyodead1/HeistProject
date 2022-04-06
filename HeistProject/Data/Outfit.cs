// Decompiled with JetBrains decompiler

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
