using Unity.Collections;
using Unity.Entities;

namespace Pancake.ECSDemo
{
    public struct SkillContextComponent : IComponentData
    {
        public Entity Owner;
        public Entity Target;
        public int SkillID;
        public int FrameCount;
    }
    
    public struct BaseSkillFrame
    {
        public float TriggerTime;
        public bool IsFinish;
    }
    
    /// <summary>
    /// 动作帧
    /// </summary>
    public struct AnimSkillFrame : IBufferElementData
    {
        public BaseSkillFrame BaseFrame;
        public float Duration;
    }

    /// <summary>
    /// 伤害帧
    /// </summary>
    public struct DamageSkillFrame : IBufferElementData
    {
        public BaseSkillFrame BaseFrame;
        public int Damage;
    }
    
    /// <summary>
    /// 技能释放者完成技能释放行为
    /// </summary>
    public struct CastActEndSkillFrame : IBufferElementData
    {
        public BaseSkillFrame BaseFrame;
    }
}