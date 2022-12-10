using Unity.Rendering;
using UnityEngine;

public class Utilities
{
    public static Color RandomColor(float hue)
    {
        //Note: if you are not familiar with this concept, this is a "local function".
        //You can search for that term on the internet for more information.
        // 0.618034005f == 2 / (math.sqrt(5) + 1) == inverse of the golden ratio

        hue = (hue + 0.618034005f) % 1;
        var color = Color.HSVToRGB(hue, 1.0f, 1.0f);
        return color;
    }
}
