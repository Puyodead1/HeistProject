// Decompiled with JetBrains decompiler
// Type: HeistProject.HeadLayout
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using System.Runtime.InteropServices;

namespace HeistProject
{
  [StructLayout(LayoutKind.Explicit)]
  public struct HeadLayout
  {
    [FieldOffset(0)]
    public int shapeFirstId;
    [FieldOffset(8)]
    public int shapeSecondId;
    [FieldOffset(16)]
    public int shapeThirdId;
    [FieldOffset(24)]
    public int skinFirstId;
    [FieldOffset(32)]
    public int skinSecondId;
    [FieldOffset(40)]
    public int skinThirdId;
    [FieldOffset(48)]
    public float shapeMix;
    [FieldOffset(56)]
    public float skinMix;
    [FieldOffset(64)]
    public float thirdMix;
    [FieldOffset(72)]
    public bool isParent;

    public HeadLayout(HeadLayout copyFrom)
    {
      this.shapeFirstId = copyFrom.shapeFirstId;
      this.shapeSecondId = copyFrom.shapeSecondId;
      this.shapeThirdId = copyFrom.shapeThirdId;
      this.skinFirstId = copyFrom.skinFirstId;
      this.skinSecondId = copyFrom.skinSecondId;
      this.skinThirdId = copyFrom.skinThirdId;
      this.shapeMix = copyFrom.shapeMix;
      this.skinMix = copyFrom.skinMix;
      this.thirdMix = copyFrom.thirdMix;
      this.isParent = copyFrom.isParent;
    }
  }
}
