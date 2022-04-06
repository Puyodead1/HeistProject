// Decompiled with JetBrains decompiler

using GTA;
using GTA.Math;
using GTA.Native;

namespace HeistProject
{
  public static class Geometry
  {
    public static Vector3 ForwardVector(this Vector3 vector, float yaw)
    {
      float num1 = (float) System.Math.Cos((double) yaw + System.Math.PI / 2.0);
      Vector3 right;
      right.X = 57.29578f * num1;
      right.Y = 0.0f;
      float num2 = (float) System.Math.Sin((double) yaw + System.Math.PI / 2.0);
      right.Z = 57.29578f * num2;
      return Geometry.CrossWith(vector, right);
    }

    public static Vector3 CrossWith(Vector3 left, Vector3 right)
    {
      Vector3 vector3;
      vector3.X = (float) ((double) left.Y * (double) right.Z - (double) left.Z * (double) right.Y);
      vector3.Y = (float) ((double) left.Z * (double) right.X - (double) left.X * (double) right.Z);
      vector3.Z = (float) ((double) left.X * (double) right.Y - (double) left.Y * (double) right.X);
      return vector3;
    }

    public static bool WorldToScreenRel(Vector3 worldCoords, out Vector2 screenCoords)
    {
      OutputArgument outputArgument1 = new OutputArgument();
      OutputArgument outputArgument2 = new OutputArgument();
      if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, (InputArgument) worldCoords.X, (InputArgument) worldCoords.Y, (InputArgument) worldCoords.Z, (InputArgument) outputArgument1, (InputArgument) outputArgument2))
      {
        screenCoords = new Vector2();
        return false;
      }
      screenCoords = new Vector2((float) (((double) outputArgument1.GetResult<float>() - 0.5) * 2.0), (float) (((double) outputArgument2.GetResult<float>() - 0.5) * 2.0));
      return true;
    }

    public static Vector3 ScreenRelToWorld(Vector3 camPos, Vector3 camRot, Vector2 coord)
    {
      Vector3 direction = Geometry.RotationToDirection(camRot);
      Vector3 rotation1 = camRot + new Vector3(10f, 0.0f, 0.0f);
      Vector3 rotation2 = camRot + new Vector3(-10f, 0.0f, 0.0f);
      Vector3 rotation3 = camRot + new Vector3(0.0f, 0.0f, -10f);
      Vector3 vector3_1 = Geometry.RotationToDirection(camRot + new Vector3(0.0f, 0.0f, 10f)) - Geometry.RotationToDirection(rotation3);
      Vector3 vector3_2 = Geometry.RotationToDirection(rotation1) - Geometry.RotationToDirection(rotation2);
      double num1 = -Geometry.DegToRad((double) camRot.Y);
      Vector3 vector3_3 = vector3_1 * (float) System.Math.Cos(num1) - vector3_2 * (float) System.Math.Sin(num1);
      Vector3 vector3_4 = vector3_1 * (float) System.Math.Sin(num1) + vector3_2 * (float) System.Math.Cos(num1);
      Vector2 screenCoords1;
      if (!Geometry.WorldToScreenRel(camPos + direction * 10f + vector3_3 + vector3_4, out screenCoords1))
        return camPos + direction * 10f;
      Vector2 screenCoords2;
      if (!Geometry.WorldToScreenRel(camPos + direction * 10f, out screenCoords2))
        return camPos + direction * 10f;
      if ((double) System.Math.Abs(screenCoords1.X - screenCoords2.X) < 0.001 || (double) System.Math.Abs(screenCoords1.Y - screenCoords2.Y) < 0.001)
        return camPos + direction * 10f;
      float num2 = (float) (((double) coord.X - (double) screenCoords2.X) / ((double) screenCoords1.X - (double) screenCoords2.X));
      float num3 = (float) (((double) coord.Y - (double) screenCoords2.Y) / ((double) screenCoords1.Y - (double) screenCoords2.Y));
      return camPos + direction * 10f + vector3_3 * num2 + vector3_4 * num3;
    }

    public static Vector3 RotationToDirection(Vector3 rotation)
    {
      double rad1 = Geometry.DegToRad((double) rotation.Z);
      double rad2 = Geometry.DegToRad((double) rotation.X);
      double num = System.Math.Abs(System.Math.Cos(rad2));
      return new Vector3()
      {
        X = (float) (-System.Math.Sin(rad1) * num),
        Y = (float) (System.Math.Cos(rad1) * num),
        Z = (float) System.Math.Sin(rad2)
      };
    }

    public static Vector3 DirectionToRotation(Vector3 direction)
    {
      direction.Normalize();
      double deg1 = System.Math.Atan2((double) direction.Z, (double) direction.Y);
      int deg2 = 0;
      double deg3 = -System.Math.Atan2((double) direction.X, (double) direction.Y);
      return new Vector3()
      {
        X = (float) Geometry.RadToDeg(deg1),
        Y = (float) Geometry.RadToDeg((double) deg2),
        Z = (float) Geometry.RadToDeg(deg3)
      };
    }

    public static double DegToRad(double deg) => deg * System.Math.PI / 180.0;

    public static double RadToDeg(double deg) => deg * 180.0 / System.Math.PI;

    public static double BoundRotationDeg(double angleDeg)
    {
      int num1 = (int) (angleDeg / 360.0);
      double num2 = angleDeg - (double) (num1 * 360);
      if (num2 < 0.0)
        num2 += 360.0;
      return num2;
    }

    public static Vector3 RaycastEverything(Vector2 screenCoord)
    {
      Vector3 position = GameplayCamera.Position;
      Vector3 rotation = GameplayCamera.Rotation;
      Vector3 world = Geometry.ScreenRelToWorld(position, rotation, screenCoord);
      Vector3 vector3_1 = position;
      Entity entity = (Entity) Game.Player.Character;
      if (Game.Player.Character.IsInVehicle())
        entity = (Entity) Game.Player.Character.CurrentVehicle;
      Vector3 vector3_2 = world - vector3_1;
      vector3_2.Normalize();
      RaycastResult raycastResult = World.Raycast(vector3_1 + vector3_2 * 1f, vector3_1 + vector3_2 * 100f, IntersectOptions.Peds1 | IntersectOptions.Map | IntersectOptions.Mission_Entities | IntersectOptions.Objects | IntersectOptions.Vegetation, entity);
      return raycastResult.DitHitAnything ? raycastResult.HitCoords : position + vector3_2 * 100f;
    }

    public static Vector3 RaycastEverything(
      Vector2 screenCoord,
      Vector3 camPos,
      Vector3 camRot,
      Entity toIgnore)
    {
      Vector3 world = Geometry.ScreenRelToWorld(camPos, camRot, screenCoord);
      Vector3 vector3_1 = camPos;
      Entity entity = toIgnore;
      Vector3 vector3_2 = vector3_1;
      Vector3 vector3_3 = world - vector3_2;
      vector3_3.Normalize();
      RaycastResult raycastResult = World.Raycast(vector3_1 + vector3_3 * 1f, vector3_1 + vector3_3 * 100f, IntersectOptions.Peds1 | IntersectOptions.Map | IntersectOptions.Mission_Entities | IntersectOptions.Objects | IntersectOptions.Vegetation, entity);
      return raycastResult.DitHitAnything ? raycastResult.HitCoords : camPos + vector3_3 * 100f;
    }

    public static Entity RaycastEntity(Vector2 screenCoord, Vector3 camPos, Vector3 camRot)
    {
      Vector3 world = Geometry.ScreenRelToWorld(camPos, camRot, screenCoord);
      Vector3 vector3_1 = camPos;
      Entity character = (Entity) Game.Player.Character;
      Vector3 vector3_2 = vector3_1;
      Vector3 vector3_3 = world - vector3_2;
      vector3_3.Normalize();
      RaycastResult raycastResult = World.Raycast(vector3_1 + vector3_3 * 1f, vector3_1 + vector3_3 * 100f, IntersectOptions.Peds1 | IntersectOptions.Mission_Entities, character);
      return raycastResult.DitHitEntity ? raycastResult.HitEntity : (Entity) null;
    }
  }
}
