using UnityEngine;
using System;

namespace Boss_DragonStates
{
    public class Idle : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Idle");
            entity.PrintText("Idle Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.MinDistance < 10f &&
                entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                entity.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                if (entity.HP < 80 &&
                    entity.HP >= 70)
                {
                    entity.ChangeState(Boss_Dragon_States.Flying);
                }

                else if (entity.HP >= 80)
                {
                    entity.ChangeState(Boss_Dragon_States.NormalAttack);
                }
                
                else if (entity.HP < 70 &&
                         entity.HP >= 60)
                {
                    entity.ChangeState(Boss_Dragon_States.Defend);
                }
                
                else if (entity.HP < 60 &&
                         entity.HP >= 50)
                {
                    entity.ChangeState(Boss_Dragon_States.ClawAttack);
                }
                
                else if (entity.HP < 50 &&
                         entity.HP >= 40)
                {
                    entity.ChangeState(Boss_Dragon_States.Chase);
                }
                
                else if (entity.HP < 40 &&
                         entity.HP >= 30)
                {
                    entity.ChangeState(Boss_Dragon_States.Die);
                }
                
                else if (entity.HP < 30 &&
                         entity.HP >= 20)
                {
                    entity.ChangeState(Boss_Dragon_States.Screaming);
                }
                
                else if (entity.HP < 20 &&
                         entity.HP >= 10)
                {
                    entity.ChangeState(Boss_Dragon_States.BreathOnAir);
                }
                
                else if (entity.HP < 10 &&
                         entity.HP >= 0)
                {
                    entity.ChangeState(Boss_Dragon_States.BreathOnLand);
                }
            }

            entity.PrintText("Idle Execute");
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("Idle Exit");
        }
    }

    public class NormalAttack : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("AttackMouth1");
            entity.PrintText("NormalAttack Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f )
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }

            entity.PrintText("NormalAttack Execute");
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
            entity.Animator.Play("BreathOnAir1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f )
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }

    public class Flying : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            
            entity.Animator.Play("Flying1");
            entity.PrintText("Flying Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }

            entity.PrintText("Flying Execute");
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.animationNormalValue = 0f;
            entity.PrintText("Flying Exit");
        }
    }

    public class ClawAttack : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("ClawAttack1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class Defend : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Defend1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class Screaming : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Screaming1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }        
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class BreathOnLand : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("BreathOnLand1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class Die : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Die1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }        
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
    
    public class Chase : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Chase1");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }          
        }

        public override void Exit(Boss_Dragon entity)
        {
        }
    }
}

