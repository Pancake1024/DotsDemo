using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitOnDamageSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitAnimStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new OnDamageEvtJob();
            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct OnDamageEvtJob : IJobEntity
    {
        public void Execute(
            DynamicBuffer<DamageEvent> events, 
            ref UnitAttributeComponent attributeComp, 
            ref UnitStateComponent stateComp,
            in Entity entity)
        {
            if (events.Length > 0)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    var e= events[i];
                    attributeComp.Health -= e.Damage;
                    Debug.LogError($"OnDamageEvtJob Entity Index：{entity.Index} OnDamageEvent: {e.Damage} Health: {attributeComp.Health}");
                    if (attributeComp.Health > 0)
                    {
                        StateUtility.TryAddState(ref stateComp, UnitState.BeHit);
                    }
                    else
                    {
                        StateUtility.TryAddState(ref stateComp, UnitState.Die);
                    }
                }

                events.Clear();
            }
        }
    }
}