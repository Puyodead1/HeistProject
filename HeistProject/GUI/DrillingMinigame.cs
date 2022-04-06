// Decompiled with JetBrains decompiler

using GTA;
using GTA.Native;
using System;

namespace HeistProject.GUI
{
  public static class DrillingMinigame
  {
    public static Action OnDrillingComplete;
    public static float _tempDecay = 0.1f;
    public static float _tempGain = 0.01f;
    public static float _drillGain = 0.005f;
    public static bool hasInit;
    public static bool hasClosed;
    public static int f2;
    public static float f5;
    public static int f6;
    public static float f8;
    public static float f7;
    public static float f9;
    public static float f10;
    public static float f11;
    public static float f12;
    public static float f13;
    public static float f14;
    public static float f15;
    public static float f16;
    public static float f17;
    public static float fA;
    public static float fB;
    public static float fC;
    public static float fD;
    public static float fE;
    public static float fF;
    public static bool bit0;
    public static bool bit1;
    public static bool bit2;
    public static bool bit3;
    public static bool bit4;
    public static bool bit5;
    public static Scaleform _sc = new Scaleform(0);
    public static float _temperature;
    public static float _speed;
    public static float _holeDepth;
    public static float _drillPosition;

    public static float Temperature
    {
      get => DrillingMinigame._temperature;
      set
      {
        DrillingMinigame._temperature = value;
        DrillingMinigame.SetTemperature(value);
      }
    }

    public static float Speed
    {
      get => DrillingMinigame._speed;
      set
      {
        DrillingMinigame._speed = value;
        DrillingMinigame.SetSpeed(value);
      }
    }

    public static float HoleDepth
    {
      get => DrillingMinigame._holeDepth;
      set
      {
        DrillingMinigame._holeDepth = value;
        DrillingMinigame.SetHoleDepth(value);
      }
    }

    public static float DrillPosition
    {
      get => DrillingMinigame._drillPosition;
      set
      {
        DrillingMinigame._drillPosition = value;
        DrillingMinigame.SetDrillPos(value);
      }
    }

    public static int BrokenPins { get; set; }

    public static bool Visible { get; set; }

    public static void StartNew()
    {
      if (DrillingMinigame._sc.Handle == 0)
        DrillingMinigame._sc.Load("drilling");
      DrillingMinigame.Temperature = 0.0f;
      DrillingMinigame.Speed = 0.0f;
      DrillingMinigame.HoleDepth = 0.0f;
      DrillingMinigame.DrillPosition = 0.0f;
      DrillingMinigame.f2 = 0;
      Function.Call(Hash.START_AUDIO_SCENE, "DLC_HEIST_MINIGAME_FLEECA_DRILLING_SCENE");
    }

    public static void Update()
    {
      if (!DrillingMinigame.Visible)
        return;
      if (DrillingMinigame._sc.Handle == 0)
        DrillingMinigame._sc.Load("drilling");
      switch (DrillingMinigame.f2)
      {
        case 0:
          DrillingMinigame.f8 = 0.0f;
          DrillingMinigame.f7 = 0.0f;
          DrillingMinigame.f9 = 0.0f;
          DrillingMinigame.f2 = 1;
          DrillingMinigame.f5 = 0.0f;
          DrillingMinigame.f6 = 0;
          DrillingMinigame.f8 = 0.0f;
          DrillingMinigame.f7 = 0.0f;
          DrillingMinigame.f9 = 0.0f;
          DrillingMinigame.f10 = 0.0f;
          DrillingMinigame.f11 = 0.0f;
          DrillingMinigame.f12 = 0.0f;
          DrillingMinigame.f13 = 0.0f;
          DrillingMinigame.f14 = 0.0f;
          DrillingMinigame.f15 = 0.0f;
          DrillingMinigame.f16 = 0.0f;
          DrillingMinigame.f17 = 0.0f;
          if (!DrillingMinigame.bit2)
          {
            if (!DrillingMinigame.bit5)
            {
              DrillingMinigame.fB = 0.08f;
              DrillingMinigame.f6 = 0;
            }
            DrillingMinigame.fF = 0.0f;
          }
          DrillingMinigame.fC = 0.25f;
          DrillingMinigame.fE = DrillingMinigame.fB;
          DrillingMinigame.fD = DrillingMinigame.fE - 0.25f;
          if ((double) DrillingMinigame.fD < 0.0)
            DrillingMinigame.fD = 0.0f;
          DrillingMinigame.bit1 = false;
          DrillingMinigame.bit2 = false;
          DrillingMinigame.bit5 = false;
          if (!DrillingMinigame.hasInit)
          {
            Function.Call(Hash._0x3D42B92563939375, "MP DRILL MINIGAME");
            DrillingMinigame.hasInit = true;
            break;
          }
          break;
        case 1:
          DrillingMinigame.bit0 = false;
          if (DrillingMinigame.bit2 || Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
          {
            DrillingMinigame.f8 = 0.0f;
            DrillingMinigame.f7 = 0.0f;
            DrillingMinigame.f9 = 0.0f;
          }
          else
          {
            DrillingMinigame.f8 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 218);
            if (Function.Call<bool>(Hash._0xA571D46727E2B718, 0))
            {
              if (Function.Call<bool>(Hash._0xE1615EC03B3BB4FD))
                DrillingMinigame.f7 += (float) ((double) Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 219) * 0.25);
              else
                DrillingMinigame.f7 -= (float) ((double) Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 219) * 0.25);
            }
            else
              DrillingMinigame.f7 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 196);
            DrillingMinigame.f9 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 196);
          }
          if ((double) DrillingMinigame.f7 < 0.0)
            DrillingMinigame.f7 = 0.0f;
          if (!Function.Call<bool>(Hash._0xA571D46727E2B718, 0))
            DrillingMinigame.f7 /= 0.82f;
          if ((double) DrillingMinigame.f7 > 1.0)
            DrillingMinigame.f7 = 1f;
          if (!Function.Call<bool>(Hash._0xA571D46727E2B718, 0) && (double) DrillingMinigame.f9 < 0.0299999993294477)
            DrillingMinigame.f9 = 0.0f;
          DrillingMinigame.fA -= (float) (0.400000005960464 + 0.200000002980232 * (1.0 - (double) DrillingMinigame.f9)) * Function.Call<float>(Hash.TIMESTEP);
          if ((double) DrillingMinigame.fA < 0.0)
            DrillingMinigame.fA = 0.0f;
          DrillingMinigame.f9 *= (float) (1.0 + (double) DrillingMinigame.fA * 0.5);
          if ((double) DrillingMinigame.f9 > 1.0)
            DrillingMinigame.f9 = 1f;
          DrillingMinigame.fC = (float) (0.25 - 0.0999999940395355 * (double) DrillingMinigame.f9);
          float num = (float) ((1.0 - (double) DrillingMinigame.fB) * 0.25);
          DrillingMinigame.fC += num;
          DrillingMinigame.f10 = (float) (0.0 + 0.025000000372529 * (double) DrillingMinigame.f9);
          DrillingMinigame.fC /= (float) (1.0 + (double) DrillingMinigame.fA * 6.0);
          DrillingMinigame.fD = DrillingMinigame.fB;
          DrillingMinigame.fE = DrillingMinigame.fD + DrillingMinigame.fC;
          if ((double) DrillingMinigame.fD < 0.0)
            DrillingMinigame.fD = 0.0f;
          if ((double) DrillingMinigame.fE > 1.0)
            DrillingMinigame.fE = 1f;
          if (!DrillingMinigame.bit2)
          {
            if ((double) DrillingMinigame.f9 > 0.0)
            {
              if ((double) DrillingMinigame.f7 < (double) DrillingMinigame.fD)
                Function.Call(Hash.SET_PAD_SHAKE, 2, 200, (15f + (float) Math.Round((double) DrillingMinigame.f9 * 45.0) + (float) Math.Round((double) DrillingMinigame.fA * 45.0)));
              else if ((double) DrillingMinigame.f7 > (double) DrillingMinigame.fE)
              {
                DrillingMinigame.fF += (float) (0.349999994039536 + (double) DrillingMinigame.f9 * 0.200000002980232) * Function.Call<float>(Hash.TIMESTEP);
                Function.Call(Hash.SET_PAD_SHAKE, 2, 200, 200);
              }
              else
              {
                DrillingMinigame.fB += DrillingMinigame.f10 * Function.Call<float>(Hash.TIMESTEP);
                if ((double) DrillingMinigame.fB >= 1.0)
                  DrillingMinigame.fB = 1f;
                DrillingMinigame.bit0 = true;
                Function.Call(Hash.SET_PAD_SHAKE, 2, 200, (30f + (float) Math.Round((double) DrillingMinigame.f9 * 60.0) + (float) Math.Round((double) DrillingMinigame.fA * 60.0)));
              }
            }
            else if ((double) DrillingMinigame.f7 > (double) DrillingMinigame.fE)
              Function.Call(Hash.SET_PAD_SHAKE, 2, 200, 30);
          }
          else
            DrillingMinigame.f12 = 0.0f;
          if (DrillingMinigame.bit2)
            DrillingMinigame.fF -= 0.16f * Function.Call<float>(Hash.TIMESTEP);
          else if ((double) DrillingMinigame.f9 > 0.0)
            DrillingMinigame.fF -= 0.08f * Function.Call<float>(Hash.TIMESTEP);
          else
            DrillingMinigame.fF -= 0.09999999f * Function.Call<float>(Hash.TIMESTEP);
          if ((double) DrillingMinigame.fF > 1.0)
            DrillingMinigame.fF = 1f;
          else if ((double) DrillingMinigame.fF < 0.0)
            DrillingMinigame.fF = 0.0f;
          DrillingMinigame.f15 += (float) (((double) DrillingMinigame.f7 - (double) DrillingMinigame.f15) * 0.5);
          if ((double) DrillingMinigame.f15 > (double) DrillingMinigame.fB)
            DrillingMinigame.f15 = DrillingMinigame.fB;
          if ((double) DrillingMinigame.f7 > (double) DrillingMinigame.fE)
          {
            if ((double) Function.Call<float>(Hash.ABSF, (DrillingMinigame.f17 - DrillingMinigame.f16)) < 1.0 / 500.0)
              DrillingMinigame.f16 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0.04f, 0.0f);
          }
          else
            DrillingMinigame.f17 = 0.0f;
          DrillingMinigame.f16 = (float) (0.0 * (double) DrillingMinigame.f16 + 1.0 * (double) DrillingMinigame.f17);
          DrillingMinigame.f11 = DrillingMinigame.subroutine185d(DrillingMinigame.f11, DrillingMinigame.f8, 0.2f);
          DrillingMinigame.f12 = DrillingMinigame.subroutine185d(DrillingMinigame.f12, DrillingMinigame.f15, 0.1f);
          DrillingMinigame.f14 = DrillingMinigame.subroutine185d(DrillingMinigame.f14, DrillingMinigame.f9, 0.4f);
          if ((double) DrillingMinigame.fB >= 0.774999976158142 && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
          {
            if ((double) DrillingMinigame.fB - 0.774999976158142 < 0.00999999977648258)
              DrillingMinigame.fB = 0.785f;
            if ((double) Function.Call<int>(Hash.GET_GAME_TIMER) - (double) DrillingMinigame.f5 > 500.0)
              DrillingMinigame.f2 = 2;
          }
          else
            DrillingMinigame.f5 = (float) Function.Call<int>(Hash.GET_GAME_TIMER);
          switch (DrillingMinigame.f6)
          {
            case 0:
              if ((double) DrillingMinigame.fB > 0.324999988079071)
              {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
                if ((double) DrillingMinigame.fB < 0.330000013113022)
                  DrillingMinigame.fB = 0.33f;
                DrillingMinigame.fB += 0.01f;
                DrillingMinigame.fA = 1f;
                ++DrillingMinigame.f6;
                break;
              }
              break;
            case 1:
              DrillingMinigame.f13 += (float) ((0.0599999986588955 - (double) DrillingMinigame.f13) * 0.5);
              if ((double) DrillingMinigame.fB > 0.474999994039536)
              {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
                if ((double) DrillingMinigame.fB < 0.479999989271164)
                  DrillingMinigame.fB = 0.48f;
                DrillingMinigame.fB += 0.01f;
                DrillingMinigame.fA = 1f;
                ++DrillingMinigame.f6;
                break;
              }
              break;
            case 2:
              DrillingMinigame.f13 += (float) ((0.119999997317791 - (double) DrillingMinigame.f13) * 0.5);
              if ((double) DrillingMinigame.fB > 0.625)
              {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
                if ((double) DrillingMinigame.fB < 0.629999995231628)
                  DrillingMinigame.fB = 0.63f;
                DrillingMinigame.fB += 0.01f;
                DrillingMinigame.fA = 1f;
                ++DrillingMinigame.f6;
                break;
              }
              break;
            case 3:
              DrillingMinigame.f13 += (float) ((0.180000007152557 - (double) DrillingMinigame.f13) * 0.5);
              if ((double) DrillingMinigame.fB > 0.774999976158142)
              {
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
                if ((double) DrillingMinigame.fB < 0.479999989271164)
                  DrillingMinigame.fB = 0.48f;
                DrillingMinigame.fB += 0.01f;
                DrillingMinigame.fA = 1f;
                ++DrillingMinigame.f6;
                Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Camera_Destroy", "DLC_HEIST_FLEECA_SOUNDSET", 1);
                Action drillingComplete = DrillingMinigame.OnDrillingComplete;
                if (drillingComplete != null)
                {
                  drillingComplete();
                  break;
                }
                break;
              }
              break;
          }
          break;
        case 2:
          DrillingMinigame.bit1 = true;
          ++DrillingMinigame.f2;
          if (!DrillingMinigame.hasClosed)
          {
            Function.Call(Hash._0x643ED62D5EA3BEBD);
            DrillingMinigame.hasClosed = true;
            break;
          }
          break;
      }
      Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
      Function.Call(Hash._0x25F87B30C382FCA7);
      DrillingMinigame.Draw();
      DrillingMinigame.SetSpeed((float) ((double) DrillingMinigame.f9 * 0.790000021457672 + (double) DrillingMinigame.fA * 0.0799999982118607));
      DrillingMinigame.SetHoleDepth(DrillingMinigame.fB);
      DrillingMinigame.SetDrillPos(DrillingMinigame.f15 + DrillingMinigame.f16);
      DrillingMinigame.SetTemperature(DrillingMinigame.fF);
      DrillingMinigame.Draw();
    }

    public static float subroutine185d(float a_0, float a_1, float a_2)
    {
      float num = (float) ((1.0 - (double) Function.Call<float>(Hash.COS, (float) ((double) a_2 * 57.2957801818848 * 3.14159274101257))) * 0.5);
      return (float) ((double) a_0 * (1.0 - (double) num) + (double) a_1 * (double) num);
    }

    public static void SetSpeed(float speed) => DrillingMinigame._sc.CallFunction("SET_SPEED", (object) speed);

    public static void SetHoleDepth(float depth) => DrillingMinigame._sc.CallFunction("SET_HOLE_DEPTH", (object) depth);

    public static void SetDrillPos(float pos) => DrillingMinigame._sc.CallFunction("SET_DRILL_POSITION", (object) pos);

    public static void SetTemperature(float temp) => DrillingMinigame._sc.CallFunction("SET_TEMPERATURE");

    public static void Draw()
    {
      if (DrillingMinigame._sc.Handle == 0)
      {
        DrillingMinigame._sc.Load("drilling");
        Function.Call(Hash._0x3D42B92563939375, "MP DRILL MINIGAME");
      }
      Function.Call(Hash._0x25F87B30C382FCA7);
      DrillingMinigame._sc.Render2D();
    }
  }
}
