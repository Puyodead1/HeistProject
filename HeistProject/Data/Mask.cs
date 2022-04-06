// Decompiled with JetBrains decompiler
// Type: HeistProject.Data.Mask
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

namespace HeistProject.Data
{
  public struct Mask
  {
    public Mask(string name, int id, int text = -1)
    {
      this.Name = name;
      this.Id = id;
      this.Texture = text;
    }

    public string Name { get; set; }

    public int Id { get; set; }

    public int Texture { get; set; }
  }
}
