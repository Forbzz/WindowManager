using System.Runtime.InteropServices;

namespace Gridcore.Win32 {
    [StructLayout(LayoutKind.Sequential)]  //Sequential - поля структуры или класса будут располагаться в памяти в том порядке, в котором объявлены в коде, с учётом выравнивания (параметр Pack).
    public struct Point {
        public int X; 
        public int Y; 

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public static Point operator +(Point lhs, Point rhs) {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Point operator -(Point lhs, Point rhs) {
            return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Point operator *(Point lhs, int rhs) {
            return new Point(lhs.X * rhs, lhs.Y * rhs);
        }

        public static Point operator *(int lhs, Point rhs) {
            return new Point(lhs * rhs.X, lhs * rhs.Y);
        }

        public static Point operator /(Point lhs, int rhs) {
            return new Point(lhs.X / rhs, lhs.Y / rhs);
        }
    }
}
