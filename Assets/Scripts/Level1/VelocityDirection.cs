using UnityEngine;

public class VelocityDirection
{
    private int velocityDirection;

    public VelocityDirection(int velocity)
    {
        velocityDirection = velocity;
    }

    public int getVelocity()
    {
        return velocityDirection;
    }

    public void setVelocity(int velocity)
    {
        velocityDirection = velocity;
    }
}
