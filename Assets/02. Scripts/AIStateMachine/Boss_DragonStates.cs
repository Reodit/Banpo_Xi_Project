using UnityEngine;
using System;
using Cinemachine;
using ExitGames.Client.Photon.StructWrapping;
using BitStream = Fusion.Protocol.BitStream;
using Random = UnityEngine.Random;

namespace Boss_DragonStates
{
    public class Idle : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.PrintText("Idle Enter");
            entity.Animator.CrossFade("Idle", 0.1f);
            entity.SetCurrentTarget();
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= entity.idleTime &&
                entity.mEnemyAggro.Target &&
                entity.IsCurrentAnimaitionStart)
            {   
                int ranValue = Random.Range(0, 100);

                if (entity.CurrentPhase != Phase.FlyAttackPhase)
                {
                    entity.SetCurrentTarget();
            
                    if (entity.mEnemyAggro.TargetDistance > 2f)
                    {
                        entity.IsPlayerExistNearby = false;
                    }
                    
                    entity.ChangeState(Boss_Dragon_States.Chase);
                }

                else
                {
                    entity.SetCurrentTarget();
            
                    if (entity.mEnemyAggro.TargetDistance > 2f)
                    {
                        entity.IsPlayerExistNearby = false;
                    }
                    
                    if (ranValue < 70)
                    {
                        entity.ChangeState(Boss_Dragon_States.Chase);
                        
                    }

                    else
                    {
                        entity.ChangeState(Boss_Dragon_States.Flying);
                    }
                }
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("Idle Exit");
            entity.mNavMeshAgent.isStopped = false;
            entity.mNavMeshAgent.updateRotation = true;
        }
    }

    public class NormalAttack : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("AttackMouth1", 0.1f);
            entity.PrintText("NormalAttack Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("NormalAttack Exit");
        }
    }

    public class BreathOnAir : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("BreathOnAir1", 0.1f);
            entity.PrintText("BreathOnAir Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("BreathOnAir Exit");
        }
    }

    public class Flying : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("Flying1", 0.1f);
            entity.PrintText("Flying Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.ChaseOnAir);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("Flying Exit");
        }
    }

    public class ClawAttack : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("ClawAttack1", 0.1f);
            entity.PrintText("ClawAttack Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("ClawAttack Exit");
        }
    }
    
    public class Defend : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("Defend1", 0.1f);
            entity.PrintText("Defend Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("Defend Exit");
        }
    }
    
    public class Screaming : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("Screaming1", 0.1f);
            entity.PrintText("Screaming Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }        
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.IsInvincible = false;
            entity.PrintText("Screaming Exit");
        }
    }
    
    public class BreathOnLand : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("BreathOnLand1", 0.1f);
            entity.PrintText("BreathOnLand Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("BreathOnLand Exit");
        }
    }
    
    public class Die : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.CrossFade("Die1", 0.1f);
        }

        public override void Execute(Boss_Dragon entity)
        {
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class Chase : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.PrintText("StartChase");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.mEnemyAggro.Target.transform.position) > 8f)
            {
                entity.Animator.Play("Chase1");
                entity.mNavMeshAgent.SetDestination(entity.mEnemyAggro.Target.transform.position);
                return;
            }

            if (true)
            {
                entity.mNavMeshAgent.velocity = Vector3.zero;
                entity.mNavMeshAgent.isStopped = true;
                entity.mNavMeshAgent.updateRotation = false;

                if (entity.CurrentPhase == Phase.Normal)
                {
                    int ranValue = Random.Range(0, 100);
                    if (ranValue < 50)
                    {
                        entity.ChangeState(Boss_Dragon_States.NormalAttack);
                    }
                    
                    else
                    {
                        entity.ChangeState(Boss_Dragon_States.ClawAttack);
                    }
                }
                
                else if (entity.CurrentPhase == Phase.FireAttackPhase)
                {
                    int ranValue = Random.Range(0, 100);
                    if (ranValue < 25)
                    {
                        entity.ChangeState(Boss_Dragon_States.NormalAttack);
                    }
                    
                    else if (ranValue < 50 &&
                             ranValue >= 25)
                    {
                        entity.ChangeState(Boss_Dragon_States.ClawAttack);
                    }
                    
                    else if (ranValue < 100 &&
                             ranValue >= 50)
                    {
                        entity.ChangeState(Boss_Dragon_States.BreathOnLand);
                    }
                }
                
                else if (entity.CurrentPhase == Phase.FlyAttackPhase)
                {
                    int ranValue = Random.Range(0, 100);
                    if (ranValue < 33)
                    {
                        entity.ChangeState(Boss_Dragon_States.ClawAttack);
                    }
                    
                    else if (ranValue < 100 &&
                             ranValue >= 33)
                    {
                        entity.ChangeState(Boss_Dragon_States.BreathOnLand);
                    }
                }
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("EndChase");
        }
    }
    
    public class ChaseOnAir : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.PrintText("StartChaseOnAir");

            if (!entity.IsPlayerExistNearby)
            {
                entity.Animator.CrossFade("ChaseOnAir1", 0.1f); 
                entity.DestinationCoroutineStart();
            }
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.IsCurrentAnimaitionStart &&
                (int)entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0 &&
                entity.mNavMeshAgent.remainingDistance < 0.1f ||
                entity.IsPlayerExistNearby)
            {
                entity.ChangeState(Boss_Dragon_States.BreathOnAir);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.IsPlayerExistNearby = false;
            entity.PrintText("ChaseOnAir Exit");
        }
    }
}

