using UnityEngine;

public class PauseState : State
{
    public PauseState(GamesManager manager) : base(manager) { }

    public override void EnterState()
    {
        Time.timeScale = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Pause State");
        Time.timeScale = 0f;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamesManager.instance.SwitchState<PlayingState>();
        }
    }
}
