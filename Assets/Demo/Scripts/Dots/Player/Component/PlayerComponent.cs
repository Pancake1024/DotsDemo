using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{

    public class PlayerGameObjectComponent : IComponentData
    {
        public GameObject AnimatedGO;
        public Animator Animator;
    }

    public class PlayerConfigManaged : IComponentData
    {
        public GameObject AnimatedPrefabGO;
    }
    
    public struct PlayerSkillIdElement : IBufferElementData
    {
        public int Slot;
        public int ID;
    }

    #region Skill Event
    
    public struct DamageEvent : IBufferElementData
    {
        public int Damage;
    }
    
    public struct AnimStateChangeEvent : IBufferElementData
    {
        public PlayerAnimState AnimState;
    }
    
    public struct CasterActFinishEvent : IBufferElementData
    {
    }
    
    #endregion
}