using System.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine.UI;
using Unity.Transforms;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject ballPrefab;
    public Text scoreText;

    public int maxScore;
    public int cubesPerFrame;
    public GameObject cubePrefab;   
    public float cubeSpeed = 3f;    

    private int curScore;
    private Entity ballEntityPrefab;
    private EntityManager manager;
    private BlobAssetStore blobAssetStore;

    private bool insaneMode;
    private Entity cubeEntityPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);

        cubeEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, settings);
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    private void Start()
    {
        curScore = 0;

        insaneMode = false;

        DisplayScore();
        SpawnBall();
    }

    private void Update()
    {
        if (!insaneMode && curScore >= maxScore)
        {
            insaneMode = true;
            StartCoroutine(SpawnLotsOfCubes());
        }
    }

    IEnumerator SpawnLotsOfCubes()
    {
        while (insaneMode)
        {
            for (int i = 0; i < cubesPerFrame; i++)
            {
                SpawnNewCube();
            }
            yield return null;
        }
    }

    void SpawnNewCube()
    {
        Entity newCubeEntity = manager.Instantiate(cubeEntityPrefab);

        Vector3 direction = Vector3.up;
        Vector3 speed = direction * cubeSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        manager.AddComponentData(newCubeEntity, velocity);
    }

    public void IncreaseScore()
    {
        curScore++;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = "Score: " + curScore;
    }

    void SpawnBall()
    {
        Entity newBallEntity = manager.Instantiate(ballEntityPrefab);

        Translation ballTrans = new Translation
        {
            Value = new float3(0f, 0.5f, 0f)
        };

        manager.AddComponentData(newBallEntity, ballTrans);
        CameraFollow.instance.ballEntity = newBallEntity;
    }
}
