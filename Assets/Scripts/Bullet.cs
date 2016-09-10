using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Health heal = hit.GetComponent<Health>();
        if (heal != null)
        {
            heal.TakeDamage(10);
        }

        DestroyObject(gameObject);
    }
}
