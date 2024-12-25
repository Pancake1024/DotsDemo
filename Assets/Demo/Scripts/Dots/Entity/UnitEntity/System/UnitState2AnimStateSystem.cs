using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitState2AnimStateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitStateComponent>();
            RequireForUpdate<UnitAnimStateComponent>();
            RequireForUpdate<UnitAuto2IdleAnimData>();
            RequireForUpdate<UnitWait2DestroyData>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new PlayerState2AnimStateJob();
            job.ScheduleParallel(Dependency).Complete();
        }
    }

    [BurstCompile]
    public partial struct PlayerState2AnimStateJob : IJobEntity
    {
        public void Execute(ref UnitAnimStateComponent animStateComp, 
            ref UnitAuto2IdleAnimData auto2IdleAnimComp, 
            ref UnitWait2DestroyData wait2DestroyComp,
            in UnitStateComponent stateComp, 
            in UnitAttributeComponent attributeComp)
        {
            var state = stateComp.State;        
            if ((state & UnitState.Idle) != 0)
            {
                animStateComp.AnimState = UnitAnimState.Idle;
            }else if ((state & UnitState.Move) != 0)
            {
                animStateComp.AnimState = UnitAnimState.Move;
            }else if ((state & UnitState.Jumping) != 0)
            {
                animStateComp.AnimState = UnitAnimState.Jumping;
            }else if ((state & UnitState.Jump_End) != 0)
            {
                animStateComp.AnimState = UnitAnimState.Jump_End;
            }else if ((state & UnitState.CastSkillEnd) != 0)
            {
                animStateComp.AnimState = UnitAnimState.Idle;
            }else if ((state & UnitState.BeHit) != 0)
            {
                if (animStateComp.AnimState != UnitAnimState.BeHit)
                {
                    auto2IdleAnimComp.Time = 0.3f;    
                }
                animStateComp.AnimState = UnitAnimState.BeHit;
                
            }else if ((state & UnitState.Die) != 0)
            {
                if (animStateComp.AnimState != UnitAnimState.Die)
                {
                    wait2DestroyComp.IsWait = true;
                    wait2DestroyComp.Time = 1.5f;    
                }
                animStateComp.AnimState = UnitAnimState.Die;
            }
            else
            {
                    
            }
        }
    }
}