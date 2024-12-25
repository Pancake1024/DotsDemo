using PFF.Common;
using Unity.Entities;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class UnitMobileInputSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<CameraComponent>();
        }

        protected override void OnUpdate()
        {
            if (Utils.GetPlatformType() == PlatfromType.Mobile)  
            {
                Entities.ForEach((ref UnitInputComponent input, ref UnitAttributeComponent attributeComp) =>
                {
                    if (attributeComp.UnitType != UnitType.Player) return;
                    
                }).Run();
            }
        }
    }
}