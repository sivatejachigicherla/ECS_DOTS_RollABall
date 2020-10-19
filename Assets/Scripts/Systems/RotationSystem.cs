using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

[AlwaysSynchronizeSystem]
public class RotationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Rotation rotation, in RotationSpeedData rotationSpeed) => 
        {
            rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(rotationSpeed.speed * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(rotationSpeed.speed * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(rotationSpeed.speed * deltaTime)));
        }).Run();

        return default;
    }
}
