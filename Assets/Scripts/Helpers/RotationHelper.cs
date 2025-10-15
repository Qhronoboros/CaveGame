public static class RotationHelper
{
    // Clamps the value between 0 and 360
    public static float ClampDegrees(float value)
    {
        if (value < 0.0f)
            value += 360.0f;
        else if (value > 360)
            value %= 360;
            
        return value;
    }
}

