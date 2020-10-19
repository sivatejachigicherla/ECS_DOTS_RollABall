using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public Entity ballEntity;
    public float3 offset;

    private EntityManager manager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void LateUpdate()
    {
        if (ballEntity == null) { return; }

        Translation ballPos = manager.GetComponentData<Translation>(ballEntity);
        transform.position = ballPos.Value + offset;
    }
}
