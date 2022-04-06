// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.TimerBarPool
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using System.Collections.Generic;

namespace HeistProject.GUI
{
  public static class TimerBarPool
  {
    private static List<TimerBarBase> _bars = new List<TimerBarBase>();

    public static void Add(TimerBarBase timer) => TimerBarPool._bars.Add(timer);

    public static void Remove(TimerBarBase timer) => TimerBarPool._bars.Remove(timer);

    public static void Draw()
    {
      for (int index = 0; index < TimerBarPool._bars.Count; ++index)
        TimerBarPool._bars[index].Draw(index * 10);
    }
  }
}
