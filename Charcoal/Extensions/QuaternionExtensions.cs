using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
	public static class QuaternionExtensions
	{
		public static float ArcTanAngle(float x, float y)
		{
			if (x == 0)
			{
				if (y == 1)
					return MathHelper.PiOver2;
				else
					return -MathHelper.PiOver2;
			}
			else if (x > 0)
				return (float)Math.Atan(y / x);
			else
			{
				if (y > 0)
					return (float)Math.Atan(y / x) + MathHelper.Pi;
				else
					return (float)Math.Atan(y / x) - MathHelper.Pi;
			}
		}

		public static Vector3 AngleTo(Vector3 from, Vector3 to)
		{
			var angle = new Vector3();
			var v = Vector3.Normalize(to - from);
			angle.X = (float)Math.Asin(v.Y);
			angle.Y = ArcTanAngle(-v.Z, -v.X);
			return angle;
		}

		public static void ToEuler(float x, float y, float z, float w, ref Vector3 result)
		{
			var rotation = new Quaternion(x, y, z, w);
			var forward = Vector3.Transform(Vector3.Forward, rotation);
			var up = Vector3.Transform(Vector3.Up, rotation);
			result = AngleTo(Vector3.Zero, forward);
			if (result.X == MathHelper.PiOver2)
			{
				result.Y = ArcTanAngle(up.Z, up.X);
				result.Z = 0;
			}
			else if (result.X == -MathHelper.PiOver2)
			{
				result.Y = ArcTanAngle(-up.Z, -up.X);
				result.Z = 0;
			}
			else
			{
				up = Vector3.Transform(up, Matrix.CreateRotationY(-result.Y));
				up = Vector3.Transform(up, Matrix.CreateRotationX(-result.X));
				result.Z = ArcTanAngle(up.Y, -up.X);
			}
		}

		public static void ToEuler(this Quaternion q, ref Vector3 result)
		{
			ToEuler(q.X, q.Y, q.Z, q.W, ref result);
		}

		public static Vector3 ToEuler(this Quaternion q)
		{
			var result = new Vector3();
			ToEuler(q, ref result);
			return result;
		}

		public static Quaternion Euler(this Quaternion q, float x, float y, float z) => Quaternion.CreateFromYawPitchRoll(y, x, z);

		public static Quaternion Euler(this Quaternion q, Vector3 rotation) => Euler(q, rotation.X, rotation.Y, rotation.Z);
	}
}
