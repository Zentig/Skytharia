using System;

[Serializable]
public class PlayerStats
{
    public int Health { get; set; }
    public float Energy { get; set; }
    public int Coins { get; set; }

    public PlayerStats()
    {
        // Default Values Here
        Health = 100;
        Energy = 100;
        Coins = 0;
    }
}