using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Enemy_target : NetworkBehaviour
{
    private NavMeshAgent agent;
    private Transform myTransform;
    private Transform targetTransform;
    private LayerMask raycastLayer;
    private float radius = 1000.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1 << LayerMask.NameToLayer("Player");
    }

    public void FixedUpdate()
    {
        SearchForTarget();
        MoveToTarget();
    }

    void SearchForTarget()
    {
        if (!isServer)
            return;


        if (targetTransform == null)
        {
            //获取 raycastLayer 层中的 myTransform.position 位置的 radius 范围内的对象
            Collider[] hitColiders = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);

            if (hitColiders.Length > 0)
            {
                int randomint = Random.Range(0, hitColiders.Length);
                targetTransform = hitColiders[randomint].transform;
            }
        }

        if (targetTransform != null && targetTransform.GetComponent<CapsuleCollider>().enabled == false)
        {
            targetTransform = null;
            Debug.LogFormat("--- target name:{0}", targetTransform.name);
        }
    }

    void MoveToTarget()
    {
        if (!isServer)
            return;

        if (targetTransform != null)
        {
            SetNavDestination(targetTransform);
        }
    }

    void SetNavDestination(Transform dest)
    {
        agent.SetDestination(dest.position);
    }
}
