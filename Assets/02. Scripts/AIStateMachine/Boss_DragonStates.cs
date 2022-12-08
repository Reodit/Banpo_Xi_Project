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
                if (entity.HP < 80)
                {
                    entity.ChangeState(Boss_Dragon_States.Flying);
                }

                else
                {
                    entity.ChangeState(Boss_Dragon_States.NormalAttack);
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
            entity.Animator.Play("Attack");
            entity.PrintText("NormalAttack Enter");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f )
            {
                if (entity.MinDistance > 10)
                {
                    entity.ChangeState(Boss_Dragon_States.Idle);

                    entity.Animator.Play("Attack");
                }
            }

            entity.PrintText("NormalAttack Execute");
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("NormalAttack Exit");
        }
    }

    public class Dragon_Breath : State
    {
        public override void Enter(Boss_Dragon entity)
        {

        }

        public override void Execute(Boss_Dragon entity)
        {
            int examScore = 0;

            // 지식이 10이면 획득점수는 10
            if (entity.HP == 10)
            {
                examScore = 10;
            }
            else
            {
                // randIndex가 지식 수치보다 낮으면 6~10점, 지식 수치보다 높으면 1~5점
                // 즉, 지식이 높을수록 높은 점수를 받을 확률이 높다
            }

            // 시험 점수에 따라 다음 행동 설정
            if (examScore <= 3)
            {
                // 술집에 가서 술을 마시는 "HitTheBottle" 상태로 변경
                entity.ChangeState(Boss_Dragon_States.Walk);
            }
            else if (examScore <= 7)
            {
                // 도서관에 가서 공부를 하는 "StudyHard" 상태로 변경
                entity.ChangeState(Boss_Dragon_States.NormalAttack);
            }
            else
            {
                // PC방에 가서 게임을 하는 "PlayAGame" 상태로 변경
                entity.ChangeState(Boss_Dragon_States.Flying);
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("강의실 문을 열고 밖으로 나온다.");
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

    public class Walk : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            //entity.CurrentLocation = Phase.Pub;

            entity.PrintText("술이나 한잔할까? 술집으로 들어간다.");
        }

        public override void Execute(Boss_Dragon entity)
        {
            entity.PrintText("술을 마신다.");

            entity.AP -= 5;


                // 집에 가서 쉬는 "RestAndSleep" 상태로 변경
                entity.ChangeState(Boss_Dragon_States.Idle);
            
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("그만 마셔야지.. 술집에서 나온다.");
        }
    }
}

