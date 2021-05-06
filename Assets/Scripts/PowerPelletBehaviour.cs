using System;

public class PowerPelletBehaviour : DotBehaviour
{
    public static event Action OnPowerPelletEaten;

    protected override void Eat()
    {
        OnPowerPelletEaten?.Invoke();
        base.Eat();
    }
}