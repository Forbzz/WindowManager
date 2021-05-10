using System;
using System.Runtime.InteropServices;

namespace Gridcore.Win32 {

    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorInfoEx {
        // size of a device name string
        private const int CCHDEVICENAME = 32;

        public int Size;

        public Rect Monitor;

        public Rect WorkArea;

        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
        public string DeviceName;

        //public int CompareTo(MonitorInfoEx other) =>
        //    WorkArea.Left != other.WorkArea.Left
        //        ? WorkArea.Left.CompareTo(other.WorkArea.Left)
        //        : WorkArea.Top.CompareTo(other.WorkArea.Top);

        public MonitorInfoEx Init() {
            this.Size = 40 + 2 * CCHDEVICENAME;
            this.DeviceName = string.Empty;
            return this;
        }
    }
}
