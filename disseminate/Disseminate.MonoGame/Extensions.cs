using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaMath = Microsoft.Xna.Framework;

public static class MonoGameExtensions
{
    public static XnaMath.Rectangle ToRect(this AxisAlignedBox2 aabb)
    {
        return new XnaMath.Rectangle(
            (int)System.Math.Floor(aabb.X0), (int)System.Math.Floor(aabb.Y0),
            (int)System.Math.Ceiling(aabb.Width), (int)System.Math.Ceiling(aabb.Height));
    }

    public static XnaMath.Rectangle ToRectangle(this Vector2 p, Vector2 size)
    {
        return new XnaMath.Rectangle(
            (int)System.Math.Floor(p.X), (int)System.Math.Floor(p.Y),
            (int)System.Math.Ceiling(size.X), (int)System.Math.Ceiling(size.Y));
    }

    public static XnaMath.Color ToXnaColor(this Color color)
    {
        return new XnaMath.Color(color.r, color.g, color.b, color.a);
    }

    public static XnaMath.Vector2 ToXnaVector2(this Vector2 p)
    {
        return new XnaMath.Vector2(p.X, p.Y);
    }

    public static XnaMath.Matrix ToXnaMatrix(this Matrix4 m)
    {
        return new XnaMath.Matrix(
                m.m00, m.m01, m.m02, m.m03,
                m.m10, m.m11, m.m12, m.m13,
                m.m20, m.m21, m.m22, m.m23,
                m.m30, m.m31, m.m32, m.m33
            );
    }

    public static Color ToColor(this XnaMath.Color color)
    {
        return new Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }
}

internal static class ImageEx
{

    // RGB to BGR convert Matrix
    private static float[][] rgbtobgr = new float[][]
              {
                     new float[] {0, 0, 1, 0, 0},
                     new float[] {0, 1, 0, 0, 0},
                     new float[] {1, 0, 0, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
              };

#if WINRT

#else
    internal static void RGBToBGR(this System.Drawing.Image bmp)
    {
        System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
        System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix(rgbtobgr);

        ia.SetColorMatrix(cm);
        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
        {
            g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, System.Drawing.GraphicsUnit.Pixel, ia);
        }
    }
#endif

}
