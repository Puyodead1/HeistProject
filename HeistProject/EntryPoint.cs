// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject.Data;
using HeistProject.GUI;
using HeistProject.GUI.MainMenu;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HeistProject
{
  public class EntryPoint : Script
  {
    public static string SAVE_PATH = "scripts\\HeistProject\\progress.sav";
    public static string MASK_DATA_PATH = "scripts\\HeistProject\\masks.json";
    public static string OUTFIT_DATA_PATH = "scripts\\HeistProject\\outfits.json";
    private UIMenu _mainMenu;
    private MenuPool _menuPool;
    private static ScriptHandler _handler;
    private ScriptedLogic _currentHeist;
    private SetupMenu _menu;
    private OutfitSelectionMenu _outfitMenu;
    private TabView _heistMainMenu;
    private Vector3 _spawnPoint = new Vector3(453.53f, -3074.97f, 5.1f);
    private Vector3 _vehSpawnPoint = new Vector3(455.87f, -3063.9f, 5.72f);
    public static Savegame MainSave;

    public EntryPoint()
    {
      EntryPoint.MainSave = Savegame.LoadProgress(EntryPoint.SAVE_PATH);
      this.Tick += new EventHandler(this.OnTick);
      this.KeyDown += new KeyEventHandler(this.OnKeyDown);
      Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "busy_spinner", true);
      Masks.Load(EntryPoint.MASK_DATA_PATH);
      Outfits.Load(EntryPoint.OUTFIT_DATA_PATH);
      this._menuPool = new MenuPool();
      this._mainMenu = new UIMenu("Heists", "START A HEIST");
      this.RebuildHeistList();
      this._menuPool.Add(this._mainMenu);
      World.SetRelationshipBetweenGroups(Relationship.Respect, -347613984, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Respect, -1467815081, Game.Player.Character.RelationshipGroup);
      Blip blip = World.CreateBlip(new Vector3(453.52f, -3077.9f, 5.07f));
      blip.Sprite = BlipSprite.Heist;
      blip.IsShortRange = true;
    }

    public static Ped[] Team { get; set; }

    private void RebuildHeistList()
    {
      EntryPoint._handler = new ScriptHandler();
      string title = "Continue Heist";
      string str = "You are not curretly playing a Heist. After you start doing setups for a heist, you may continue it from here.\n\nYou can start a heist from the HEISTS tab.";
      string format = "Continue heist \"{0}\".";
      this._heistMainMenu = new TabView("Grand Theft Auto V");
      TabTextItem tabTextItem1 = new TabTextItem("Continue", title, string.IsNullOrEmpty(EntryPoint.MainSave.CurrentHeist) ? str : (EntryPoint.GetHeistById(EntryPoint.MainSave.CurrentHeist) == null ? str : string.Format(format, (object) EntryPoint.GetHeistById(EntryPoint.MainSave.CurrentHeist).Name)));
      tabTextItem1.Activated += (EventHandler) ((o, e) =>
      {
        if (string.IsNullOrEmpty(EntryPoint.MainSave.CurrentHeist))
          return;
        this._menuPool.CloseAllMenus();
        this._heistMainMenu.Visible = false;
        Game.FadeScreenOut(500);
        Script.Wait(500);
        if (!this.BuildHeistBoardForHeist(EntryPoint.GetHeistById(EntryPoint.MainSave.CurrentHeist)))
          this._heistMainMenu.Visible = true;
        Game.FadeScreenIn(500);
      });
      this._heistMainMenu.Tabs.Add((TabItem) tabTextItem1);
      TabHeistListItem tabHeistListItem = new TabHeistListItem("Heists", (IEnumerable<HeistDefinition>) EntryPoint._handler.Scripts);
      tabHeistListItem.OnItemSelect += (OnItemSelect) (hest =>
      {
        this._menuPool.CloseAllMenus();
        this._heistMainMenu.Visible = false;
        Game.FadeScreenOut(500);
        Script.Wait(500);
        if (!this.BuildHeistBoardForHeist(hest))
          this._heistMainMenu.Visible = true;
        Game.FadeScreenIn(500);
      });
      this._heistMainMenu.Tabs.Add((TabItem) tabHeistListItem);
      List<TabItem> items = new List<TabItem>();
      items.Add((TabItem) new TabItemSimpleList("Stats", new Dictionary<string, string>()
      {
        {
          "Current Cash",
          "5000$"
        },
        {
          "Total Money Spent",
          "5000$"
        },
        {
          "Total Money Earned",
          "15000$"
        },
        {
          "Heists Played",
          "5"
        },
        {
          "Money Transferred",
          "120,000$"
        },
        {
          "Money Withdrawn",
          "10000$"
        },
        {
          "Money Lost In Laundering",
          "300$"
        }
      }));
      TabTextItem tabTextItem2 = new TabTextItem("Transfer Money", "Transfer Money", "You can spend dirty money on setting up heists and purchasing equipment. You can acquire dirty money by completing heists or transferring from Michael's account.\n\nPress " + (Game.CurrentInputMode == null ? "Return" : "A") + " to transfer $10,000");
      tabTextItem2.Activated += (EventHandler) ((sender, args) =>
      {
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
          EntryPoint.MainSave.MoneyTransferred += 10000;
          Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
          Util.SendFleecaMessage(10000.ToString("C0") + " have been transferred to your account!");
        }
      });
      items.Add((TabItem) tabTextItem2);
      TabTextItem tabTextItem3 = new TabTextItem("Withdraw Money", "Withdraw Money", "Withdrawing dirty money requires laundering it first. Lester can set you up with a 40% loss.\n\nPress " + (Game.CurrentInputMode == null ? "Return" : "A") + " to withdraw money to Michael's account.");
      tabTextItem3.Activated += (EventHandler) ((sender, args) =>
      {
        OutputArgument outputArgument = new OutputArgument();
        Function.Call(Hash.STAT_GET_INT, Function.Call<int>(Hash.GET_HASH_KEY, "SP0_TOTAL_CASH"), outputArgument, -1);
        int result = outputArgument.GetResult<int>();
        int val1 = 10000;
        if (EntryPoint.MainSave.DirtyMoney < 0)
        {
          UI.Notify("You don't have enough cash!");
        }
        else
        {
          int num = System.Math.Min(val1, EntryPoint.MainSave.DirtyMoney);
          Function.Call(Hash.STAT_SET_INT, Function.Call<int>(Hash.GET_HASH_KEY, "SP0_TOTAL_CASH"), (result + (int) ((double) num * 0.600000023841858)), 1);
          EntryPoint.MainSave.DirtyMoney -= num;
          EntryPoint.MainSave.MoneyWithdrawn += (int) ((double) num * 0.600000023841858);
          Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
          Util.SendLesterMessage("I've got your " + num.ToString("C0") + " and pulling some strings I could launder " + ((int) (0.600000023841858 * (double) num)).ToString("C0") + " of profit out of it.");
        }
      });
      items.Add((TabItem) tabTextItem3);
      this._heistMainMenu.Tabs.Add((TabItem) new TabSubmenuItem("Money", (IEnumerable<TabItem>) items));
      this._heistMainMenu.Tabs.Add((TabItem) new TabTextItem("Character", "Character Editor", "The character editor is not available in this version."));
      this._heistMainMenu.Tabs.Add((TabItem) new TabTextItem("Brief", "Heist Project", "Created by ~h~Guadmaz~h~\nVersion " + (object) ParseableVersion.FromAssembly()));
      this._heistMainMenu.RefreshIndex();
      this._heistMainMenu.OnMenuClose += (EventHandler) ((sender, args) => WarehouseCamera.Stop());
    }

    private void NoArea()
    {
      Vector3 position = Game.Player.Character.Position;
      Vector2 vector2_1 = new Vector2(425.66f, -2895.09f);
      Vector2 vector2_2 = new Vector2(635.07f, -3375.69f);
      if ((double) position.X <= (double) vector2_1.X || (double) position.Y >= (double) vector2_1.Y || (double) position.X >= (double) vector2_2.X || (double) position.Y <= (double) vector2_2.Y || Game.Player.WantedLevel != 0)
        return;
      Game.MaxWantedLevel = 0;
      Game.Player.WantedLevel = 0;
    }

    public static HeistDefinition GetHeistById(string id) => EntryPoint._handler.Scripts.FirstOrDefault<HeistDefinition>((Func<HeistDefinition, bool>) (s => s.Id == id));

    public bool BuildHeistBoardForHeist(HeistDefinition heist)
    {
      if (!EntryPoint.MainSave.StartedHeists.Contains(heist.Id))
      {
        if (EntryPoint.MainSave.DirtyMoney < heist.SetupCost)
        {
          UI.Notify("You don't have enough money to set up this score!");
          return false;
        }
        EntryPoint.MainSave.DirtyMoney -= heist.SetupCost;
        EntryPoint.MainSave.StartedHeists.Add(heist.Id);
        Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
      }
      Game.Player.Character.Position = new Vector3(568.96f, -3124.21f, 18.77f);
      if (this._menu != null)
        this._menu.Dispose();
      List<MenuSetup> menuSetupList = new List<MenuSetup>();
      foreach (SetupDefinition setup1 in heist.Setups)
      {
        SetupDefinition setup = setup1;
        MenuSetup ourSet = new MenuSetup();
        ourSet.Name = setup.Name;
        ourSet.Description = setup.Description;
        ourSet.Available = true;
        ourSet.Texture = "MPHEIST_BIOLAB";
        ourSet.Photo1 = "bpsbadhacker";
        ourSet.Photo2 = "bpsbadhacker2";
        ourSet.Position = setup.MapPosition;
        ourSet.Complete = EntryPoint.MainSave.FinishedSetups.Contains(setup.Id);
        ourSet.Activated += (EventHandler) ((sender, args) =>
        {
          EntryPoint.MainSave.CurrentHeist = heist.Id;
          Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
          this._menu.Dispose();
          this.CreateTeam(setup.MaxCapacity);
          this._outfitMenu = new OutfitSelectionMenu(ourSet.Name, ourSet.Description, EntryPoint.Team);
            this._outfitMenu.OnSelectionComplete += (EventHandler)((o, eventArgs) =>
            {
                EntryPoint.PrepareTeam();
                this.StartScriptedHeist(setup.GetLogic());
            });
        });
        menuSetupList.Add(ourSet);
      }
      MenuSetup ourSet1 = new MenuSetup();
      ourSet1.Name = "Heist Finale";
      ourSet1.Description = heist.Description;
      ourSet1.Available = heist.Setups.All<SetupDefinition>((Func<SetupDefinition, bool>) (s => EntryPoint.MainSave.FinishedSetups.Contains(s.Id)));
      ourSet1.Complete = false;
      ourSet1.Position = heist.MapPosition;
      ourSet1.Texture = "bpsbadhacker";
      ourSet1.Photo1 = "bpsbadhacker";
      ourSet1.Photo2 = "bpsbadhacker2";
      ourSet1.Activated += (EventHandler) ((sender, args) =>
      {
        EntryPoint.MainSave.CurrentHeist = heist.Id;
        Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
        this._menu.Dispose();
        this.CreateTeam(heist.MaxCapacity);
        this._outfitMenu = new OutfitSelectionMenu(ourSet1.Name, ourSet1.Description, EntryPoint.Team);
          this._outfitMenu.OnSelectionComplete += (EventHandler)((o, eventArgs) =>
          {
              EntryPoint.PrepareTeam();
              this.StartScriptedHeist(heist.GetLogic());
          });
      });
      menuSetupList.Add(ourSet1);
      this._menu = new SetupMenu(heist.Name, menuSetupList.ToArray());
      return true;
    }

    public void StartScriptedHeist(ScriptedLogic logic)
    {
      ScriptHandler.Log("SCRIPT START " + (object) DateTime.Now);
      ScriptHandler.Log("Logic null: " + (logic == null).ToString());
      ScriptHandler.Log("Starting script " + logic.Id + "...");
      logic.StartTime = Game.GameTime;
      ScriptHandler.Log("Spawning Player");
      Game.Player.Character.Position = this._spawnPoint - new Vector3(0.0f, 0.0f, 1f);
      Game.Player.Character.Armor = 200;
      Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 1593441988, 200, false, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, -1063057011, 5000, false, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, -37975472, 5, false, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 487013001, 200, false, true);
      Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, 324215364, 1000, false, true);
      ScriptHandler.Log("Spawning car...");
      Model model = new Model(VehicleHash.Stanier);
      model.Request(10000);
      World.CreateVehicle(model, this._vehSpawnPoint, 2.6f).IsPersistent = false;
      model.MarkAsNoLongerNeeded();
      ScriptHandler.Log("Teleporting team...");
      foreach (Entity entity in EntryPoint.Team)
        entity.Position = Game.Player.Character.Position + Vector3.RandomXY() * 3f;
      ScriptHandler.Log("Starting logic...");
      logic.Start();
      this._currentHeist = logic;
      BackgroundThread.CurrentScriptedLogic = logic;
      Script.Wait(1000);
      ScriptHandler.Log("Done!");
      Game.FadeScreenIn(1000);
    }

    public void EndCurrentScript()
    {
      for (int index = 1; index < EntryPoint.Team.Length; ++index)
      {
        if ((Entity) EntryPoint.Team[index] != (Entity) null)
          EntryPoint.Team[index].Delete();
      }
      if (this._currentHeist == null)
        return;
      ScriptHandler.Log("ENDING CURRENT SCRIPT " + this._currentHeist.Id);
      this._currentHeist.End();
      BackgroundThread.CurrentScriptedLogic = (ScriptedLogic) null;
      this._currentHeist = (ScriptedLogic) null;
      Model model = new Model(PedHash.Michael);
      model.Request(20000);
      Util.SetPlayerModel(model);
      model.MarkAsNoLongerNeeded();
      string str = "You are not curretly playing a Heist. After you start doing setups for a heist, you may continue it from here.\n\nYou can start a heist from the HEISTS tab.";
      string format = "Continue heist \"{0}\".";
      ((TabTextItem) this._heistMainMenu.Tabs[0]).Text = string.IsNullOrEmpty(EntryPoint.MainSave.CurrentHeist) ? str : string.Format(format, (object) EntryPoint.GetHeistById(EntryPoint.MainSave.CurrentHeist).Name);
    }

    public static void PrepareTeam()
    {
      for (int index = 1; index < EntryPoint.Team.Length; ++index)
      {
        int num = Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Game.Player.Character.Handle);
        Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, EntryPoint.Team[index].Handle, num);
        Function.Call(Hash.SET_GROUP_FORMATION_SPACING, num, 1.5f, 3.5f, 3.212837E+09f);
        Function.Call(Hash._0x2E2F4240B3F24647, EntryPoint.Team[index], num, false);
        if (EntryPoint.Team[index].CurrentBlip == null || !EntryPoint.Team[index].CurrentBlip.Exists())
        {
          EntryPoint.Team[index].AddBlip();
          EntryPoint.Team[index].CurrentBlip.Color = BlipColor.Blue;
          EntryPoint.Team[index].CurrentBlip.Scale = 0.7f;
        }
        EntryPoint.Team[index].BlockPermanentEvents = false;
        EntryPoint.Team[index].NeverLeavesGroup = true;
        EntryPoint.Team[index].Armor = 200;
        EntryPoint.Team[index].MaxHealth = 200;
        EntryPoint.Team[index].Health = 200;
        EntryPoint.Team[index].AlwaysDiesOnLowHealth = false;
        Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, EntryPoint.Team[index].Handle, 1);
        Function.Call(Hash.SET_PED_HEARING_RANGE, EntryPoint.Team[index], 100f);
        Function.Call(Hash.SET_PED_SEEING_RANGE, EntryPoint.Team[index], 100f);
        Function.Call(Hash.SET_PED_COMBAT_RANGE, EntryPoint.Team[index], 1);
        Function.Call(Hash.SET_PED_TO_INFORM_RESPECTED_FRIENDS, EntryPoint.Team[index], 100f, 8);
        Function.Call(Hash.GIVE_WEAPON_TO_PED, EntryPoint.Team[index], 1593441988, 200, false, true);
        Function.Call(Hash.GIVE_WEAPON_TO_PED, EntryPoint.Team[index], -1063057011, 5000, false, true);
        Function.Call(Hash.GIVE_WEAPON_TO_PED, EntryPoint.Team[index], -37975472, 5, false, true);
        Function.Call(Hash.GIVE_WEAPON_TO_PED, EntryPoint.Team[index], 487013001, 200, false, true);
        Function.Call(Hash.GIVE_WEAPON_TO_PED, EntryPoint.Team[index], 324215364, 1000, false, true);
      }
    }

    public void CreateTeam(int amount)
    {
      if (EntryPoint.Team != null)
      {
        for (int index = 1; index < EntryPoint.Team.Length; ++index)
          EntryPoint.Team[index]?.Delete();
      }
      EntryPoint.Team = new Ped[amount + 1];
      EntryPoint.Team[0] = Game.Player.Character;
      Model model = new Model(PedHash.FreemodeMale01);
      model.Request(10000);
      int num = World.AddRelationshipGroup("HEIST_TEAMMATES");
      World.SetRelationshipBetweenGroups(Relationship.Respect, num, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Respect, Game.Player.Character.RelationshipGroup, num);
      for (int index = 1; index < amount + 1; ++index)
      {
        EntryPoint.Team[index] = World.CreatePed(model, Game.Player.Character.Position);
        EntryPoint.Team[index].RelationshipGroup = num;
        Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, EntryPoint.Team[index].Handle, 1);
      }
      model.MarkAsNoLongerNeeded();
    }

    private void UpdateHeistMenu()
    {
      List<HeistDefinition> source1 = new List<HeistDefinition>(EntryPoint.MainSave.FinishedHeists.Select<string, HeistDefinition>((Func<string, HeistDefinition>) (id => EntryPoint.GetHeistById(id))).Where<HeistDefinition>((Func<HeistDefinition, bool>) (h => h != null)));
      List<HeistDefinition> source2 = new List<HeistDefinition>(EntryPoint.MainSave.StartedHeists.Select<string, HeistDefinition>((Func<string, HeistDefinition>) (id => EntryPoint.GetHeistById(id))).Where<HeistDefinition>((Func<HeistDefinition, bool>) (h => h != null)));
      TabItemSimpleList tabItemSimpleList = (TabItemSimpleList) ((TabSubmenuItem) this._heistMainMenu.Tabs[2]).Items[0];
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("Current Cash", EntryPoint.MainSave.DirtyMoney.ToString("C0"));
      dictionary.Add("Total Money Spent", (source1.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.SetupCost)) + source2.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.SetupCost))).ToString("C0"));
      dictionary.Add("Total Money Earned", source1.Sum<HeistDefinition>((Func<HeistDefinition, int>) (h => h.Payout)).ToString("C0"));
      int num = source1.Count;
      dictionary.Add("Heists Finished", num.ToString());
      num = EntryPoint.MainSave.MoneyTransferred;
      dictionary.Add("Money Transferred", num.ToString("C0"));
      dictionary.Add("Money Withdrawn", EntryPoint.MainSave.MoneyWithdrawn.ToString("C0"));
      dictionary.Add("Money Lost In Laundering", ((float) (0.400000005960464 * (double) EntryPoint.MainSave.MoneyWithdrawn / 0.600000023841858)).ToString("C0"));
      tabItemSimpleList.Dictionary = dictionary;
      ((TabTextItem) ((TabSubmenuItem) this._heistMainMenu.Tabs[2]).Items[1]).Text = "You can spend dirty money on setting up heists and purchasing equipment. You can acquire dirty money by completing heists or transferring from Michael's account.\n\nPress " + (Game.CurrentInputMode == null ? "Return" : "A") + " to transfer $10,000";
      ((TabTextItem) ((TabSubmenuItem) this._heistMainMenu.Tabs[2]).Items[2]).Text = "Withdrawing dirty money requires laundering it first. Lester can set you up with a 40% loss.\n\nPress " + (Game.CurrentInputMode == null ? "Return" : "A") + " to withdraw money to Michael's account.";
    }

    private void OnTick(object sender, EventArgs eventArgs)
    {
      if (this._currentHeist == null && !this._heistMainMenu.Visible)
      {
        Util.DrawEntryMarker(new Vector3(453.52f, -3077.9f, 5.07f));
        if (Game.Player.Character.IsInRangeOf(new Vector3(453.52f, -3077.9f, 5.07f), 3f))
        {
          Util.DisplayHelpText("Press ~INPUT_CONTEXT~ to enter Heist Menu.");
          if (Game.IsControlJustPressed(0, GTA.Control.Context))
          {
            WarehouseCamera.StartTransition();
            BackgroundThread.DrawLoadingIcon = true;
            ParseableVersion internetVersion = new ParseableVersion();
            Thread thread = new Thread((ThreadStart) (() =>
            {
              try
              {
                internetVersion = ParseableVersion.FromWebsite("heist-project");
              }
              catch (Exception ex)
              {
              }
            }));
            thread.Start();
            DateTime now = DateTime.Now;
            while (thread.IsAlive && DateTime.Now.Subtract(now).TotalMilliseconds < 6000.0)
            {
              Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
              Script.Yield();
            }
            BackgroundThread.DrawLoadingIcon = false;
            if (thread.IsAlive)
              thread.Abort();
            if (internetVersion.Revision != 0)
            {
              ((TabTextItem) this._heistMainMenu.Tabs[4]).Text = "Created by ~h~Guadmaz~h~\nVersion " + (object) ParseableVersion.FromAssembly() + "\nLatest Version: " + (object) internetVersion;
              if (internetVersion > ParseableVersion.FromAssembly())
              {
                Util.SendPictureNotification("New version " + (object) internetVersion + " is available!", "commonmenu", "mp_alerttriangle", 1, 7, "~b~~h~Heist Project~h~", "~m~gta5-mods.com");
                Util.PlaySoundFrontend("Phone_SoundSet_Default", "Text_Arrive_Tone");
              }
            }
            this._heistMainMenu.Visible = !this._heistMainMenu.Visible;
          }
        }
      }
      this.UpdateHeistMenu();
      this._heistMainMenu.MoneySubtitle = EntryPoint.MainSave.DirtyMoney.ToString("C0");
      this._heistMainMenu.Update();
      HeistCelebration.Draw();
      this._menuPool.ProcessMenus();
      HeistProject.GUI.TimerBarPool.Draw();
      DrillingMinigame.Update();
      this._menu?.Update();
      this.NoArea();
      this._outfitMenu?.Update();
      if (this._currentHeist == null)
        return;
      if (Game.Player.Character.Health <= 0)
      {
        Model model = new Model(PedHash.Michael);
        model.Request(10000);
        Util.SetPlayerModel(model);
        model.MarkAsNoLongerNeeded();
        Game.Player.Character.Kill();
      }
      if (Game.Player.IsInvincible)
      {
        for (int index = 1; index < EntryPoint.Team.Length; ++index)
          EntryPoint.Team[index].IsInvincible = true;
      }
      else if (EntryPoint.Team.Length > 1 && EntryPoint.Team[1].IsInvincible)
      {
        for (int index = 1; index < EntryPoint.Team.Length; ++index)
          EntryPoint.Team[index].IsInvincible = false;
      }
      this._currentHeist.Update();
      for (int index = 1; index < EntryPoint.Team.Length; ++index)
      {
        if (EntryPoint.Team[index].IsDead && EntryPoint.Team[index].CurrentBlip != null && EntryPoint.Team[index].CurrentBlip.Exists())
          EntryPoint.Team[index].CurrentBlip.Remove();
      }
      if (this._currentHeist.HasFinished)
      {
        Game.Player.IsInvincible = true;
        if (this._currentHeist.HasWon)
        {
          if (this._currentHeist.IsFinale)
          {
            EntryPoint.MainSave.FinishedSetups.Clear();
            EntryPoint.MainSave.CurrentHeist = (string) null;
            EntryPoint.MainSave.FinishedHeists.Add(this._currentHeist.Id);
            EntryPoint.MainSave.StartedHeists.Remove(this._currentHeist.Id);
            Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
            HeistDefinition heistById = EntryPoint.GetHeistById(this._currentHeist.Id);
            if (heistById != null)
            {
              float num1 = 1f;
              if (this._currentHeist.MissionChallenges != null && this._currentHeist.MissionChallenges.Count > 0)
                num1 = (float) this._currentHeist.MissionChallenges.Count<KeyValuePair<string, Tuple<string, bool>>>((Func<KeyValuePair<string, Tuple<string, bool>>, bool>) (p => p.Value.Item2)) / (float) this._currentHeist.MissionChallenges.Count;
              MissionPassedScreen missionPassedScreen = new MissionPassedScreen(heistById.Name, (int) ((double) num1 * 100.0), MissionPassedScreen.Medal.Gold, true);
              if (this._currentHeist.MissionChallenges != null)
              {
                foreach (KeyValuePair<string, Tuple<string, bool>> missionChallenge in this._currentHeist.MissionChallenges)
                  missionPassedScreen.AddItem(missionChallenge.Key, missionChallenge.Value.Item1, missionChallenge.Value.Item2 ? MissionPassedScreen.TickboxState.Tick : MissionPassedScreen.TickboxState.Cross);
                if (this._currentHeist.MissionChallenges.All<KeyValuePair<string, Tuple<string, bool>>>((Func<KeyValuePair<string, Tuple<string, bool>>, bool>) (p => p.Value.Item2)))
                {
                  int num2 = (int) ((double) heistById.Payout * 0.0500000007450581);
                  EntryPoint.MainSave.DirtyMoney += num2;
                  missionPassedScreen.AddItem("Challenges Bonus", num2.ToString("C0"), MissionPassedScreen.TickboxState.None);
                }
              }
              EntryPoint.MainSave.DirtyMoney += heistById.Payout;
              missionPassedScreen.Show();
            }
            Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
          }
          else
          {
            EntryPoint.MainSave.FinishedSetups.Add(this._currentHeist.Id);
            Savegame.SaveProgress(EntryPoint.MainSave, EntryPoint.SAVE_PATH);
            HeistDefinition heistDefinition1 = (HeistDefinition) null;
            SetupDefinition setupDefinition = (SetupDefinition) null;
            foreach (HeistDefinition heistDefinition2 in new List<HeistDefinition>((IEnumerable<HeistDefinition>) EntryPoint._handler.Scripts))
            {
              foreach (SetupDefinition setup in heistDefinition2.Setups)
              {
                if (setup.Id == this._currentHeist.Id)
                {
                  heistDefinition1 = heistDefinition2;
                  setupDefinition = setup;
                }
              }
            }
            if (heistDefinition1 != null)
            {
              float num = 1f;
              if (this._currentHeist.MissionChallenges != null && this._currentHeist.MissionChallenges.Count > 0)
                num = (float) this._currentHeist.MissionChallenges.Count<KeyValuePair<string, Tuple<string, bool>>>((Func<KeyValuePair<string, Tuple<string, bool>>, bool>) (p => p.Value.Item2)) / (float) this._currentHeist.MissionChallenges.Count;
              MissionPassedScreen missionPassedScreen = new MissionPassedScreen(setupDefinition.Name + " - " + heistDefinition1.Name, (int) ((double) num * 100.0), MissionPassedScreen.Medal.Gold, false);
              missionPassedScreen.AddItem("Time Elapsed", Util.FormatMilliseconds(Game.GameTime - this._currentHeist.StartTime), MissionPassedScreen.TickboxState.None);
              if (this._currentHeist.MissionChallenges != null)
              {
                foreach (KeyValuePair<string, Tuple<string, bool>> missionChallenge in this._currentHeist.MissionChallenges)
                  missionPassedScreen.AddItem(missionChallenge.Key, missionChallenge.Value.Item1, missionChallenge.Value.Item2 ? MissionPassedScreen.TickboxState.Tick : MissionPassedScreen.TickboxState.Cross);
              }
              missionPassedScreen.Show();
            }
          }
        }
        else if (!string.IsNullOrEmpty(this._currentHeist.FailureReason))
          new MissionFailedScreen(this._currentHeist.FailureReason, !this._currentHeist.IsFinale).Show();
        Game.FadeScreenOut(1000);
        Script.Wait(1000);
        this.EndCurrentScript();
        Game.Player.IsInvincible = false;
        Game.Player.Character.Position = new Vector3(453.52f, -3077.9f, 5.07f);
        Script.Wait(500);
        Game.FadeScreenIn(1000);
      }
      else
      {
        int num3 = Function.Call<int>(Hash.GET_TIME_SINCE_LAST_DEATH);
        int num4 = Function.Call<int>(Hash.GET_TIME_SINCE_LAST_ARREST);
        if ((num3 == -1 || num3 >= 500) && (num4 == -1 || num4 >= 500))
          return;
        this.EndCurrentScript();
      }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.D9 || this._currentHeist == null)
        return;
      this.EndCurrentScript();
      if (EntryPoint.Team == null)
        return;
      foreach (Entity entity in EntryPoint.Team)
        entity.Delete();
    }
  }
}
