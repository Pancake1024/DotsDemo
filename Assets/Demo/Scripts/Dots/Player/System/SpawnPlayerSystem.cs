using Unity.Entities;
using Unity.Transforms;

namespace Pancake.ECSDemo
{
    public partial class SpawnPlayerSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<SpawnPlayerConfig>();
        }

        protected override void OnUpdate()
        {
            this.Enabled = false;
            
            SpawnPlayerConfig SpawnPlayerConfig = SystemAPI.GetSingleton<SpawnPlayerConfig>();
            
            EntityCommandBuffer ecb = new EntityCommandBuffer(WorldUpdateAllocator);
            
            Entity spawnedEntity = ecb.Instantiate(SpawnPlayerConfig.PlayerPrefab);
            //初始化位置
            ecb.SetComponent(spawnedEntity, new LocalTransform()
            {
                Position = SpawnPlayerConfig.Position,
                Rotation = SpawnPlayerConfig.Rotation,
                Scale = SpawnPlayerConfig.Scale,
            });
            
            //初始化技能
            ecb.AddBuffer<PlayerSkillIdElement>(spawnedEntity);
            var buffer = ecb.SetBuffer<PlayerSkillIdElement>(spawnedEntity);
            
            buffer.Add(new PlayerSkillIdElement() { Slot = 0,ID = 101 });
            buffer.Add(new PlayerSkillIdElement() { Slot = 1,ID = 102 });
            buffer.Add(new PlayerSkillIdElement() { Slot = 2,ID = 103 });
            buffer.Add(new PlayerSkillIdElement() { Slot = 3,ID = 104 });
            buffer.Add(new PlayerSkillIdElement() { Slot = 4,ID = 105 });
            
            ecb.Playback(EntityManager);
        }
    }
}