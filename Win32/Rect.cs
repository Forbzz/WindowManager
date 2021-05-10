using System;
using System.Runtime.InteropServices;

namespace Gridcore.Win32 {

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect {
        public int Left;

        public int Top;

        public int Right;

        public int Bottom;

        // прямоугольник
        public Rect(Point p1, Point p2) {
            Left = Math.Min(p1.X, p2.X);
            Top = Math.Min(p1.Y, p2.Y);
            Right = Math.Max(p1.X, p2.X);
            Bottom = Math.Max(p1.Y, p2.Y);
        }

        public override string ToString() => $"L:{Left} T:{Top} R:{Right} B:{Bottom}";

        public int Width => Right - Left;
        public int Height => Bottom - Top;
        public int HalfWidth => Width / 2;
        public int HalfHeight => Height / 2;
        public int HCenter => Left + HalfWidth;
        public int VCenter => Top + HalfHeight;

        public Point TopLeft => new Point(Left, Top);
        public Point TopRight => new Point(Right, Top);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point BottomRight => new Point(Right, Bottom);
        public Point Center => new Point(HCenter, VCenter);
        public Point Size => new Point(Width, Height);
    }
}
