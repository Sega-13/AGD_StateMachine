

using StatePattern.Enemy;
using StatePattern.Main;
using StatePattern.Player;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.GraphicsBuffer;

public class ShootingState : IState
{
    public OnePunchManController Owner { get; set; }
    private OnePunchManStateMachine stateMachine;
    private float shootTimer;
    private PlayerController target;

    public ShootingState(OnePunchManStateMachine stateMachine) => this.stateMachine = stateMachine;
    public void OnStateEnter()
    {
        SetTarget();
        shootTimer = 0;
    }

    public void OnStateExit() =>target = null;
   
    public void Update()
    {
        Quaternion desiredRotation = CalculateRotationTowardsPlayer();
        Owner.SetRotation(RotateTowards(desiredRotation));
        if (IsFacingPlayer(desiredRotation))
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ResetTimer();
                Owner.Shoot();
            }
        }

    }
    private void SetTarget() => target = GameService.Instance.PlayerService.GetPlayer();
    private Quaternion CalculateRotationTowardsPlayer()
    {
        Vector3 directionToPlayer = target.Position - Owner.Position;
        directionToPlayer.y = 0f;
        return Quaternion.LookRotation(directionToPlayer, Vector3.up);
    }
    private bool IsFacingPlayer(Quaternion desiredRotation) => Quaternion.Angle(Owner.Rotation, desiredRotation) < Owner.Data.RotationThreshold;
    private Quaternion RotateTowards(Quaternion desiredRotation) => Quaternion.LerpUnclamped(Owner.Rotation, desiredRotation, Owner.Data.RotationSpeed / 30 * Time.deltaTime);
    private void ResetTimer() => shootTimer = Owner.Data.RateOfFire;
}
