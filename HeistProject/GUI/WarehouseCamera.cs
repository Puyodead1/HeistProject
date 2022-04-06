// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace HeistProject.GUI
{
  public static class WarehouseCamera
  {
    private static Vector3 _warehouseCamPos = new Vector3(449.85f, -2997.76f, 43.69f);
    private static Vector3 _warehouseCamTarget = new Vector3(490.78f, -3068.82f, 30.81f);

    public static Camera MainCamera { get; set; }

    public static void StartTransition()
    {
      World.DestroyAllCameras();
      Vector3 position = GameplayCamera.Position;
      Vector3 rotation1 = GameplayCamera.Rotation;
      Vector3 direction = WarehouseCamera._warehouseCamTarget - WarehouseCamera._warehouseCamPos;
      direction.Normalize();
      Vector3 rotation2 = Geometry.DirectionToRotation(direction) with
      {
        X = 0.0f
      };
      WarehouseCamera.MainCamera = World.CreateCamera(position, rotation1, GameplayCamera.FieldOfView);
      World.RenderingCamera = WarehouseCamera.MainCamera;
      int gameTime = Game.GameTime;
      while (Game.GameTime - gameTime <= 5000)
      {
        WarehouseCamera.MainCamera.Position = Util.VectorLerp(position, WarehouseCamera._warehouseCamPos, Game.GameTime - gameTime, 5000, new Func<float, float, int, int, float>(Util.QuadraticEasingLerp));
        WarehouseCamera.MainCamera.Rotation = Util.VectorLerp(rotation1, rotation2, Game.GameTime - gameTime, 5000, new Func<float, float, int, int, float>(Util.QuadraticEasingLerp));
        Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
        Script.Yield();
      }
    }

    public static void Stop() => World.RenderingCamera = (Camera) null;
  }
}
