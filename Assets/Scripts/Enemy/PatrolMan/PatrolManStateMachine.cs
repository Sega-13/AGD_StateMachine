using StatePattern.Enemy;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StatePattern.Enemy
{
    public class PatrolManStateMachine : IStateMachine
    {
        private PatrolManController Owner;
        private IState currentState;
        protected Dictionary<States, IState> States = new Dictionary<States, IState>();

        public PatrolManStateMachine(PatrolManController Owner)
        {
            this.Owner = Owner;
            CreateStates();
            SetOwner();
        }
        private void CreateStates()
        {
            States.Add(StatePattern.Enemy.States.IDLE, new IdleState(this));
            States.Add(StatePattern.Enemy.States.ROTATING, new RotatingState(this));
            States.Add(StatePattern.Enemy.States.PATROLLING, new PatrollingState(this));
            States.Add(StatePattern.Enemy.States.CHASING, new ChasingState(this));
            States.Add(StatePattern.Enemy.States.SHOOTING, new  ShootingState(this));
        }
        private void SetOwner()
        {
            foreach(IState state in States.Values)
            {
                state.Owner = Owner;
            }
        }
        public void ChangeState(IState newState)
        {
            currentState?.OnStateExit();
            currentState = newState;
            currentState?.OnStateEnter();
        }
        public void Update() => currentState?.Update();

        public void ChangeState(States newState) => ChangeState(States[newState]);
       
    }
}

