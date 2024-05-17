using UnityEngine;

namespace Plataformer
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 100;
        [SerializeField] FloatEventChannel playerHealthChannel;

        int health;
        
        public bool IsDead => health <= 0;

        private void Update()
        {
            ifdead();
        }

        void ifdead()
        {
            if (IsDead)
            {
                print("morreu");
            }
        }

        private void Awake()
        {
            health = maxHealth;
        }

        private void Start()
        {
            PublishHealthPercentage();
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            PublishHealthPercentage();
        }


        void PublishHealthPercentage() 
        {
            if (playerHealthChannel != null)
            {
                playerHealthChannel.Invoke(health / (float)maxHealth);
            }
        }
        

    }
}
