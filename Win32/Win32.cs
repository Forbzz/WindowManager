
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace Gridcore.Win32 {

    public static class HWND
    {
        public static IntPtr
        NoTopMost = new IntPtr(-2),
        TopMost = new IntPtr(-1),
        Top = new IntPtr(0),
        Bottom = new IntPtr(1);
    }


    public enum CmdShow {
        Hide = 0,
        Normal = 1,
        ShowMinimized = 2,
        Maximize = 3, 
        ShowMaximized = 3,
        ShowNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActive = 7,
        ShowNA = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimize = 11
    }

    public enum MonitorDefault {
        Null = 0,
        Primary = 1,
        Nearest = 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Msg {
        IntPtr hwnd;
        uint message;
        UIntPtr wParam;
        IntPtr lParam;
        int time;
        Point pt;

    }

    public static class User32 {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        private const int SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        public static List<IntPtr> windows = new List<IntPtr>();

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        // переправляет информацию о фильтре (hook)  в следующий фильтр (hook)  в текущей цепочке фильтров событий. Процедура фильтров (hook) может вызвать эту функцию или до или после обработки информации о фильтре (hook).

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        // Обычно она  используется, чтобы доставить сообщение, извлеченное функцией GetMessage

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref Msg lpmsg);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int len, StringBuilder st);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg);


        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // извлекает сообщение из очереди сообщений вызывающего потока и помещает его в заданную структуру. 

        [DllImport("user32.dll")]
        public static extern int GetMessage(out Msg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hwnd, out Rect lprect);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorDefault dwFlags);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        // устанавливает определяемую программой процедуру фильтра (hook) в цепочку фильтров (hook).

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, CmdShow nCmdShow);

        // переводит сообщения виртуальных клавиш в символьные сообщения.

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref Msg lpMsg);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        public static bool SetWindowPos(IntPtr hWnd, Rect position) {
            return SetWindowPos(hWnd, position, SWP_NOZORDER);
        }

        public static bool SetWindowPosTop(IntPtr hWnd, Rect position)
        {
            return SetWindowPosTop(hWnd, position, 0x0001 | 0x0002);
        }

        public static bool SetWindowPosBottom(IntPtr hWnd, Rect position)
        {
            return SetWindowPosBottom(hWnd, position, 0x0001 | 0x0002);
        }

        public static void CloseWindow()
        {
            var window = GetForegroundWindow();
            SendMessage(window, 0x0010);
        }

        public static bool SetWindowPos(IntPtr hWnd, Rect position, uint flags) {
            return SetWindowPos(
                hWnd,
                HWND.NoTopMost,
                position.Left,
                position.Top,
                position.Width,
                position.Height,
                flags);
        }

        public static bool SetWindowPosTop(IntPtr hWnd, Rect position, uint flags)
        {
            return SetWindowPos(
                hWnd,
                HWND.TopMost,
                position.Left,
                position.Top,
                position.Width,
                position.Height,
                0x0001 | 0x0002);
        }

        public static bool SetWindowPosBottom(IntPtr hWnd, Rect position, uint flags)
        {
            return SetWindowPos(
                hWnd,
                HWND.NoTopMost,
                position.Left,
                position.Top,
                position.Width,
                position.Height,
                0x0001 | 0x0002);
        }

        public static void DisplayWindows()
        {
            var currentWindow = GetWindowText(GetForegroundWindow());
            Console.WriteLine("========================================");
            Console.WriteLine(currentWindow);
            Console.WriteLine("========================================");

            foreach(var item in windows)
            {
                Console.WriteLine(GetWindowText(item));
            }
        }

        private static string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }

        public static void AddWindowsToList()
        {
            windows.Clear();
            IntPtr shellWindow = GetShellWindow();
            EnumWindows((hWnd, lParam) =>
            {
                if (hWnd == shellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;
                if (GetWindowText(hWnd) == "Калькулятор" || GetWindowText(hWnd) == "Параметры" || GetWindowText(hWnd) == "Microsoft Store" || 
                GetWindowText(hWnd) == "Microsoft Text Input Application" || GetWindowText(hWnd) == "")
                    return true;

                StringBuilder builder = new StringBuilder(length);
                GetWindowText(hWnd, builder, length + 1);

                windows.Add(hWnd);
                return true;

            }, IntPtr.Zero);

        }

        public static void SwitchMainWindow(Rect position)
        {
            IntPtr hWnd = GetForegroundWindow();
            bool find = false;
            for(int i = 0;i<windows.Count;i++)
            {
                if (windows[i] == hWnd)
                {
                    //Console.WriteLine("Нашёл");
                    if(i == windows.Count-1)
                    {
                        i = -1;
                        find = true;
                        continue;
                    }
                    find = true;
                        continue;
                }

                if (find == true)
                {
                    Console.WriteLine("============");
                    Console.WriteLine(GetWindowText(windows[i]));
                    //SetWindowPos(windows[i], position);
                    SetActiveWindow(windows[i]);
                    SetForegroundWindow(windows[i]);
                    break;
                }
                
            }
        }
    }
}
