using StatePattern.Enemy;
using System.Collections.Generic;


public class OnePunchManStateMachine 
{
    // Reference to the owner of the state machine
    private OnePunchManController Owner;
    private IState currentState;

    // Dictionary to store available states, mapped to enum values
    protected Dictionary<OnePunchManStates, IState> States = new Dictionary<OnePunchManStates, IState>();

    public OnePunchManStateMachine(OnePunchManController Owner)
    {
        this.Owner = Owner;
        CreateStates();
        SetOwner();
    }
    public void Update() => currentState?.Update();
    // Create and initialize the states
    private void CreateStates()
    {
        States.Add(OnePunchManStates.IDLE, new IdleState(this));
        States.Add(OnePunchManStates.ROTATING, new RotatingState(this));
        States.Add(OnePunchManStates.SHOOTING, new ShootingState(this));
    }
    protected void ChangeState(IState newState)
    {
        currentState?.OnStateExit(); // Exit the current state (if it exists)
        currentState = newState; // Set the new state
        currentState?.OnStateEnter(); // Enter the new state
    }

    // Change the current state to a new state by providing a state enum
    public void ChangeState(OnePunchManStates newState) => ChangeState(States[newState]);
    // Set the owner for each state
    private void SetOwner()
    {
        foreach (IState state in States.Values)
        {
            state.Owner = Owner;
        }
    }
}
