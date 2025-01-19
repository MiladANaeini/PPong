using UnityEngine;

public abstract class State
{
    protected GamesManager gamesManager;

    public State(GamesManager manager)
    {
        gamesManager = manager;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}
