using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerSkillSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<PlayerSkillIdElement>();
            RequireForUpdate<PlayerStateComponent>();
        }

        [BurstCompile]
        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            var job = new PlayerSkillJob()
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
    public partial struct PlayerSkillJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        
        public void Execute(ref DynamicBuffer<PlayerSkillIdElement> skillIds,
            ref PlayerStateComponent stateComp,
            in PlayerInputComponent input,
            [EntityIndexInQuery] int entityIndex,
            in Entity entity)
        {
            var castSkillID = _TryGetCastSkillId(skillIds, input.UsedSkillSlot);
            if (castSkillID == -1)return;
            Debug.LogError($"Try Cast skill id: {castSkillID}");
            if (StateUtility.CanCastActiveSkill(stateComp))
            {
                Debug.LogError($"Cast skill id: {castSkillID} Success");
                StateUtility.TryAddState(ref stateComp, PlayerState.CastSkill);
                _CreateSkill(ecb,entityIndex, entity, castSkillID);
            }
            else
            {
                Debug.LogError($"Cast skill id: {castSkillID} Failed");
            }
        }
        
        private int _TryGetCastSkillId(DynamicBuffer<PlayerSkillIdElement> skillIdElements, int slot)
        {
            int id = -1;
            if (slot != -1)
            {
                for (int i = 0; i < skillIdElements.Length; i++)
                {
                    var skillIdElement = skillIdElements[i];
                    if (skillIdElement.Slot == slot)
                    {
                        id = skillIdElement.ID;
                        break;
                    }
                }
            }

            return id;
        }

        private void _CreateSkill(EntityCommandBuffer.ParallelWriter ecb,int entityIndex,Entity owner,int skillId)
        {
            var skillEntity = ecb.CreateEntity(entityIndex);
            Debug.LogError($"Skill ID: {skillId} Entity Created");
            ecb.AddComponent(entityIndex, skillEntity, new SkillContextComponent
            {
                Owner = owner,
                SkillID = skillId,
                FrameCount =  3,
                IsFinish = false,
            });
            
            //TODO: Create skill frame form config
            
            //伤害帧
            ecb.AddBuffer<DamageSkillFrame>(entityIndex,skillEntity);
            var damage = ecb.SetBuffer<DamageSkillFrame>(entityIndex,skillEntity);
            damage.Add(new DamageSkillFrame() { BaseFrame = new BaseSkillFrame() { TriggerTime = 0.34f }, Damage = 10 });
        
            //动作帧
            ecb.AddBuffer<AnimSkillFrame>(entityIndex,skillEntity);
            var anim = ecb.AddBuffer<AnimSkillFrame>(entityIndex,skillEntity);
            anim.Add(new AnimSkillFrame() { BaseFrame = new BaseSkillFrame() { TriggerTime = 0.0f }, Duration = 1.333f});

            //技能释放者完成技能释放行为帧
            ecb.AddBuffer<CastActEndSkillFrame>(entityIndex,skillEntity);
            var castEnd = ecb.SetBuffer<CastActEndSkillFrame>(entityIndex,skillEntity);
            castEnd.Add(new CastActEndSkillFrame() { BaseFrame = new BaseSkillFrame() { TriggerTime = 1.333f } });
        }
    }
}