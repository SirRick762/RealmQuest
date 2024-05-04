using UnityEngine;
using DG.Tweening;

namespace Platformer
{
    public class PlataformMover : MonoBehaviour {

        [SerializeField] Vector3 moveto = Vector3.zero;
        [SerializeField] float moveTime = 1f;
        [SerializeField] Ease ease = Ease.InOutQuad;

        Vector3 startPosition;
        
        void Start ()
        {
            startPosition = transform.position;
            Move();
        }
        
        void Move ()
        {
            transform.DOMove(startPosition + moveto, moveTime).SetEase(ease).SetLoops(-1,LoopType.Yoyo);

        }
    }
}