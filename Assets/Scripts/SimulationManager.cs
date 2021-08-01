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
    public List<List<float>> jarakAntarKota = new List<List<float>>();
	public List<List<float>> inversJarakAntarKota = new List<List<float>>();
	public List<List<float>> pheromoneGlobal = new List<List<float>>();
	
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
		
		// Menghitung jarak Antar kota -> Vector3.Distance(a, b);
		for (int i = 0; i < daftarKota.Length; i++)
		{
			List<float> jarak = new List<float>();
			
			for (int j = 0; j < daftarKota.Length; j++)
			{
				float _jarak = Vector3.Distance(daftarKota[i].transform.position, daftarKota[j].transform.position);
				
				jarak.Add(_jarak);
			}
			
			jarakAntarKota.Add(jarak);
		}
		
		// Hitung invers 		
		for (int i = 0; i < daftarKota.Length; i++)
		{
			List<float> _invers = new List<float>();
			
			for (int j = 0; j < daftarKota.Length; j++)
			{
				_invers.Add( 1 / jarakAntarKota[i][j]);				
			}
			
			inversJarakAntarKota.Add(_invers);
		}			
		
		// Hitung Pheromone
		for (int i = 0; i < daftarKota.Length; i++)
		{
			List<float> pheromone = new List<float>();
			
			for (int j = 0; j < daftarKota.Length; j++)
			{
				pheromone.Add(0.000001f);
				//pheromoneGlobal[i][j] = .000001f;
			}
			
			pheromoneGlobal.Add(pheromone);
		}			
		
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
