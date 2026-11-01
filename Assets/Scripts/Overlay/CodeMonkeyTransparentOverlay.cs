using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.EventSystems;

public class CodeMonkeyTransparentOverlay : TransparentOverlay
{
    // Using Player -> Resolution -> Fullscreen Window
    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    const uint WS_CAPTION = WS_EX_LAYERED | WS_EX_TRANSPARENT;

    const uint LWA_COLORKEY = 0x00000001;
    const uint LWA_ALPHA = 0x00000002;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    IntPtr hwnd;

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll")]
    static extern int GetWindowLong(
        IntPtr hWnd,
        int nIndex
    );

    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT pt);

    [DllImport("Dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

    void Start()
    {
        // IntPtr hwnd = GetActiveWindow();
        // MessageBox(new IntPtr(0), "CodeMonkeyTransparentOverlay", "CodeMonkeyTransparentOverlay", 0);

        #if !UNITY_EDITOR
        // Set the window to have transparent background and no border
        hwnd = GetActiveWindow();

        MARGINS margins = new MARGINS()
        {
            cxLeftWidth = -1
        };

        DwmExtendFrameIntoClientArea(hwnd, ref margins);

        // Make it click-through
        int style = SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        // Make it not click-through
        // int style = SetWindowLong(hwnd, GWL_EXSTYLE, WS_EX_LAYERED);

        // SetClickThrough(false);

        // Not click through if there is color
        // SetLayeredWindowAttributes(hwnd, 0, 0, LWA_COLORKEY);
        // Mot click through if not transparent
        SetLayeredWindowAttributes(hwnd, 0, 255, LWA_ALPHA);        

        // Make it always on top
        // bool onTop = SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
        #endif

        // Make it run in background, can also be config in Player
        Application.runInBackground = true;
    }

    // void Update()
    // {
    //     Vector2 mouseScreenPos = GetMousePosition();

    //     Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

    //     bool overSprite = Physics2D.OverlapPoint(mouseWorldPos);

    //     bool overUI = EventSystem.current.IsPointerOverGameObject();

    //     SetClickThrough(!(overSprite || overUI));
    // }

    public void SetClickThrough(bool clickThrough)
    {
        int style = GetWindowLong(
            hwnd,
            GWL_EXSTYLE
        );

        if (clickThrough)
        {
            style |= (int)WS_EX_TRANSPARENT;
        }
        else
        {
            style &= ~(int)WS_EX_TRANSPARENT;
        }

        style |= (int)WS_EX_LAYERED;

        SetWindowLong(
            hwnd,
            GWL_EXSTYLE,
            (uint)style
        );
    }

    public static Vector2 GetMousePosition()
    {
        POINT point;
        GetCursorPos(out point);

        return new Vector2(point.X, point.Y);
    }

    public void DebugMessage(string content)
    {
        Debug.Log(content);
    }
}
