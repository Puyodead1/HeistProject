// Decompiled with JetBrains decompiler
// Type: HeistProject.GUI.MainMenu.TabMoneyItem
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeistProject.GUI.MainMenu
{
  public class TabMoneyItem : TabTextItem
  {
    public Savegame Savegame { get; set; }

    public TabMoneyItem(ref Savegame sg)
      : base("Money", "Dirty Money")
    {
      this.Savegame = sg;
    }

    private void ProcessControls()
    {
      if (!Game.IsControlJustPressed(0, Control.PhoneSelect))
        return;
      OutputArgument outputArgument = new OutputArgument();
      Function.Call(Hash.STAT_GET_INT, Function.Call<int>(Hash.GET_HASH_KEY, "SP0_TOTAL_CASH"), outputArgument, -1);
      int result = outputArgument.GetResult<int>();
      if (result < 10000)
      {
        UI.Notify("Michael has insufficent funds.");
      }
      else
      {
        Function.Call(Hash.STAT_SET_INT, Function.Call<int>(Hash.GET_HASH_KEY, "SP0_TOTAL_CASH"), (result - 10000), 1);
        EntryPoint.MainSave.DirtyMoney += 10000;
        Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
        Util.SendFleecaMessage(10000.ToString("C0") + " have been transferred to your account!");
      }
    }

    public override void Draw()
    {
      this.ProcessControls();
      string str1 = "You can spend dirty money on setting up heists and acquiring equipment. You can transfer your money from the main characters, and withdraw it with a loss.\n\n";
      try
      {
        List<HeistDefinition> source1 = new List<HeistDefinition>(this.Savegame.FinishedHeists.Select<string, HeistDefinition>((Func<string, HeistDefinition>) (id => EntryPoint.GetHeistById(id))).Where<HeistDefinition>((Func<HeistDefinition, bool>) (h => h != null)));
        List<HeistDefinition> source2 = new List<HeistDefinition>(this.Savegame.FinishedHeists.Select<string, HeistDefinition>((Func<string, HeistDefinition>) (id => EntryPoint.GetHeistById(id))).Where<HeistDefinition>((Func<HeistDefinition, bool>) (h => h != null)));
        str1 = str1 + "Current cash: " + this.Savegame.DirtyMoney.ToString("C0") + "\n\n";
        string str2 = str1;
        int num = source1.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.SetupCost)) + source2.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.SetupCost));
        string str3 = num.ToString("C0");
        str1 = str2 + "Total money spent: " + str3 + "\n";
        string str4 = str1;
        num = source1.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.Payout));
        string str5 = num.ToString("C0");
        str1 = str4 + "Total money earned: " + str5 + "\n";
        str1 = str1 + "Total completed heists: " + (object) source1.Count + "\n";
      }
      catch (Exception ex)
      {
      }
      this.Text = str1;
      base.Draw();
      Scaleform scaleform = new Scaleform(0);
      scaleform.Load("instructional_buttons");
      scaleform.CallFunction("SET_DATA_SLOT", (object) 0, (object) Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 176, 0), (object) "Transfer Money");
    }
  }
}
