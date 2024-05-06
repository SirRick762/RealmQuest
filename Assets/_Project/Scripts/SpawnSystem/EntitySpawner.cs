using System.Collections;
using System.Collections.Generic;

namespace Plataformer
{
    public partial class EntitySpawner<T> where T : Entity
    {
        IEntityFactory<T> entityFactory;


        IspawnPointStrategy spawnPointStrategy;

        public EntitySpawner(IEntityFactory<T> entityFactory, IspawnPointStrategy spawnPointStrategy)
        {
            this.entityFactory = entityFactory;
            this.spawnPointStrategy = spawnPointStrategy;
        }

        public T Spawn()
        {
            return entityFactory.Create(spawnPointStrategy.NextSpawnPoint());
        }
    }
}
