using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{
    [SerializeField] private float kecepatan;
    [SerializeField] private float rotateSpeed;
    public Transform target;
    private Transform myTransform;

    // Update is called once per frame
    void Update()
    {
        Rotasi(target);
        
        Bergerak();
    }

    private void Bergerak()
    {
        transform.position += transform.forward * (kecepatan * Time.deltaTime);
    }

    private void Rotasi(Transform antTarget)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(antTarget.position - transform.position), rotateSpeed * Time.deltaTime);
    }
}
