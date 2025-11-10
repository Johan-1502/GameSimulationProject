using UnityEngine;

public class TimeConversor
{
    public static float transform(float number)
    {
        if (number < 0.25)
        {
            return 1f;
        }
        else if (number < 0.5)
        {
            return 2f;
        }
        else if (number < 0.75)
        {
            return 3f;
        }
        else
        {
            return 4f;
        }
    }
    public static float transform2(float number)
    {
        return 1 + (2-1)*number;
    }
}
