using GameMechanic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 20; 
    private bool _isActive = false; 

    public void Activate(int attack)
    {
        damage = attack;
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive) return; 

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, transform.position); 
            }
        }
    }
}