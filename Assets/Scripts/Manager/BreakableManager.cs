using UnityEngine;
using UnityEngine.Events;

public class BreakableManager : MonoBehaviour
{
    [SerializeField]
    private int _durability;
    
    public UnityEvent onBreak;
    
    public int durability
    {
        get { return _durability; }
        set
        {
            _durability = value;

            if (_durability <= 0)
            {
                onBreak?.Invoke();
                                
                Destroy(gameObject);
            }
        }
    }
}
