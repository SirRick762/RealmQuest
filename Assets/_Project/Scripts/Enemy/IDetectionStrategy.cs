using UnityEngine;
using Utilities;

namespace Plataformer
{
    public interface IDetectionStrategy
    {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);
    }
}
