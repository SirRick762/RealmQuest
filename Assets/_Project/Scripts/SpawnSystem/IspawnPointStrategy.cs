using UnityEngine;

namespace Plataformer
{
    public interface IspawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}
