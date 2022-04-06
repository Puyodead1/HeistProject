// Decompiled with JetBrains decompiler
// Type: InstaScript.PosInfo
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

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
