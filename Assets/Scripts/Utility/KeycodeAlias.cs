using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeycodeAlias
{
    public static string CheckSpecial(KeyCode keybind)
    {
        switch (keybind)
        {
            case KeyCode.LeftArrow:
                return "←";
            case KeyCode.RightArrow:
                return "→";
            case KeyCode.UpArrow:
                return "↑";
            case KeyCode.DownArrow:
                return "↓";
            case KeyCode.LeftShift:
                return "LS";
            case KeyCode.RightShift:
                return "RS";
            case KeyCode.Escape:
                return "Esc";
            case KeyCode.Space:
                return "⎵";
        }

        return keybind.ToString();
    }
}
