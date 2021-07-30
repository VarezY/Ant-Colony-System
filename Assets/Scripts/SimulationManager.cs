using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public class ModelAgent
    {
        public float kecepatan;
        public float rotasi;
        public float targetDistance;
    }
    
    public class ModelKota
    {
        public string namaKota;
        public Vector3 koordinatKota;
    }
    
    // Variabels 
    private GameObject[] daftarKota;
    public List<ModelKota> kotaList = new List<ModelKota>();
    private int kotaTarget = 0;
    [SerializeField] private List<AgentBehaviour> Agents = new List<AgentBehaviour>();
    
    // Start is called before the first frame update
    void Start()
    {
        daftarKota = GameObject.FindGameObjectsWithTag("Kota");

        foreach (GameObject Kota in daftarKota)
        {
            ModelKota mKota = new ModelKota()
            {
                namaKota = Kota.name,
                koordinatKota = Kota.transform.position
            };
            kotaList.Add(mKota);
        }

        foreach (GameObject semut in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Agents.Add(semut.GetComponent<AgentBehaviour>());
        }
        
        UpdateNextKota();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Agents.Count; i++)
        {
            if (KotaAgentDistance(Agents[i].transform, daftarKota[kotaTarget].transform))
            {
                // UpdateNextKota();
                // UpdateNextKotaperAgent(Agents[i]);
            }
        }
    }

    private void UpdateNextKota()
    {
        kotaTarget++;
        kotaTarget %= kotaList.Count;

        for (int i = 0; i < Agents.Count; i++)
        {
            Agents[i].target = daftarKota[kotaTarget].transform;
        }
    }

    private void UpdateNextKotaperAgent(AgentBehaviour _agent)
    {
        kotaTarget++;
        kotaTarget %= kotaList.Count;

        _agent.target = daftarKota[kotaTarget].transform;
    }

    private bool KotaAgentDistance(Transform _agent, Transform _target)
    {
        return Vector3.Distance(_agent.position, _target.position) < 1;
    }
}
