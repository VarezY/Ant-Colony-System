using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    public GameObject parentKota, parentAgent;
    [Space]
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject _prefabsKota;
    [SerializeField] private GameObject _prefabsSemut;
    
    private void Awake()
    {
        float scale = 1f;
        float moveAreaX = ground.GetComponent<Renderer>().bounds.size.x / 2;
        float moveAreaZ = ground.GetComponent<Renderer>().bounds.size.z / 2;
        Vector3 center = ground.GetComponent<Renderer>().bounds.center;
        for (int i = 0; i < Konstanta.jumlahKota; i++)
        {   
            var targetCoordsX = center.x + Random.Range(-moveAreaX*scale, moveAreaX*scale);
            var targetCoordsZ = center.z + Random.Range(-moveAreaZ*scale, moveAreaZ*scale);
            GameObject _newKota = Instantiate(_prefabsKota,parentKota.transform);
            _newKota.transform.position = new Vector3(targetCoordsX,0.222f,
                targetCoordsZ);

            _newKota.transform.name = "Kota" + (i + 1);
        }
    }

    
}
