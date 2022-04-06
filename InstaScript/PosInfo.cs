// Decompiled with JetBrains decompiler

using GTA.Math;

namespace InstaScript
{
  public struct PosInfo
  {
    public Vector3 Position;
    public float Heading;

    public PosInfo(float x, float y, float z, float h)
    {
      this.Position = new Vector3(x, y, z);
      this.Heading = h;
    }
  }
}
