// Decompiled with JetBrains decompiler
// Type: HeistProject.Data.TexturedOutfit
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

namespace HeistProject.Data
{
  public struct TexturedOutfit
  {
    public TexturedOutfit(
      Outfit takeFrom,
      int upperText,
      int lowerText,
      int shoesText,
      int accText,
      int overlayText)
    {
      this.Name = takeFrom.Name;
      this.Upper = takeFrom.Upper;
      this.Lower = takeFrom.Lower;
      this.Shoes = takeFrom.Shoes;
      this.Accessory = takeFrom.Accessory;
      this.ShirtOverlay = takeFrom.ShirtOverlay;
      this.UpperTexture = upperText;
      this.LowerTexture = lowerText;
      this.ShoesTexture = shoesText;
      this.AccessoryTexture = accText;
      this.ShirtOverlayTexture = overlayText;
      this.MatchColors = takeFrom.MatchColors;
    }

    public int Upper { get; set; }

    public int Lower { get; set; }

    public int Accessory { get; set; }

    public int ShirtOverlay { get; set; }

    public int Shoes { get; set; }

    public int UpperTexture { get; set; }

    public int LowerTexture { get; set; }

    public int AccessoryTexture { get; set; }

    public int ShirtOverlayTexture { get; set; }

    public int ShoesTexture { get; set; }

    public bool MatchColors { get; set; }

    public string Name { get; set; }
  }
}
