using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

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

        StateMachine stateMachine;
        EnemyChaseState chaseState;
        EnemyWanderState wanderState;

        void OnValidate() => this.ValidateRefs();

        void Start()
        {
            stateMachine = new StateMachine();

            wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);

            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));

            stateMachine.SetState(wanderState);

            // Subscribe to player detection events
            playerDetector.OnPlayerDetected.AddListener(UpdatePlayerInChaseState);
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void UpdatePlayerInChaseState(Transform playerTransform)
        {
            chaseState.UpdatePlayer(playerTransform);
        }

        private void OnDestroy()
        {
            // Unsubscribe from player detection events to avoid memory leaks
            playerDetector.OnPlayerDetected.RemoveListener(UpdatePlayerInChaseState);
        }
    }
}
