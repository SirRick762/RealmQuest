using UnityEngine;
using UnityEngine.AI;

namespace Plataformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        Transform player;

        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Attack");
            animator.CrossFade(AttackHash, crossFadeDuration);
        }

        public override void Update()
        {
            if (player != null)
            {
                agent.SetDestination(player.position);
                enemy.Attack();
            }
        }

        public void UpdatePlayer(Transform player)
        {
            this.player = player;
        }
    }
}
