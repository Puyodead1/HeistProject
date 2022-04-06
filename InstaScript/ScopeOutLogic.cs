// Decompiled with JetBrains decompiler
// Type: InstaScript.ScopeOutLogic
// Assembly: HeistProject, Version=0.4.32.678, Culture=neutral, PublicKeyToken=null
// MVID: 41C549D2-321F-410C-95E1-71855CBC3D16
// Assembly location: C:\Users\23562\Downloads\HeistProject.dll

using GTA;
using GTA.Math;
using GTA.Native;
using HeistProject;
using NativeUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InstaScript
{
  public class ScopeOutLogic : ScriptedLogic
  {
    private Vector3 _fleecaScopePos = new Vector3(1175.03f, 2695.7f, 36.45f);
    private Vector3 _vanPos = new Vector3(693.06f, -1014.96f, 22.22f);
    private float _valHead = 181.16f;
    private Vector3 _camFirstPos = new Vector3(1171.509f, 2706.983f, 39.51627f);
    private Vector3 _camFirstRot = new Vector3(-13.28037f, 0.0f, -125.8339f);
    private Vector3 _camSecondPos = new Vector3(1171.76f, 2712.188f, 39.66f);
    private Vector3 _camSecondRot = new Vector3(-35.48422f, 0.0f, -120.87f);
    private Vector3 _camThirdRot = new Vector3(-35.247f, 0.0f, -50.949f);
    private Sprite _bars;
    private Camera _mainCamera;
    private Blip _destBlip;
    private Vehicle _vehTarget;
    private Ped[] _ambient;
    private int _heistStart;
    private int _msgsShown;
    private int _watchStart;
    private int _watchMsgsShown;
    private bool _oldInCar;

    private int Stage { get; set; }

    public override void Start()
    {
      this.Stage = 0;
      ScriptHandler.Log("STARTING SCOPEOUT SETUP " + (object) DateTime.Now);
      ScriptHandler.Log("Game version: " + (object) Game.Version);
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      this._bars = new Sprite("securitycam", "securitycam_scanlines", new Point(0, 0), new Size((int) resolutionMantainRatio.Width, (int) resolutionMantainRatio.Height));
      this._bars.Visible = false;
      this._ambient = new Ped[5];
      Model[] source = new Model[5]
      {
        new Model(-1868718465),
        new Model(-1371020112),
        new Model(664399832),
        new Model(377976310),
        new Model(-570394627)
      };
      DateTime now = DateTime.Now;
      int num;
      for (num = 0; !((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)) && num <= 200; ++num)
      {
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
        {
          model.Request();
          Script.Yield();
        }
      }
      ScriptHandler.Log("Time elapsed: " + (object) DateTime.Now.Subtract(now).TotalMilliseconds + "ms");
      ScriptHandler.Log("Iterations: " + (object) num);
      if (!((IEnumerable<Model>) source).All<Model>((Func<Model, bool>) (m => m.IsLoaded)))
      {
        ScriptHandler.Log("All models loaded: false. Unloaded models:");
        foreach (Model model in ((IEnumerable<Model>) source).Where<Model>((Func<Model, bool>) (m => !m.IsLoaded)))
          ScriptHandler.Log(model.Hash.ToString());
      }
      else
        ScriptHandler.Log("All models loaded: true");
      ScriptHandler.Log("Spawning entities...");
      this._ambient[0] = World.CreatePed(source[0], new Vector3(1176.413f, 2708.152f, 38.08788f), -173.4103f);
      this._ambient[1] = World.CreatePed(source[1], new Vector3(1174.919f, 2708.194f, 38.0873f), 179.8228f);
      this._ambient[2] = World.CreatePed(source[2], new Vector3(1171.96f, 2705.053f, 38.08799f), -82.64564f);
      this._ambient[3] = World.CreatePed(source[3], new Vector3(1176.46f, 2706.254f, 38.09838f), 1.732682f);
      this._ambient[4] = World.CreatePed(source[4], new Vector3(1175.266f, 2706.466f, 38.09655f), 1.732682f);
      Function.Call(Hash.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP, (InputArgument) this._ambient[3], (InputArgument) this._ambient[3].Position.X, (InputArgument) this._ambient[3].Position.Y, (InputArgument) this._ambient[3].Position.Z, (InputArgument) 5f, (InputArgument) false);
      Function.Call(Hash.TASK_USE_NEAREST_SCENARIO_TO_COORD_WARP, (InputArgument) this._ambient[4], (InputArgument) this._ambient[4].Position.X, (InputArgument) this._ambient[4].Position.Y, (InputArgument) this._ambient[4].Position.Z, (InputArgument) 5f, (InputArgument) false);
      for (int index = 0; index < source.Length; ++index)
        source[index].MarkAsNoLongerNeeded();
      ScriptHandler.Log("Scope out started.");
    }

    public override void End()
    {
      if ((Entity) this._vehTarget != (Entity) null)
        this._vehTarget.Delete();
      if (Blip.op_Inequality(this._destBlip, (Blip) null))
        this._destBlip.Remove();
      for (int index = 0; index < this._ambient.Length; ++index)
      {
        if ((Entity) this._ambient[index] != (Entity) null)
          this._ambient[index].Delete();
      }
      UI.ShowSubtitle(" ");
      Function.Call(Hash._STOP_SCREEN_EFFECT, (InputArgument) "DeathFailMPDark");
      World.RenderingCamera = (Camera) null;
    }

    public override void Update()
    {
      SizeF resolutionMantainRatio = UIMenu.GetScreenResolutionMantainRatio();
      this._bars.Size = new Size((int) resolutionMantainRatio.Width, (int) resolutionMantainRatio.Height);
      this._bars.Draw();
      if (this._bars.Visible)
      {
        Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        ((UIRectangle) new UIResRectangle(new Point(10, 10), new Size(170, 100), Color.Black)).Draw();
        ((UIText) new UIResText(this._mainCamera.Position == this._camFirstPos ? "CAM 01" : "CAM 02", new Point(18, 20), 1.2f, Color.White, GTA.Font.ChaletComprimeCologne, UIResText.Alignment.Left)).Draw();
      }
      if (this.Stage == 0)
      {
        this._destBlip = World.CreateBlip(this._fleecaScopePos);
        this._destBlip.ShowRoute = true;
        this._heistStart = Game.GameTime;
        ++this.Stage;
        UI.ShowSubtitle("Drive to the ~y~Fleeca Bank franchise~w~.", 60000);
      }
      if (this.Stage == 1 && Game.Player.Character.IsInRangeOf(this._fleecaScopePos, 2f) && (Game.Player.Character.IsInVehicle() && (double) Game.Player.Character.CurrentVehicle.Speed < 1.0 || !Game.Player.Character.IsInVehicle()))
      {
        Game.Player.CanControlCharacter = false;
        ++this.Stage;
        this._destBlip.Remove();
        this._watchStart = Game.GameTime;
        World.DestroyAllCameras();
        this._mainCamera = World.CreateCamera(this._camFirstPos, this._camFirstRot, 60f);
        World.RenderingCamera = this._mainCamera;
        Function.Call(Hash._START_SCREEN_EFFECT, (InputArgument) "DeathFailMPDark", (InputArgument) 0, (InputArgument) true);
        UI.ShowSubtitle("");
        this._bars.Visible = true;
      }
      if (this.Stage == 1)
        World.DrawMarker(MarkerType.VerticalCylinder, this._fleecaScopePos, new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
      if (this.Stage == 3 && Game.Player.Character.IsInVehicle(this._vehTarget))
      {
        ++this.Stage;
        this._vehTarget.CurrentBlip.Alpha = 0;
        this._vehTarget.CurrentBlip.ShowRoute = false;
        UI.ShowSubtitle("Drive back to the ~y~warehouse.");
        this._destBlip = World.CreateBlip(new Vector3(453.53f, -3074.97f, 5.1f));
        this._destBlip.ShowRoute = true;
      }
      if (this.Stage == 4 && Game.Player.Character.IsInVehicle(this._vehTarget) && Game.Player.Character.IsInRangeOf(new Vector3(453.53f, -3074.97f, 5.1f), 1.5f))
      {
        this.HasWon = true;
        this.HasFinished = true;
      }
      if (this.Stage != 4 || !Game.Player.Character.IsInVehicle(this._vehTarget))
        return;
      World.DrawMarker(MarkerType.VerticalCylinder, new Vector3(453.53f, -3074.97f, 5.1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), Color.Yellow);
    }

    public override void BackgroundThreadUpdate()
    {
      if (Game.Player.WantedLevel > 0)
      {
        this.HasFinished = true;
        this.FailureReason = "You attracted police attention.";
      }
      if (this.Stage == 1)
      {
        int num = Game.GameTime - this._heistStart;
        if (this._msgsShown == 0 && num > 5000)
        {
          Util.SendLesterMessage("Don't let the cops notice you. If they come after you, we call it off.");
          ++this._msgsShown;
        }
        if (this._msgsShown == 1 && num > 30000)
        {
          Util.SendLesterMessage("You will be driving out to the bank, taking a look at it.");
          ++this._msgsShown;
        }
        if (this._msgsShown == 2 && num > 34000)
        {
          Util.SendLesterMessage("We're not going in, we're not poking it with a stick.");
          ++this._msgsShown;
        }
        if (this._msgsShown == 3 && num > 38000)
        {
          Util.SendLesterMessage("We're just sitting back and taking a look.");
          ++this._msgsShown;
        }
        if (this._msgsShown == 4 && num > 80000)
        {
          Util.SendLesterMessage("I guess you're thinking, \"Why this branch in particular?\". Good question my friend. ");
          ++this._msgsShown;
        }
        if (this._msgsShown == 5 && num > 88000)
        {
          Util.SendLesterMessage("This bank's got some safety deposit boxes and in one of those are some bonds.");
          ++this._msgsShown;
        }
        if (this._msgsShown == 6 && num > 96000)
        {
          Util.SendLesterMessage("Those bonds are being hidden from the taxman. No one's going to report them missing.");
          ++this._msgsShown;
        }
      }
      if (this.Stage == 2)
      {
        int num = Game.GameTime - this._watchStart;
        if (this._watchMsgsShown == 0 && num > 1000)
        {
          Util.SendLesterMessage("Small joint. Not much security. Staff won't give a crap about the deposit boxes.");
          ++this._watchMsgsShown;
        }
        if (num > 10000 && num < 15000)
        {
          this._mainCamera.Position = this._camSecondPos;
          this._mainCamera.Rotation = this._camSecondRot;
        }
        if (num >= 15000 && num <= 25000)
          this._mainCamera.Rotation = Util.LinearVectorLerp(this._camSecondRot, this._camThirdRot, num - 15000, 10000);
        if (this._watchMsgsShown == 1 && num > 16000)
        {
          Util.SendLesterMessage("The boxes are in the back behind a safe door. We want box number 167.");
          ++this._watchMsgsShown;
        }
        if (this._watchMsgsShown == 2 && num >= 30000)
        {
          World.RenderingCamera = (Camera) null;
          this._bars.Visible = false;
          Function.Call(Hash._STOP_SCREEN_EFFECT, (InputArgument) "DeathFailMPDark");
          Game.Player.CanControlCharacter = true;
          ++this.Stage;
          Model model = new Model(VehicleHash.Bison2);
          model.Request(10000);
          this._vehTarget = World.CreateVehicle(model, this._vanPos, this._valHead);
          this._vehTarget.AddBlip();
          this._vehTarget.CurrentBlip.Color = BlipColor.Blue;
          this._vehTarget.CurrentBlip.ShowRoute = true;
          model.MarkAsNoLongerNeeded();
          Util.SendLesterMessage("Now to open that deposit box you're gonna need a drill. I've stashed one in a van near my office.");
          UI.ShowSubtitle("Get in the ~b~Bison.", 120000);
        }
      }
      if (!((Entity) this._vehTarget != (Entity) null) || this.HasFinished)
        return;
      bool flag = Game.Player.Character.IsInVehicle(this._vehTarget);
      if (this.Stage == 4 && this._oldInCar && !flag)
      {
        UI.ShowSubtitle("Get back in the ~b~Bison", 120000);
        this._destBlip.ShowRoute = false;
        this._destBlip.Alpha = 0;
        this._vehTarget.CurrentBlip.Alpha = (int) byte.MaxValue;
      }
      if (((this.Stage != 4 ? 0 : (!this._oldInCar ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        UI.ShowSubtitle("Drive back to the ~y~warehouse.", 120000);
        this._destBlip.ShowRoute = true;
        this._destBlip.Alpha = (int) byte.MaxValue;
        this._vehTarget.CurrentBlip.Alpha = 0;
      }
      this._oldInCar = flag;
      if (!this._vehTarget.IsDead)
        return;
      this.FailureReason = "The tool van has been destroyed.";
      this.HasFinished = true;
    }
  }
}
