using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitAuto2IdleStateSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitStateComponent>();
            RequireForUpdate<UnitAnimStateComponent>();
            RequireForUpdate<UnitAuto2IdleAnimData>();
        }
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new UnitAuto2IdleStateJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct UnitAuto2IdleStateJob : IJobEntity
    {
        public float deltaTime;
        
        public void Execute(ref UnitStateComponent stateComp, 
            ref UnitAuto2IdleAnimData animData, 
            in UnitAnimStateComponent animStateComp)
        {
            if (animStateComp.AnimState == UnitAnimState.BeHit)
            {
                if (animData.Time > 0)
                {
                    animData.Time -= deltaTime;
                    if (animData.Time <= 0)
                    {
                        StateUtility.TryAddState(ref stateComp, UnitState.Idle);
                    }
                }
            }
        }
    }
}