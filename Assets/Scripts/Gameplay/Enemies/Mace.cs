using UnityEngine;

namespace GameMechanic
{
    public class Mace : MonoBehaviour
    {
        [SerializeField] private int damage = 15; 
        [SerializeField] private EnemyController _enemy;


        private void OnTriggerStay(Collider other)
        {
            if (_enemy == null || !_enemy.IsAttacking) return;

            if (other.CompareTag("Player"))
            {
                CharacterController player = other.GetComponent<CharacterController>();
                if (player != null && !_enemy.IsDead)
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}