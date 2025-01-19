using UnityEngine;

public class PlayingState : State
{
    public PlayingState(GamesManager manager) : base(manager) { }

    public override void EnterState()
    {
        Debug.Log("Entered Playing State");
        Time.timeScale = 1f;
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Playing State");
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamesManager.instance.SwitchState<PauseState>();
        }
    }
}
