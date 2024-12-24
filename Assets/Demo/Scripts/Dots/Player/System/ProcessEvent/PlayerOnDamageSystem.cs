using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerOnDamageSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerAnimStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new OnDamageEvtJob();

            var jobHandle = job.ScheduleParallel(Dependency);
            jobHandle.Complete();
        }
    }

    [BurstCompile]
    public partial struct OnDamageEvtJob : IJobEntity
    {
        public void Execute(
            DynamicBuffer<DamageEvent> events, 
            ref PlayerAttributeComponent attributeComp, 
            in Entity entity)
        {
            if (events.Length > 0)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    var e= events[i];
                    attributeComp.Health -= e.Damage;
                    Debug.LogError($"OnDamageEvtJob Entity Index：{entity.Index} OnDamageEvent: {e.Damage} Health: {attributeComp.Health}");
                }

                events.Clear();
            }
        }
    }
}