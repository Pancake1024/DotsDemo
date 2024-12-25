using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class AnimSkillFrameSystem : SystemBase
    {

        protected override void OnCreate()
        {
            RequireForUpdate<AnimSkillFrame>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            var job = new SkillFrameAnimJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecb = ecb.AsParallelWriter(),
            };

            var jobHandle = job.ScheduleParallel(Dependency);
            
            jobHandle.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
 
    [BurstCompile]
    public partial struct SkillFrameAnimJob : IJobEntity
    {
        public float deltaTime;
        public EntityCommandBuffer.ParallelWriter ecb; // EntityCommandBuffer to handle component modification safely in a job
        
        public void Execute(ref DynamicBuffer<AnimSkillFrame> frames,
            in SkillContextComponent context)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                ref AnimSkillFrame frame = ref frames.ElementAt(i);
                if (!frame.BaseFrame.IsFinish)
                {
                    if (frame.BaseFrame.TriggerTime >= 0)
                    {
                        frame.BaseFrame.TriggerTime -= deltaTime;
                        if (frame.BaseFrame.TriggerTime <= 0)
                        {
                            var buffer = ecb.AddBuffer<AnimStateChangeEvent>(context.Owner.Index, context.Owner);
                            buffer.Add(new AnimStateChangeEvent()
                            {
                                AnimState = UnitAnimState.Attack,
                            });
                        }   
                    }
                    
                    frame.Duration -= deltaTime;
                    frame.BaseFrame.IsFinish = frame.Duration <= 0;
                }
            }
        }
    }
}