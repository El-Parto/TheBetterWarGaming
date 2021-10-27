using UnityEngine;
using Mirror;

public static class ColorReadWriter
{
    public static void WriteColor(this NetworkWriter writer, Color color)
    {
        writer.WriteFloat(color.r);
        writer.WriteFloat(color.g);
        writer.WriteFloat(color.b);
        writer.WriteFloat(color.a);
    }

    public static Color ReadColor(this NetworkReader reader)
    {
        Color color = new Color()
        {
            r = reader.ReadFloat(),
            g = reader.ReadFloat(),
            b = reader.ReadFloat(),
            a = reader.ReadFloat()
        };
        return color;
    }

}
