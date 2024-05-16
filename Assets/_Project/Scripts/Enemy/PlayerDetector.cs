using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

namespace Plataformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f;
        [SerializeField] float detectionRadius = 10f;
        [SerializeField] float innerDetectionRadius = 5f;
        [SerializeField] float detectionCooldown = 1f;
        public Transform Player { get; private set; }

        public UnityEvent<Transform> OnPlayerDetected;

        CountdownTimer detectionTimer;
        IDetectionStrategy detectionStrategy;

        void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
            StartCoroutine(FindPlayer());
        }

        void Update() => detectionTimer.Tick(Time.deltaTime);

        IEnumerator FindPlayer()
        {
            while (true)
            {
                if (Player == null)
                {
                    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                    if (playerObject != null)
                    {
                        Player = playerObject.transform;
                        OnPlayerDetected?.Invoke(Player); // Notify listeners that the player has been detected
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public bool CanDetectPlayer()
        {
            if (Player == null) return false;
            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
        }

        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;
    }
}
