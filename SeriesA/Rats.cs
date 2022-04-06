// Decompiled with JetBrains decompiler
// Type: SeriesA.Rats
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using HeistProject.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeriesA
{
  public class Rats : ScriptedLogic
  {
    private int _heistStart;
    private int _msgsShown;
    private bool _oldInCar;
    private List<Entity> _cleanup;
    private List<Entity> _muriaticAcid;
    private List<Entity> _causticSoda;
    private List<Entity> _hydrogen;
    private List<Ped> _enemies;
    private Blip _destBlip;
    private bool _hasPlayerSoda;
    private bool _hasPlayerAcid;
    private bool _hasPlayerHydrogen;
    private bool _hasLabSoda;
    private bool _hasLabAcid;
    private bool _hasLabHydrogen;
    private int _cooksDone;
    private TextTimerBar _methBar;
    private Vector3 _methlabPosTarget = new Vector3(1455.82f, 3597.62f, 33.75f);
    private Vector3 _mehlabPos = new Vector3(1392.26f, 3606.73f, 37.94f);
    private bool _oldWanted;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      this._methBar = new TextTimerBar("METH BATCHES", "0/4");
      this._cleanup = new List<Entity>();
      this._causticSoda = new List<Entity>();
      this._muriaticAcid = new List<Entity>();
      this._hydrogen = new List<Entity>();
      this._enemies = new List<Ped>();
      ScriptHandler.Log("STARTING RATS SETUP " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      Model[] source = new Model[1]
      {
        new Model(850468060)
      };
      ScriptHandler.Log("Starting model load...");
      DateTime now = DateTime.Now;
      int num1;
      for (num1 = 0; !((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)) && num1 <= 200; ++num1)
      {
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
        {
          model.Request();
          Script.Yield();
        }
      }
      ScriptHandler.Log("Time elapsed: " + (object) DateTime.Now.Subtract(now).TotalMilliseconds + "ms");
      ScriptHandler.Log("Iterations: " + (object) num1);
      if (!((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)))
      {
        ScriptHandler.Log("All models loaded: false. Unloaded models:");
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
          ScriptHandler.Log(model.Hash.ToString());
      }
      else
        ScriptHandler.Log("All models loaded: true");
      ScriptHandler.Log("Spawning entities...");
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1407.09f, 3606.831f, 39.0012f), -0.0239378f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1405.347f, 3619.401f, 39.00578f), 10.0005f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1397.94f, 3606.638f, 34.98091f), 40.00005f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1391.027f, 3607.472f, 34.98091f), -154.9997f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1388.112f, 3612.392f, 38.94191f), 15.00002f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1384.133f, 3616.959f, 38.92603f), 54.99997f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1394.733f, 3619.268f, 38.92603f), 0.0f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1398.52f, 3606.819f, 38.94191f), 14.99999f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1396.157f, 3607.954f, 38.94191f), -79.9999f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1387.346f, 3608.188f, 38.94191f), -129.9998f));
      this._cleanup.Add((Entity) World.CreatePed(source[0], new Vector3(1390.189f, 3600.852f, 38.94191f), 104.9998f));
      int num2 = World.AddRelationshipGroup("SERIES_A_RATS_LOSTMC");
      World.SetRelationshipBetweenGroups(Relationship.Hate, num2, Game.Player.Character.RelationshipGroup);
      World.SetRelationshipBetweenGroups(Relationship.Hate, Game.Player.Character.RelationshipGroup, num2);
      string[] strArray = new string[4]
      {
        "WORLD_HUMAN_AA_SMOKE",
        "WORLD_HUMAN_DRINKING",
        "WORLD_HUMAN_DRUG_DEALER",
        "WORLD_HUMAN_PARTYING"
      };
      int num3 = 0;
      foreach (Entity entity in this._cleanup)
      {
        if (!(entity == (Entity) null) && entity.Model.IsPed)
        {
          Ped ped = (Ped) entity;
          ped.RelationshipGroup = num2;
          Function.Call(Hash.SET_PED_COMBAT_MOVEMENT,  ped.Handle,  1);
          ped.BlockPermanentEvents = false;
          Function.Call(Hash.SET_PED_HEARING_RANGE,  ped,  100f);
          Function.Call(Hash.SET_PED_SEEING_RANGE,  ped,  100f);
          Function.Call(Hash.SET_PED_COMBAT_RANGE,  ped,  1);
          ped.AddBlip();
          ped.CurrentBlip.Sprite = BlipSprite.Enemy;
          ped.CurrentBlip.Color = BlipColor.Red;
          ped.CurrentBlip.Scale = 0.7f;
          ped.CurrentBlip.Alpha = 0;
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  324215364,  1000,  true,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  453432689,  1000,  true,  true);
          Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  -1074790547,  1000,  true,  true);
          if (num3 >= 9)
            Function.Call(Hash.GIVE_WEAPON_TO_PED,  ped,  100416529,  1000,  true,  true);
          Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE,  ped,  strArray[Util.SharedRandom.Next(strArray.Length)],  0,  false);
          this._enemies.Add(ped);
          ++num3;
        }
      }
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      ScriptHandler.Log("Rats start complete.");
    }

    public override void End()
    {
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      TimerBarPool.Remove((TimerBarBase) this._methBar);
      foreach (Entity entity in this._cleanup)
        entity.Delete();
      foreach (Entity entity in this._muriaticAcid)
        entity.Delete();
      foreach (Entity entity in this._causticSoda)
        entity.Delete();
      foreach (Entity entity in this._hydrogen)
        entity.Delete();
      if (this._destBlip != null)
        this._destBlip.Remove();
      UI.ShowSubtitle("");
    }

    public override void Update()
    {
      if (this.Stage == 3)
      {
        Game.MaxWantedLevel = 5;
        if (Game.Player.WantedLevel < 3)
          Game.Player.WantedLevel = 3;
        if (Game.Player.WantedLevel > 4)
          Game.Player.WantedLevel = 4;
      }
      if (this.Stage == 1)
        Util.DrawEntryMarker(this._methlabPosTarget, 3f);
      foreach (Entity entity in this._cleanup)
      {
        if (!(entity == (Entity) null) && entity.Model.IsPed && entity.CurrentBlip != null && entity.CurrentBlip.Alpha != 0 && entity.IsDead)
          entity.CurrentBlip.Alpha = 0;
      }
      if (this.Stage == 0)
      {
        UI.ShowSubtitle("Go to the ~y~Meth Lab.", 360000);
        this._destBlip = World.CreateBlip(this._methlabPosTarget);
        this._destBlip.Color = BlipColor.Yellow;
        this._destBlip.ShowRoute = true;
        ++this.Stage;
      }
      if (this.Stage == 1 && (Game.Player.Character.IsInRangeOf(this._methlabPosTarget, 2f) || this._cleanup.Where<Entity>((Func<Entity, bool>) (ent => ent != (Entity) null && ent.Model.IsPed)).Any<Entity>((Func<Entity, bool>) (p => ((Ped) p).IsInCombat))))
      {
        this._destBlip.Remove();
        this._destBlip = (Blip) null;
        foreach (Entity entity in this._cleanup)
        {
          if (!(entity == (Entity) null) && entity.Model.IsPed)
            entity.CurrentBlip.Alpha = (int) byte.MaxValue;
        }
        UI.ShowSubtitle("Eliminate the ~r~Lost MC gang~w~.", 360000);
        ++this.Stage;
      }
      if (this.Stage == 2 && this._cleanup.Where<Entity>((Func<Entity, bool>) (ent => ent != (Entity) null && ent.Model.IsPed)).All<Entity>((Func<Entity, bool>) (p => p.IsDead)))
      {
        this.SpawnMethIngridients();
        Util.SendLesterMessage("Alright, get cooking. You'll need Muriatic Acid, Hydrogen Chloride and Caustic Soda to cook a batch.");
        UI.ShowSubtitle("Find the appropriate ~g~ingredients.", 560000);
        TimerBarPool.Add((TimerBarBase) this._methBar);
        ++this.Stage;
      }
      if (this.Stage == 3)
      {
        bool flag = false;
        for (int index = this._muriaticAcid.Count - 1; index >= 0; --index)
        {
          Entity entity = this._muriaticAcid[index];
          if (Function.Call<bool>(Hash.HAS_OBJECT_BEEN_BROKEN,  entity.Handle))
          {
            Model mod = new Model(-1414337382);
            mod.Request(10000);
            Vector3 position = entity.Position;
            Vector3 rotation = entity.Rotation;
            entity.Delete();
            this._muriaticAcid[index] = (Entity) this.CreateFixedProp(mod, position, rotation, false, false);
            mod.MarkAsNoLongerNeeded();
            entity = this._muriaticAcid[index];
            entity.AddBlip();
            entity.CurrentBlip.Color = BlipColor.Green;
            entity.CurrentBlip.Scale = 0.5f;
          }
          if (!this._hasLabAcid && !this._hasPlayerAcid)
          {
            Function.Call((Hash) 17626695965285041557,  entity.Position.X,  entity.Position.Y,  entity.Position.Z,  (int) byte.MaxValue,  0,  0,  0.8f,  1f,  1f);
            if (flag = flag || Game.Player.Character.IsInRangeOf(entity.Position, 2f))
            {
              Util.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pick up Muriatic Acid.");
              if (Game.IsControlJustPressed(0, Control.Context))
              {
                entity.Delete();
                this._muriaticAcid.Remove(entity);
                UI.Notify("~r~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                this._hasPlayerAcid = true;
                UI.Notify("~g~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullGreenDuffelBag);
                if (this._destBlip == null)
                  this._destBlip = World.CreateBlip(this._mehlabPos);
                Function.Call(Hash.PLAY_SOUND_FRONTEND,  -1,  "ROBBERY_MONEY_TOTAL",  "HUD_FRONTEND_CUSTOM_SOUNDSET",  1);
                UI.ShowSubtitle("Add the Muriatic Acid to the ~y~meth lab~w~ or find another ~g~ingredient~w~.", 120000);
                return;
              }
            }
          }
        }
        for (int index = this._hydrogen.Count - 1; index >= 0; --index)
        {
          Entity entity = this._hydrogen[index];
          if (Function.Call<bool>(Hash.HAS_OBJECT_BEEN_BROKEN,  entity.Handle))
          {
            Model mod = new Model(-1382355819);
            mod.Request(10000);
            Vector3 position = entity.Position;
            Vector3 rotation = entity.Rotation;
            entity.Delete();
            this._hydrogen[index] = (Entity) this.CreateFixedProp(mod, position, rotation, false, false);
            mod.MarkAsNoLongerNeeded();
            entity = this._muriaticAcid[index];
            entity.AddBlip();
            entity.CurrentBlip.Color = BlipColor.Green;
            entity.CurrentBlip.Scale = 0.5f;
          }
          if (!this._hasLabHydrogen && !this._hasPlayerHydrogen)
          {
            Function.Call((Hash) 17626695965285041557,  entity.Position.X,  entity.Position.Y,  entity.Position.Z,  0,  (int) byte.MaxValue,  0,  0.8f,  1f,  1f);
            if (flag = flag || Game.Player.Character.IsInRangeOf(entity.Position, 2f))
            {
              Util.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pick up Hydrogen Chloride.");
              if (Game.IsControlJustPressed(0, Control.Context))
              {
                entity.Delete();
                this._hydrogen.Remove(entity);
                UI.Notify("~r~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                this._hasPlayerHydrogen = true;
                UI.Notify("~g~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullGreenDuffelBag);
                if (this._destBlip == null)
                  this._destBlip = World.CreateBlip(this._mehlabPos);
                Function.Call(Hash.PLAY_SOUND_FRONTEND,  -1,  "ROBBERY_MONEY_TOTAL",  "HUD_FRONTEND_CUSTOM_SOUNDSET",  1);
                UI.ShowSubtitle("Add the Hydrogen Chloride to the ~y~meth lab~w~ or find another ~g~ingredient~w~.", 120000);
                return;
              }
            }
          }
        }
        for (int index = this._causticSoda.Count - 1; index >= 0; --index)
        {
          Entity entity = this._causticSoda[index];
          if (Function.Call<bool>(Hash.HAS_OBJECT_BEEN_BROKEN,  entity.Handle))
          {
            Model mod = new Model(-374844025);
            mod.Request(10000);
            Vector3 position = entity.Position;
            Vector3 rotation = entity.Rotation;
            entity.Delete();
            this._causticSoda[index] = (Entity) this.CreateFixedProp(mod, position, rotation, false, false);
            mod.MarkAsNoLongerNeeded();
            entity = this._muriaticAcid[index];
            entity.AddBlip();
            entity.CurrentBlip.Color = BlipColor.Green;
            entity.CurrentBlip.Scale = 0.5f;
          }
          if (!this._hasLabSoda && !this._hasPlayerSoda)
          {
            Function.Call((Hash) 17626695965285041557,  entity.Position.X,  entity.Position.Y,  entity.Position.Z,  0,  0,  (int) byte.MaxValue,  0.8f,  1f,  1f);
            if (flag = flag || Game.Player.Character.IsInRangeOf(entity.Position, 2f))
            {
              Util.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to pick up Caustic Soda.");
              if (Game.IsControlJustPressed(0, Control.Context))
              {
                entity.Delete();
                this._causticSoda.Remove(entity);
                UI.Notify("~r~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                this._hasPlayerSoda = true;
                UI.Notify("~g~Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
                Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullGreenDuffelBag);
                if (this._destBlip == null)
                  this._destBlip = World.CreateBlip(this._mehlabPos);
                Function.Call(Hash.PLAY_SOUND_FRONTEND,  -1,  "ROBBERY_MONEY_TOTAL",  "HUD_FRONTEND_CUSTOM_SOUNDSET",  1);
                UI.ShowSubtitle("Add the Caustic Soda to the ~y~meth lab~w~ or find another ~g~ingredient~w~.", 120000);
                return;
              }
            }
          }
        }
        Util.DrawEntryMarker(this._mehlabPos);
        if (!flag && !Game.Player.Character.IsInRangeOf(this._mehlabPos, 2f))
          Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
      }
      if (this.Stage != 4)
        return;
      bool flag1 = Game.Player.WantedLevel > 0;
      if (!this._oldWanted & flag1)
      {
        UI.ShowSubtitle("Lose the cops.", 360000);
        this._destBlip.Alpha = 0;
        this._destBlip.ShowRoute = false;
      }
      else if (this._oldWanted && !flag1)
      {
        UI.ShowSubtitle("Return to the ~y~warehouse.", 360000);
        this._destBlip.Alpha = (int) byte.MaxValue;
        this._destBlip.ShowRoute = true;
      }
      this._oldWanted = flag1;
    }

    public override void BackgroundThreadUpdate()
    {
      if (this.Stage != 3 || !Game.Player.Character.IsInRangeOf(this._mehlabPos, 1.5f) || !this._hasPlayerHydrogen && !this._hasPlayerSoda && !this._hasPlayerAcid)
        return;
      Util.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to add the ingredients.");
      if (!Game.IsControlJustPressed(0, Control.Context))
        return;
      Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.None);
      this._destBlip.Remove();
      this._destBlip = (Blip) null;
      UI.Notify("Acid: " + this._hasPlayerAcid.ToString() + " Hyd: " + this._hasPlayerHydrogen.ToString() + " Soda: " + this._hasPlayerSoda.ToString());
      UI.Notify("Lab Acid: " + this._hasLabAcid.ToString() + " Lab Hyd: " + this._hasLabHydrogen.ToString() + " Lab Soda: " + this._hasLabSoda.ToString());
      this._hasLabAcid = this._hasLabAcid || this._hasPlayerAcid;
      this._hasLabHydrogen = this._hasLabHydrogen || this._hasPlayerHydrogen;
      this._hasLabSoda = this._hasLabSoda || this._hasPlayerSoda;
      this._hasPlayerHydrogen = false;
      this._hasPlayerAcid = false;
      this._hasPlayerSoda = false;
      if (this._hasLabAcid && this._hasLabHydrogen && this._hasLabSoda)
      {
        UI.ShowSubtitle("Wait for the meth to cook.", 120000);
        Util.SendLesterMessage("Alright, it's cooking, let's hope for the best.");
        Script.Wait(30000);
        this._hasLabHydrogen = false;
        this._hasLabAcid = false;
        this._hasLabSoda = false;
        ++this._cooksDone;
        this._methBar.Text = this._cooksDone.ToString() + "/4";
        if (this._cooksDone >= 4)
        {
          Util.SendLesterMessage("Alright that's enough meth for today. Pack up and return to the safehouse, but lose the heat!");
          Util.SetPedAccessory(Game.Player.Character, Util.HeistAccessory.FullGreenDuffelBag);
          foreach (Entity entity in this._causticSoda)
            entity.Delete();
          foreach (Entity entity in this._hydrogen)
            entity.Delete();
          foreach (Entity entity in this._muriaticAcid)
            entity.Delete();
          UI.ShowSubtitle("Lose the cops.", 360000);
          this._destBlip = World.CreateBlip(Util.Warehouse);
          this._destBlip.Color = BlipColor.Yellow;
          this._destBlip.ShowRoute = true;
          this._destBlip.Alpha = 0;
          ++this.Stage;
        }
        else
        {
          Util.SetPedAccessory(EntryPoint.Team[this._cooksDone], Util.HeistAccessory.FullGreenDuffelBag);
          Util.SendLesterMessage("Wow, you didn't explode! Onto the next batch.");
          UI.ShowSubtitle("Find the ~g~ingredients.", 120000);
        }
      }
      else
      {
        Util.SendLesterMessage("Very nice, go and find the next ingredient.");
        int num = 0;
        string str1 = "";
        string str2 = "";
        if (!this._hasLabHydrogen)
        {
          ++num;
          str1 = "Hydrogen Chlorire";
        }
        if (!this._hasLabAcid)
        {
          ++num;
          if (str1 == "")
            str1 = "Muriatic Acid";
          else
            str2 = "Muriatic Acid";
        }
        if (!this._hasLabSoda)
        {
          ++num;
          if (str1 == "")
            str1 = "Caustic Soda";
          else
            str2 = "Caustic Soda";
        }
        UI.ShowSubtitle(string.Format("Find ~g~{0}~w~{2}~g~{1}~w~.", (object) str1, (object) str2, num > 1 ? (object) " and " : (object) ""), 120000);
      }
    }

    public void SpawnMethIngridients()
    {
      Model mod1 = new Model(-1382355819);
      mod1.Request(10000);
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1397.503f, 3605.191f, 34.96178f), new Vector3(0.0f, 0.0f, 160.0003f), false, false));
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1396.925f, 3606.038f, 38.73773f), new Vector3(0.0f, 0.0f, 85.00015f), false, false));
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1391.589f, 3619.986f, 38.74288f), new Vector3(0.0f, 0.0f, -4.999912f), false, false));
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1381.742f, 3617.712f, 33.8928f), new Vector3(0.0f, 0.0f, -119.9997f), false, false));
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1387.061f, 3622.184f, 34.01186f), new Vector3(0.0f, 0.0f, 165.0003f), false, false));
      this._hydrogen.Add((Entity) this.CreateFixedProp(mod1, new Vector3(1354.962f, 3600.14f, 34.86f), new Vector3(0.0f, 0.0f, 137.399f), false, false));
      foreach (Entity entity in this._hydrogen)
      {
        entity.IsInvincible = true;
        entity.FreezePosition = true;
        entity.AddBlip();
        entity.CurrentBlip.Color = BlipColor.Green;
        entity.CurrentBlip.Scale = 0.5f;
      }
      mod1.MarkAsNoLongerNeeded();
      Model mod2 = new Model(-374844025);
      mod2.Request(10000);
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1401.355f, 3617.822f, 39.33835f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1387.297f, 3612.035f, 38.82689f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1391.455f, 3599.987f, 34.96041f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1367.202f, 3615.53f, 35.00018f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1389.213f, 3598.672f, 35.35949f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      this._causticSoda.Add((Entity) this.CreateFixedProp(mod2, new Vector3(1391.741f, 3623.54f, 34.012f), new Vector3(0.0f, 0.0f, 0.0f), false, false));
      foreach (Entity entity in this._causticSoda)
      {
        entity.IsInvincible = true;
        entity.FreezePosition = true;
        entity.AddBlip();
        entity.CurrentBlip.Color = BlipColor.Green;
        entity.CurrentBlip.Scale = 0.5f;
      }
      mod2.MarkAsNoLongerNeeded();
      Model mod3 = new Model(-1414337382);
      mod3.Request(10000);
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1403.868f, 3626.846f, 34.01229f), new Vector3(0.0f, 0.0f, -165.0003f), false, false));
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1433.273f, 3620.875f, 33.93318f), new Vector3(0.0f, 0.0f, 24.99944f), false, false));
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1387.835f, 3608.558f, 35.29737f), new Vector3(0.0f, 0.0f, 24.99944f), false, false));
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1397.757f, 3603.616f, 33.98091f), new Vector3(0.0f, 0.0f, 14.99944f), false, false));
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1412.307f, 3607.858f, 37.92791f), new Vector3(0.0f, 0.0f, 89.99916f), false, false));
      this._muriaticAcid.Add((Entity) this.CreateFixedProp(mod3, new Vector3(1384.337f, 3619.06f, 37.67938f), new Vector3(0.0f, 0.0f, -75.00084f), false, false));
      foreach (Entity entity in this._muriaticAcid)
      {
        entity.IsInvincible = true;
        entity.FreezePosition = true;
        entity.AddBlip();
        entity.CurrentBlip.Color = BlipColor.Green;
        entity.CurrentBlip.Scale = 0.5f;
      }
      mod3.MarkAsNoLongerNeeded();
      this._hasPlayerHydrogen = false;
      this._hasPlayerAcid = false;
      this._hasPlayerSoda = false;
    }

    private Prop CreateFixedProp(
      Model mod,
      Vector3 pos,
      Vector3 rot,
      bool dynamic,
      bool onGround)
    {
      Prop prop = World.CreateProp(mod, pos, rot, dynamic, onGround);
      prop.FreezePosition = true;
      prop.Position = pos;
      return prop;
    }
  }
}
