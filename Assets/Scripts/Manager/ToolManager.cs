using Unity.VisualScripting;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collide!");
    }

   void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Trigger!");

        if (other.TryGetComponent<BreakableManager>(out var target))
        {
            target.durability -= damage;
        }
    }
}
