using UnityEngine;

public static class Utils
{
    public static void SetResolution(string resolution)
    {
        string[] data = resolution.Split('x');
        Screen.SetResolution(int.Parse(data[0]), int.Parse(data[1]), Screen.fullScreen);
    }

    public static void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
