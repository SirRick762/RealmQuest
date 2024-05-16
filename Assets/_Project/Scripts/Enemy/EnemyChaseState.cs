using UnityEngine;
using UnityEngine.AI;

namespace Plataformer
{
    public class EnemyChaseState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        Transform player;

        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Chase");
            animator.CrossFade(RunHash, crossFadeDuration);
        }

        public override void Update()
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
        }

        public void UpdatePlayer(Transform player)
        {
            this.player = player;
        }
    }
}
