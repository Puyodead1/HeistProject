// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.HeistBoard
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Linq;

namespace HeistProject.GUI
{
  internal static class HeistBoard
  {
    private static Scaleform _sc = new Scaleform(0);

    public static void Init()
    {
      if (HeistBoard._sc.Handle != 0)
        return;
      HeistBoard._sc.Load("heist_mp");
    }

    public static void Clear()
    {
      for (int index = 0; index < 6; ++index)
        HeistBoard._sc.CallFunction("BLANK_PLANNING_SLOT", (object) index);
    }

    public static void HighlightItem(int index) => HeistBoard._sc.CallFunction("HIGHLIGHT_ITEM", (object) index);

    public static void Refresh(int view) => HeistBoard._sc.CallFunction("DISPLAY_VIEW", (object) view);

    public static void SetHeistName(string name) => HeistBoard._sc.CallFunction("SET_HEIST_NAME", (object) name, (object) "INFO", (object) "CREW", (object) name);

    public static void AddPlanningDescriptionLeft(int slot, params string[] desc)
    {
      List<object> objectList = new List<object>();
      objectList.Add((object) slot);
      objectList.AddRange((IEnumerable<object>) desc);
      HeistBoard._sc.CallFunction("UPDATE_PLANNING_SLOT_LEFT", objectList.ToArray());
    }

    public static void AddPlanningDescriptionRight(int slot, params string[] desc) => HeistBoard._sc.CallFunction("UPDATE_PLANNING_SLOT_RIGHT", (object) slot, (object) desc);

    public static void InitializePlanningSlots(
      string heistTitle,
      bool refresh,
      params MenuSetup[] setups)
    {
      HeistBoard.SetHeistName(heistTitle);
      HeistBoard._sc.CallFunction("INITIALISE_PLANNINGBOARD");
      HeistBoard._sc.CallFunction("SHOW_PLANNINGBOARD");
      for (int slot1 = 0; slot1 < setups.Length; ++slot1)
      {
        Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, (InputArgument) setups[slot1].Photo1, (InputArgument) true);
        HeistBoard.UpdatePlanningSlot(slot1, setups[slot1].Texture, setups[slot1].Name, setups[slot1].Photo1 ?? "", setups[slot1].Photo2 ?? "", setups[slot1].Photo3 ?? "", setups[slot1].Photo3 ?? "", setups[slot1].Photo3 ?? "", setups[slot1].Photo3 ?? "", setups[slot1].Complete ? 0 : -1, setups[slot1].Available, setups[slot1].Highlighted, false);
        int slot2 = slot1;
        string description = setups[slot1].Description;
        string[] strArray = description != null ? description.SplitInParts(98).ToArray<string>() : (string[]) null;
        HeistBoard.AddPlanningDescriptionLeft(slot2, strArray);
      }
      HeistBoard.ShowPlanningImages(true);
      if (!refresh)
        return;
      HeistBoard.Refresh(1);
    }

    public static void UpdatePlanningSlot(
      int slot,
      string txd,
      string title,
      string txn1,
      string image1,
      string txn2,
      string image2,
      string ixn3,
      string image3,
      int completed,
      bool available,
      bool highlight,
      bool fadeInCross)
    {
      HeistBoard._sc.CallFunction("ADD_PLANNING_SLOT", (object) slot, (object) txd, (object) title, (object) txn1, (object) image1, (object) txn2, (object) image2, (object) ixn3, (object) image3, (object) completed, (object) available, (object) highlight, (object) fadeInCross);
      HeistBoard._sc.CallFunction("UPDATE_PLANNING_SLOT", (object) slot, (object) txd, (object) title, (object) txn1, (object) image1, (object) txn2, (object) image2, (object) ixn3, (object) image3, (object) completed, (object) available, (object) highlight, (object) fadeInCross);
    }

    public static void InitStrandboard(int numberOfSlots)
    {
      for (int slot = 0; slot < numberOfSlots; ++slot)
        HeistBoard.AddStrandboardSlot(slot, "MPHEIST_BIOLAB", "HEIST_TITLE", "HEIST_DESCRIPTION", "HEIST_LAUN", true, true, 5000);
      HeistBoard._sc.CallFunction("SET_STRAND_BOARD_TITLE", (object) "HEIST_STR_T");
      HeistBoard._sc.CallFunction("INITIALISE_STRANDBOARD");
      HeistBoard._sc.CallFunction("SHOW_STRANDBOARD");
    }

    public static void ShowPlanningImages(bool show) => HeistBoard._sc.CallFunction("SHOW_PLANNING_IMAGES", (object) show);

    public static void UpdateCrewmember(
      int playerSlot,
      string playerName,
      int rank,
      string portrait,
      string role,
      int roleIcon,
      string status,
      int statusIcon,
      int cutCash,
      int cutPercentage,
      int gangEnum,
      string codename,
      string outfit,
      int numPlayers,
      int headset,
      bool invalidSelection)
    {
      HeistBoard._sc.CallFunction("UPDATE_CREW_MEMBER", (object) playerSlot, (object) playerName, (object) rank, (object) portrait, (object) role, (object) roleIcon, (object) status, (object) statusIcon, (object) cutCash, (object) cutPercentage, (object) gangEnum, (object) codename, (object) outfit, (object) numPlayers, (object) headset, (object) invalidSelection);
    }

    public static void SetLabels(
      string heist,
      string launchButton,
      string take,
      string plan,
      string total,
      string playerCut,
      string role,
      string cut,
      string status,
      string outfit)
    {
      HeistBoard._sc.CallFunction("SET_LABELS", (object) heist, (object) launchButton, (object) take, (object) plan, (object) total, (object) playerCut, (object) role, (object) cut, (object) status, (object) outfit);
    }

    public static void AddStrandboardSlot(
      int slot,
      string txd,
      string title,
      string desc,
      string txn1,
      bool completed,
      bool available,
      int cost)
    {
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION, (InputArgument) HeistBoard._sc.Handle, (InputArgument) "UPDATE_STRAND_SLOT");
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_INT, (InputArgument) slot);
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, (InputArgument) txd);
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, (InputArgument) title);
      Function.Call(Hash._0xE83A3E3557A56640, (InputArgument) desc);
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_STRING, (InputArgument) txn1);
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_INT, (InputArgument) (available ? 1 : 0));
      Function.Call(Hash._PUSH_SCALEFORM_MOVIE_FUNCTION_PARAMETER_BOOL, (InputArgument) completed);
      Function.Call(Hash._BEGIN_TEXT_COMPONENT, (InputArgument) "HEIST_STR_C");
      Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, (InputArgument) cost);
      Function.Call(Hash._END_TEXT_COMPONENT);
      Function.Call(Hash._POP_SCALEFORM_MOVIE_FUNCTION_VOID);
    }

    public static void Draw()
    {
      if (HeistBoard._sc.Handle == 0)
        HeistBoard.Init();
      HeistBoard._sc.Render2D();
    }

    public static void Draw(Vector3 pos, Vector3 rot, Vector3 scale)
    {
      if (HeistBoard._sc.Handle == 0)
        HeistBoard.Init();
      HeistBoard._sc.Render3D(pos, rot, scale);
    }
  }
}
