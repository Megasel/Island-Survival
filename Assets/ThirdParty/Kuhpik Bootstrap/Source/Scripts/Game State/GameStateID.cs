﻿namespace Kuhpik
{
    public enum GameStateID
    {
        // Don't change int values in the middle of development.
        // Otherwise all of your settings in inspector can be messed up.

        Loading = 0,
        Game = 1,
        WorkBenchUpgrade=2,
        Pause = 3,
        NextIsland=4,
        Result = 10,
        Shop = 20,
        Settings = 30

        // Extend just like that
        //
        // Revive = 100,
        // QTE = 200
    }
}