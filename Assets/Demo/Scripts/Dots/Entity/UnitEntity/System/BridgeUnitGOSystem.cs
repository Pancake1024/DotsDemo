using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Pancake.ECSDemo
{
    public partial class BridgeUnitGOSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<SpawnUnitConfig>();
            RequireForUpdate<UnitInputComponent>();
            RequireForUpdate<UnitGameObjectComponent>();
        }

        protected override void OnUpdate()
        {
            Entities.WithoutBurst().ForEach((in UnitGameObjectComponent objComp,
                in LocalTransform localTransform,
                in Entity entity
                ) =>
            {
                objComp.GO.transform.localPosition = localTransform.Position;
                objComp.GO.transform.rotation = localTransform.Rotation;
            }).Run();
        }
    }
}