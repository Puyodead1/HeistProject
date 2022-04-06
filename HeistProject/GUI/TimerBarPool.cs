// Decompiled with JetBrains decompiler

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
