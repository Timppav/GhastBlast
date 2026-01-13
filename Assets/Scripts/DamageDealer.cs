using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float _dps;

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"Trigger with: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out EntityHealth entityHealth))
        {
            Debug.Log("CONTACT");
            entityHealth.LoseHealth(Time.fixedDeltaTime * _dps);
        }
    }
}
