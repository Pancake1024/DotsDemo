using PFF.Common;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public class AnimationData : Singleton<AnimationData>
    {
        public int IDLE_HASH;
        public int RUN_HASH;
        public int JUMP_HASH;
        public int JUMP_END_HASH;
        public int ATTACK_HASH;
        public int BE_HIT_HASH;
        public int DIE_HASH;

        protected override void OnInit()
        {
            IDLE_HASH = Animator.StringToHash("Idle");
            RUN_HASH = Animator.StringToHash("run");
            JUMP_HASH = Animator.StringToHash("jump");
            JUMP_END_HASH = Animator.StringToHash("jump_end");
            ATTACK_HASH = Animator.StringToHash("attack");
            BE_HIT_HASH = Animator.StringToHash("behit");
            DIE_HASH = Animator.StringToHash("die");
        }
    }
}