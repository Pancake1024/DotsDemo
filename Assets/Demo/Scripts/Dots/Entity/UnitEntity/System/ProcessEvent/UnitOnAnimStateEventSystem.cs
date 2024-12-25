using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitOnAnimStateEventSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitAnimStateComponent>();
            RequireForUpdate<AnimStateChangeEvent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var job = new OnAnimStateEventJob();
            
            var jobHandle = job.ScheduleParallel(Dependency);
            jobHandle.Complete();
        }
    }

    [BurstCompile]
    public partial struct OnAnimStateEventJob : IJobEntity
    {
        public void Execute(
            DynamicBuffer<AnimStateChangeEvent> events,
            ref UnitAnimStateComponent animStateComp,
            in Entity entity
            )
        {
            if (events.Length > 0)
            {
                for (int i = 0; i < events.Length; i++)
                {
                    var e = events[i];
                    if (_AnimStateCanChange())
                    {
                        animStateComp.AnimState = e.AnimState;
                        Debug.LogError($"OnAnimStateEventJob Entity Index：{entity.Index} OnStateEvent: {e.AnimState}");
                    }
                }

                events.Clear();
            }
        }

        private bool _AnimStateCanChange()
        {
            return true;
        }
    }
}