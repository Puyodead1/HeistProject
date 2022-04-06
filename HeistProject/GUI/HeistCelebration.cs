// Decompiled with JetBrains decompiler

using GTA;

namespace HeistProject.GUI
{
  public class HeistCelebration
  {
    private static Scaleform _sc;
    private static Scaleform _scFg;
    private static Scaleform _scBg;
    private static bool _hasInit;

    private static void Init()
    {
      HeistCelebration._sc = new Scaleform(0);
      HeistCelebration._scFg = new Scaleform(0);
      HeistCelebration._scBg = new Scaleform(0);
      HeistCelebration._sc.Load("heist_celebration");
      HeistCelebration._scFg.Load("heist_celebration_fg");
      HeistCelebration._scBg.Load("heist_celebration_bg");
    }

    public static void Show()
    {
      HeistCelebration._sc.CallFunction("CREATE_STAT_WALL", (object) 666, (object) "SUMMARY", (object) "HUD_COLOUR_BLACK", (object) 1, (object) 0);
      HeistCelebration._scFg.CallFunction("CREATE_STAT_WALL", (object) 666, (object) "SUMMARY", (object) "HUD_COLOUR_BLACK", (object) 1, (object) 0);
      HeistCelebration._scBg.CallFunction("CREATE_STAT_WALL", (object) 666, (object) "SUMMARY", (object) "HUD_COLOUR_BLACK", (object) 1, (object) 0);
      HeistCelebration._sc.CallFunction("ADD_MISSION_RESULT_TO_WALL", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "AYY_LMAO", (object) "TEST", (object) true);
      HeistCelebration._scBg.CallFunction("ADD_MISSION_RESULT_TO_WALL", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "AYY_LMAO", (object) "TEST", (object) true);
      HeistCelebration._scFg.CallFunction("ADD_MISSION_RESULT_TO_WALL", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "AYY_LMAO", (object) "TEST", (object) true);
      HeistCelebration._sc.CallFunction("ADD_STAT_TO_TABLE", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "TOP_KEK", (object) "25.5125", (object) "kekeke", (object) true, (object) true, (object) true, (object) true, (object) "HUD_COLOUR_PLATINUM");
      HeistCelebration._scBg.CallFunction("ADD_STAT_TO_TABLE", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "TOP_KEK", (object) "25.5125", (object) "kekeke", (object) true, (object) true, (object) true, (object) true, (object) "HUD_COLOUR_PLATINUM");
      HeistCelebration._scFg.CallFunction("ADD_STAT_TO_TABLE", (object) 666, (object) "SUMMARY", (object) "FMMC_RSTAR_HP", (object) "TOP_KEK", (object) "25.5125", (object) "kekeke", (object) true, (object) true, (object) true, (object) true, (object) "HUD_COLOUR_PLATINUM");
      HeistCelebration._sc.CallFunction("PAUSE", (object) "fag", (object) 100f);
      HeistCelebration._scBg.CallFunction("PAUSE", (object) "fag", (object) 100f);
      HeistCelebration._scFg.CallFunction("PAUSE", (object) "fag", (object) 100f);
      HeistCelebration._sc.CallFunction("ADD_JOB_POINTS_TO_WALL", (object) 666, (object) "SUMMARY", (object) 15, (object) "left");
      HeistCelebration._scBg.CallFunction("ADD_JOB_POINTS_TO_WALL", (object) 666, (object) "SUMMARY", (object) 15, (object) "left");
      HeistCelebration._scFg.CallFunction("ADD_JOB_POINTS_TO_WALL", (object) 666, (object) "SUMMARY", (object) 15, (object) "left");
      HeistCelebration._sc.CallFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", (object) 666, (object) 10, (object) 0, (object) 0, (object) 30, (object) 1, (object) "Fag", (object) "Homo");
      HeistCelebration._scBg.CallFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", (object) 666, (object) 10, (object) 0, (object) 0, (object) 30, (object) 1, (object) "Fag", (object) "Homo");
      HeistCelebration._scFg.CallFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", (object) 666, (object) 10, (object) 0, (object) 0, (object) 30, (object) 1, (object) "Fag", (object) "Homo");
      HeistCelebration._sc.CallFunction("PAUSE_BEFORE_PREVIOUS_LAYOUT", (object) "SUMMARY");
      HeistCelebration._scBg.CallFunction("PAUSE_BEFORE_PREVIOUS_LAYOUT", (object) "SUMMARY");
      HeistCelebration._scFg.CallFunction("PAUSE_BEFORE_PREVIOUS_LAYOUT", (object) "SUMMARY");
    }

    public static void Draw()
    {
      if (!HeistCelebration._hasInit)
      {
        HeistCelebration.Init();
        HeistCelebration._hasInit = true;
        HeistCelebration.Show();
      }
      HeistCelebration._sc.Render2D();
      HeistCelebration._scBg.Render2D();
      HeistCelebration._scFg.Render2D();
    }
  }
}
