using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class DamageSkillFrameSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<DamageSkillFrame>();
            RequireForUpdate<SkillContextComponent>();
        }

        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            var job = new DamageSkillFrameJob()
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
    public partial struct DamageSkillFrameJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float deltaTime;
        
        public void Execute(ref DynamicBuffer<DamageSkillFrame> frames, 
            [EntityIndexInQuery] int entityIndex,
            in SkillContextComponent context)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                ref DamageSkillFrame frame = ref frames.ElementAt(i);
                if (!frame.BaseFrame.IsFinish)
                {
                    frame.BaseFrame.TriggerTime -= deltaTime;
                    if (frame.BaseFrame.TriggerTime <= 0)
                    {
                        frame.BaseFrame.IsFinish = true;
                        var buffer = ecb.AddBuffer<DamageEvent>(entityIndex, context.Target);
                        buffer.Add(new DamageEvent()
                        {
                            Damage = frame.Damage,
                        });
                    }
                }
                
            }
        }
    }
}