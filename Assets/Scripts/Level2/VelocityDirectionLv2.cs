using UnityEngine;

public class VelocityDirectionLv2
{
    private int velocityDirection;

    public VelocityDirectionLv2(int velocity)
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
