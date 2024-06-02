using System.Linq;
using UnityEngine;
using Utilities;


namespace Plataformer
{
    public class CollectibleSpawnManager1 : EntitySpawnManager
    {
        [SerializeField] CollectibleData1[] collectibleData;
        [SerializeField] float spawnInterval = 1.0f;

        EntitySpawner<Collectible> spawner;

        CountdownTimer spawnTimer;
        int counter;

        protected override void Awake()
        {
            base.Awake();

            spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(collectibleData), spawnPointStrategy);

            spawnTimer = new CountdownTimer(spawnInterval);
            spawnTimer.OnTimerStop += () =>
            {
                if(counter++ >= spawnPoints.Length)
                {
                    spawnTimer.Stop();
                    return;
                }
                Spawn();
                spawnTimer.Start();
            };
        }

        void Start() => spawnTimer.Start();

        void Update()
        {
            spawnTimer.Tick(Time.deltaTime);
            
        }
        public override void Spawn() => spawner.Spawn();
    }
}
