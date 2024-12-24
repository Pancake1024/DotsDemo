using Unity.Burst;
using Unity.Entities;

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
            if (stateComp.State == PlayerState.Idle)
            {
                animStateComp.AnimState = PlayerAnimState.Idle;
            }else if (stateComp.State == PlayerState.Move)
            {
                animStateComp.AnimState = PlayerAnimState.Move;
            }else if (stateComp.State == PlayerState.Jumping)
            {
                animStateComp.AnimState = PlayerAnimState.Jumping;
            }else if (stateComp.State == PlayerState.Jump_End)
            {
                animStateComp.AnimState = PlayerAnimState.Jump_End;
            }else if (stateComp.State == PlayerState.CastSkillEnd)
            {
                animStateComp.AnimState = PlayerAnimState.Idle;
            }
            else
            {
                    
            }
        }
    }
}