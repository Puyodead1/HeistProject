// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;

namespace HeistProject.GUI
{
  public static class HeistMap
  {
    private static Scaleform _sc = new Scaleform(0);
    private static int _postIts;
    private static int _highlights;
    private static int _texts;

    public static void Cleanup()
    {
      HeistMap._sc.CallFunction("REMOVE_ALL_HIGHLIGHTS");
      HeistMap._sc.CallFunction("REMOVE_ALL_POSTITS");
      HeistMap._sc.CallFunction("REMOVE_ALL_TEXT");
      HeistMap._postIts = 0;
      HeistMap._highlights = 0;
      HeistMap._texts = 0;
    }

    public static int AddPostit(int number, int posX, int posY)
    {
      ++HeistMap._postIts;
      HeistMap._sc.CallFunction("ADD_POSTIT", (object) HeistMap._postIts, (object) number, (object) posX, (object) posY);
      return HeistMap._postIts;
    }

    public static void AddPostit(int slot, int number, int posX, int posY) => HeistMap._sc.CallFunction("ADD_POSTIT", (object) slot, (object) number, (object) posX, (object) posY);

    public static void RemovePostit(int id) => HeistMap._sc.CallFunction("REMOVE_POSTIT", (object) id);

    public static int AddText(string text, int posX, int posY, int fontSize = 50, int textWidth = 800)
    {
      ++HeistMap._texts;
      HeistMap._sc.CallFunction("ADD_TEXT", (object) HeistMap._texts, (object) text, (object) posX, (object) posY, (object) 0, (object) fontSize, (object) textWidth, (object) (int) byte.MaxValue, (object) true);
      return HeistMap._texts;
    }

    public static void AddText(
      int slot,
      string text,
      int posX,
      int posY,
      int fontSize = 50,
      int textWidth = 800)
    {
      HeistMap._sc.CallFunction("ADD_TEXT", (object) slot, (object) text, (object) posX, (object) posY, (object) 0, (object) fontSize, (object) textWidth, (object) (int) byte.MaxValue, (object) true);
    }

    public static void RemoveText(int id) => HeistMap._sc.CallFunction("REMOVE_TEXT", (object) id);

    public static int AddHighlight(int xPos, int yPos)
    {
      ++HeistMap._highlights;
      HeistMap._sc.CallFunction("ADD_HIGHLIGHT", (object) HeistMap._highlights, (object) xPos, (object) yPos, (object) 900, (object) 200, (object) 200, (object) 50, (object) 100);
      return HeistMap._highlights;
    }

    public static void AddHighlight(int slot, int xPos, int yPos) => HeistMap._sc.CallFunction("ADD_HIGHLIGHT", (object) slot, (object) xPos, (object) yPos, (object) 900, (object) 200, (object) 200, (object) 50, (object) 100);

    public static void RemoveHighlight(int id) => HeistMap._sc.CallFunction("REMOVE_HIGHLIGHT", (object) id);

    public static void Draw()
    {
      if (HeistMap._sc.Handle == 0)
        HeistMap._sc.Load("heistmap_mp");
      HeistMap._sc.Render2D();
    }

    public static void Draw3D(Vector3 pos, Vector3 rot, Vector3 scale)
    {
      if (HeistMap._sc.Handle == 0)
        HeistMap._sc.Load("heistmap_mp");
      HeistMap._sc.Render3D(pos, rot, scale);
    }
  }
}
