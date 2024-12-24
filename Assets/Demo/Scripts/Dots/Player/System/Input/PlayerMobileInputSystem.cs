using PFF.Common;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class PlayerMobileInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<PlayerInputComponent>();
            RequireForUpdate<CameraComponent>();
        }

        protected override void OnUpdate()
        {
            if (Utils.GetPlatformType() == PlatfromType.Mobile)  
            {
                Entities.ForEach((ref PlayerInputComponent input) =>
                {
                    
                }).Run();
            }
        }
    }
}