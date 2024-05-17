using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Plataformer
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {
        [SerializeField, Self] NavMeshAgent agent;
        [SerializeField, Self] PlayerDetector playerDetector;
        [SerializeField, Child] Animator animator;

        [SerializeField] float wanderRadius = 30f;
        [SerializeField] float timeBetweenAttacks = 1f;

        StateMachine stateMachine;
        EnemyChaseState chaseState;
        EnemyWanderState wanderState;
        EnemyAttackState attackState;

        CountdownTimer attackTimer;

        void OnValidate() => this.ValidateRefs();

        void Start()
        {
            stateMachine = new StateMachine();

            attackTimer = new CountdownTimer(timeBetweenAttacks);

            wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);

            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

            stateMachine.SetState(wanderState);

            // Subscribe to player detection events
            playerDetector.OnPlayerDetected.AddListener(UpdatePlayerInStates);
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void Update()
        {
            stateMachine.Update();
            attackTimer.Tick(Time.deltaTime);
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void Attack()
        {
            if (attackTimer.IsRunning) return;
            attackTimer.Start();
            Debug.Log("Attacking");
            playerDetector.PlayerHealth.TakeDamage(1);
            // Implement your attack logic here
        }

        void UpdatePlayerInStates(Transform playerTransform)
        {
            chaseState.UpdatePlayer(playerTransform);
            attackState.UpdatePlayer(playerTransform);
        }

        private void OnDestroy()
        {
            // Unsubscribe from player detection events to avoid memory leaks
            playerDetector.OnPlayerDetected.RemoveListener(UpdatePlayerInStates);
        }
    }
}
