using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class CastActEndSkillFrameSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<CastActEndSkillFrame>();   
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            var job = new CastActEndSkillFrameJob()
            {
                ecb = ecb.AsParallelWriter(),
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            
            var jobHandle = job.ScheduleParallel(Dependency);
            jobHandle.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }   
    }
    
    [BurstCompile]
    public partial struct CastActEndSkillFrameJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float deltaTime;
        
        public void Execute(ref DynamicBuffer<CastActEndSkillFrame> frames, 
            in SkillContextComponent context, 
            in Entity entity)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                ref CastActEndSkillFrame frame = ref frames.ElementAt(i);
                if (!frame.BaseFrame.IsFinish)
                {
                    frame.BaseFrame.TriggerTime -= deltaTime;
                    if (frame.BaseFrame.TriggerTime <= 0)
                    {
                        frame.BaseFrame.IsFinish = true;
                        var buffer = ecb.AddBuffer<CasterActFinishEvent>(context.Owner.Index, context.Owner);
                        buffer.Add(new CasterActFinishEvent());
                    }
                }
            }
        }
    }
}