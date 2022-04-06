// Decompiled with JetBrains decompiler

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
