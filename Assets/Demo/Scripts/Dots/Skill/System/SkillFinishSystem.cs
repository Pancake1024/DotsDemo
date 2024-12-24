using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class SkillFinishSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<SkillContextComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            var job = new SkillFinishJob()
            {
                ecb = ecb.AsParallelWriter(),
            };

            var jobHandle = job.ScheduleParallel(Dependency);
            jobHandle.Complete();
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }

    [BurstCompile]
    public partial struct SkillFinishJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        
         public void Execute(ref SkillContextComponent skillContext,
             DynamicBuffer<AnimSkillFrame> animSkillFrames,
             DynamicBuffer<DamageSkillFrame> damageSkillFrames,
             DynamicBuffer<CastActEndSkillFrame> castActEndSkillFrames,
            [EntityIndexInQuery] int entityIndex, 
             in Entity entity)
          {
              if (animSkillFrames.Length > 0)
              {
                  for (int i = 0; i < animSkillFrames.Length; i++)
                  {
                      var frame = animSkillFrames[i];
                        if (!frame.BaseFrame.IsFinish)
                        {
                            return;
                        }
                  }
              }

              if (damageSkillFrames.Length > 0)
              {
                  for (int i = 0; i < damageSkillFrames.Length; i++)
                  {
                      var frame = damageSkillFrames[i];
                        if (!frame.BaseFrame.IsFinish)
                        {
                            return;
                        }
                  }
              }
              
                if (castActEndSkillFrames.Length > 0)
                {
                    for (int i = 0; i < castActEndSkillFrames.Length; i++)
                    {
                        var frame = castActEndSkillFrames[i];
                            if (!frame.BaseFrame.IsFinish)
                            {
                                return;
                            }
                    }
                }
              
              ecb.AddComponent(entityIndex,entity, new SkillContextComponent()
              {
                  Owner = skillContext.Owner,
                  SkillID = skillContext.SkillID,
                  FrameCount = skillContext.FrameCount,
                  IsFinish = true,
              });
              
              Debug.LogError($"Desroy Skill Entity: {entity.Index}");
              ecb.DestroyEntity(entityIndex, entity);
          }
    }
}