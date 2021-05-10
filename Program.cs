using Gridcore.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


using static Gridcore.Win32.User32;

namespace Gridcore {

    public class Gridcore {
        private static readonly BitArray mPressedKeys = new BitArray(256);

        private static List<HotkeyAction> mHotkeyActions = new List<HotkeyAction>();

        private static bool mDebugKeys = false;

        private static IntPtr wmKeyDown = new IntPtr(0x100);
        private static IntPtr wmSysKeyDown = new IntPtr(0x104);

        private static IntPtr LowLevelKeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam){
            if (nCode < 0)
            {
                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }
            //AddWindowsToList();
            var key = Marshal.ReadInt32(lParam);

            var down = wParam == wmKeyDown;
            if (mDebugKeys) {
                Console.WriteLine((VK) key + (down ? " DOWN" : " UP"));
            }
            mPressedKeys[key] = down;

            foreach (var hotkeyAction in mHotkeyActions) {
                if (hotkeyAction.BitArray.BitwiseEquals(mPressedKeys)) {
                    hotkeyAction.Action();
                   

                    if (mDebugKeys) {
                        Console.WriteLine($"Handled {hotkeyAction.BitArray.ToKeyString()}");
                    }
                    return new IntPtr(1);
                }
            }
            if (mDebugKeys) {
                Console.WriteLine($"Ignored {mPressedKeys.ToKeyString()}");
            }

            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static Action SetWindowPosAction(Func<Rect, Rect> workAreaToWindowPos) {
            return () => {
                var foregroundWindow = GetForegroundWindow();
                
                var monitorInfo = new MonitorInfoEx().Init();
                GetMonitorInfo(MonitorFromWindow(foregroundWindow, MonitorDefault.Primary), ref monitorInfo);
                SetWindowPos(foregroundWindow, workAreaToWindowPos(monitorInfo.WorkArea));
            };
        }

        private static Action SetWindowPosActionTop(Func<Rect, Rect> workAreaToWindowPos)
        {
            return () => {
                var foregroundWindow = GetForegroundWindow();

                var monitorInfo = new MonitorInfoEx().Init();
                GetMonitorInfo(MonitorFromWindow(foregroundWindow, MonitorDefault.Primary), ref monitorInfo);
                SetWindowPosTop(foregroundWindow, workAreaToWindowPos(monitorInfo.WorkArea));
            };
        }

        private static Action SetWindowPosActionBottom(Func<Rect, Rect> workAreaToWindowPos)
        {
            return () => {
                var foregroundWindow = GetForegroundWindow();

                var monitorInfo = new MonitorInfoEx().Init();
                GetMonitorInfo(MonitorFromWindow(foregroundWindow, MonitorDefault.Primary), ref monitorInfo);
                SetWindowPosBottom(foregroundWindow, workAreaToWindowPos(monitorInfo.WorkArea));
            };
        }

        private static Action SwitchWindow(Func<Rect, Rect> workAreaToWindowPos)
        {
            return () =>
            {
                var foregroundWindow = GetForegroundWindow();
                var monitorInfo = new MonitorInfoEx().Init();
                GetMonitorInfo(MonitorFromWindow(foregroundWindow, MonitorDefault.Primary), ref monitorInfo);
                SwitchMainWindow(workAreaToWindowPos(monitorInfo.WorkArea));
            };
            
        }

        private static Action TopLeft = SetWindowPosAction(workArea => new Rect(workArea.TopLeft, workArea.Center));
        private static Action TopRight = SetWindowPosAction(workArea => new Rect(workArea.TopRight, workArea.Center));
        private static Action BottomLeft = SetWindowPosAction(workArea => new Rect(workArea.BottomLeft, workArea.Center));
        private static Action BottomRight = SetWindowPosAction(workArea => new Rect(workArea.BottomRight, workArea.Center));
        private static Action Top = SetWindowPosAction(workArea => new Rect(workArea.TopLeft, new Point(workArea.Right, workArea.VCenter)));
        private static Action Bottom = SetWindowPosAction(workArea => new Rect(workArea.BottomLeft, new Point(workArea.Right, workArea.VCenter)));
        private static Action Left = SetWindowPosAction(workArea => new Rect(workArea.TopLeft, new Point(workArea.HCenter, workArea.Bottom)));
        private static Action Right = SetWindowPosAction(workArea => new Rect(workArea.TopRight, new Point(workArea.HCenter, workArea.Bottom)));
        private static Action TopPos = SetWindowPosActionTop(workArea => new Rect(workArea.TopLeft, workArea.Center));
        private static Action BottomPos = SetWindowPosActionBottom((workArea => new Rect(workArea.TopLeft, workArea.Center)));
        private static Action ShowWindow = SwitchWindow(workArea => new Rect(workArea.TopLeft, workArea.Center));

        private static void PrintCurrentMonitor() {
            var foregroundWindow = GetForegroundWindow();
            var monitorInfo = new MonitorInfoEx().Init();
            GetMonitorInfo(MonitorFromWindow(foregroundWindow, MonitorDefault.Primary), ref monitorInfo);
            Console.WriteLine(monitorInfo.DeviceName + " " + monitorInfo.WorkArea);
        }


        public static void Main(string[] args) {

            mHotkeyActions = new List<HotkeyAction>() {
                new HotkeyAction(ShowWindow, VK.LeftControl, VK.S),
                new HotkeyAction(AddWindowsToList, VK.LeftControl, VK.P),
                new HotkeyAction(DisplayWindows, VK.LeftControl, VK.N0),
                new HotkeyAction(PrintCurrentMonitor, VK.LeftControl, VK.P),
                new HotkeyAction(CloseWindow, VK.LeftControl, VK.E),
                new HotkeyAction(() => mDebugKeys = !mDebugKeys, VK.LeftControl, VK.K),
                new HotkeyAction(TopPos, VK.LeftControl, VK.Numpad5),
                new HotkeyAction(TopPos, VK.LeftControl, VK.N5),
                new HotkeyAction(BottomPos, VK.LeftControl, VK.T),
                new HotkeyAction(TopLeft, VK.LeftControl, VK.Numpad7),
                new HotkeyAction(TopRight, VK.LeftControl, VK.Numpad9),
                new HotkeyAction(BottomLeft, VK.LeftControl, VK.Numpad1),
                new HotkeyAction(BottomRight, VK.LeftControl, VK.Numpad3),
                new HotkeyAction(Top, VK.LeftControl, VK.Numpad8),
                new HotkeyAction(Bottom, VK.LeftControl, VK.Numpad2),
                new HotkeyAction(Left, VK.LeftControl, VK.Numpad4),
                new HotkeyAction(Right, VK.LeftControl, VK.Numpad6),

                new HotkeyAction(TopLeft, VK.LeftControl, VK.N7),
                new HotkeyAction(TopRight, VK.LeftControl, VK.N9),
                new HotkeyAction(BottomLeft, VK.LeftControl, VK.N1),
                new HotkeyAction(BottomRight, VK.LeftControl, VK.N3),
                new HotkeyAction(Top, VK.LeftControl, VK.N8),
                new HotkeyAction(Bottom, VK.LeftControl, VK.N2),
                new HotkeyAction(Left, VK.LeftControl, VK.N4),
                new HotkeyAction(Right, VK.LeftControl, VK.N6),
            };

            // отлавливает нажатия клавиш, т.к. веден код 13

            using (var curProcess = Process.GetCurrentProcess()) {
                using (var curModule = curProcess.MainModule) {
                    SetWindowsHookEx(13, LowLevelKeyboardCallback, IntPtr.Zero, 0);
                }
            }

            Msg msg;
            int flag = 0;
            Console.WriteLine("Press a hotkey!");
            AddWindowsToList();
            while ( (flag = GetMessage(out msg, IntPtr.Zero, 0, 0)) > 0) {
                AddWindowsToList();

            }
        }
    }
}
