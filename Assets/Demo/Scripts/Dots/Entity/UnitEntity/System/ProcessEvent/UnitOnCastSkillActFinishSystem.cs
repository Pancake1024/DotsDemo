using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitOnCastSkillActFinishSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitStateComponent>();
            RequireForUpdate<CasterActFinishEvent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new OnCastSkillActEndJob();
            
            var jobHandle = job.ScheduleParallel(Dependency);
            jobHandle.Complete();
        }
    }

    [BurstCompile]
    public partial struct OnCastSkillActEndJob : IJobEntity
    {
        public void Execute(DynamicBuffer<CasterActFinishEvent> events,
            ref UnitStateComponent stateComp,
            in Entity entity)
        {
            if (events.Length > 0)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    StateUtility.TryAddState(ref stateComp, UnitState.Idle);
                    Debug.LogError($"OnCastSkillActEndJob Entity Index：{entity.Index}");
                }

                events.Clear();
            }
        }
    }
}