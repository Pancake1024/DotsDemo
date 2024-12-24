using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerState2AnimStateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerStateComponent>();
            RequireForUpdate<PlayerAnimStateComponent>();
        }

        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerState2AnimStateJob();
            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct PlayerState2AnimStateJob : IJobEntity
    {
        public void Execute(ref PlayerAnimStateComponent animStateComp, in PlayerStateComponent stateComp)
        {
            var state = stateComp.State;        
            if ((state & PlayerState.Idle) != 0)
            {
                animStateComp.AnimState = PlayerAnimState.Idle;
            }else if ((state & PlayerState.Move) != 0)
            {
                animStateComp.AnimState = PlayerAnimState.Move;
            }else if ((state & PlayerState.Jumping) != 0)
            {
                animStateComp.AnimState = PlayerAnimState.Jumping;
            }else if ((state & PlayerState.Jump_End) != 0)
            {
                animStateComp.AnimState = PlayerAnimState.Jump_End;
            }else if ((state & PlayerState.CastSkillEnd) != 0)
            {
                animStateComp.AnimState = PlayerAnimState.Idle;
            }
            else
            {
                    
            }
        }
    }
}