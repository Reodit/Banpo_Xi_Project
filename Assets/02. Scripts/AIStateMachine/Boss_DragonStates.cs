using System.Xml.Linq;
using TMPro;
using UnityEngine;

namespace Boss_DragonStates
{
    public class Idle : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Idle");
            entity.PrintText("Idle Enter");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
        }

        public override void Execute(Boss_Dragon entity)
        {
            if (entity.MinDistance < 10f && entity.Animator.HasState(0, Animator.StringToHash("Idle")))
            {
                entity.ChangeState(Boss_Dragon_States.NormalAttack);
            }


            entity.PrintText("Idle Execute");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("Idle Exit");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
        }
    }

    public class NormalAttack : State
    {
        public override void Enter(Boss_Dragon entity)
        {
            entity.Animator.Play("Attack");
            entity.PrintText("NormalAttack Enter");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
        }

        public override void Execute(Boss_Dragon entity)
        {
            Debug.Log(entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log(entity.MinDistance);

            if (entity.MinDistance > 10f && entity.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                entity.ChangeState(Boss_Dragon_States.Idle);
            }

            entity.PrintText("NormalAttack Enter");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("NormalAttack Enter");
            entity.PrintText($"현재 페이즈 : {entity.CurrentPhase}");
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
                int randIndex = Random.Range(0, 10);
                examScore = randIndex < entity.HP ? Random.Range(6, 11) : Random.Range(1, 6);
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

        }

        public override void Execute(Boss_Dragon entity)
        {
            entity.PrintText("건전하게?? 게임을 즐긴다..");

            int randState = Random.Range(0, 10);
            if (randState == 0 || randState == 9)
            {
                entity.AP += 20;

                // 술집에 가서 술을 마시는 "HitTheBottle" 상태로 변경
                entity.ChangeState(Boss_Dragon_States.Walk);
            }
            else
            {
                entity.AP--;

                if (entity.AP <= 0)
                {
                    // 도서관에 가서 공부를 하는 "StudyHard" 상태로 변경
                    entity.ChangeState(Boss_Dragon_States.NormalAttack);
                }
            }
        }

        public override void Exit(Boss_Dragon entity)
        {
            entity.PrintText("PC방에서 나온다.");
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

