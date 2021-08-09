using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Random = UnityEngine.Random;

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
    [SerializeField] private bool ExportDataAwal;
    [Space] [SerializeField] private GameObject _prefabsSemut;
    [SerializeField] private GameObject parentAgent;
	[Space]
    [SerializeField] private GameObject[] daftarKota;
	[SerializeField] private List<AgentBehaviour> Agents = new List<AgentBehaviour>();

	public static string myData;
	public static List<List<float>> pheromoneGlobal = new List<List<float>>();
	
    private int totalSemut = 1;
    private int startPoint;
    private int kotaSelanjutnya;
    private List<double> kotaTerdekat = new List<double>();
    
    private List<List<float>> jarakAntarKota = new List<List<float>>();
	private List<List<float>> inversJarakAntarKota = new List<List<float>>();
	private List<ModelKota> kotaList = new List<ModelKota>();

	private ANCOS agentSemut;
	
	// Start is called before the first frame update
    void Start()
    {
	    // Mendapatkan semua gameObject Kota
        daftarKota = GameObject.FindGameObjectsWithTag("Kota");

        // Mengambil semua lokasi kota yg ada di daftarKota
        foreach (GameObject Kota in daftarKota)
        {
            ModelKota mKota = new ModelKota()
            {
                namaKota = Kota.name,
                koordinatKota = Kota.transform.position
            };
            kotaList.Add(mKota);
        }

        // Instantiate Agent di random Kota
        startPoint = Random.Range(0, Konstanta.jumlahKota);
        GameObject newAgent = Instantiate(_prefabsSemut, daftarKota[startPoint].transform.position,
	        Quaternion.identity);
        newAgent.transform.parent = parentAgent.transform;
        
        foreach (GameObject semut in GameObject.FindGameObjectsWithTag("Agent"))
        {
            Agents.Add(semut.GetComponent<AgentBehaviour>());
        }

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
		
		// Hitung invers dari jarak kota		
		for (int i = 0; i < jarakAntarKota.Count; i++)
		{
			List<float> _invers = new List<float>();
			
			for (int j = 0; j < jarakAntarKota.Count; j++)
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

		#region Create Data for Export CSV

		myData += "Data Jarak Antar Kota\n";
		for (int i = 0; i < jarakAntarKota.Count; i++)
		{
			for (int j = 0; j < jarakAntarKota.Count; j++)
			{
				myData += jarakAntarKota[i][j] + ",";
			}
			myData += "\n";
		}

		myData += "\n\n";

		myData += "Data Invers Jarak Antar Kota\n";
		for (int i = 0; i < inversJarakAntarKota.Count; i++)
		{
			for (int j = 0; j < inversJarakAntarKota.Count; j++)
			{
				myData += inversJarakAntarKota[i][j] + ",";
			}

			myData += "\n";
		}
		
		myData += "\n\n";
		
		myData += "Pheromone Awal\n";
		for (int i = 0; i < inversJarakAntarKota.Count; i++)
		{
			for (int j = 0; j < inversJarakAntarKota.Count; j++)
			{
				myData += pheromoneGlobal[i][j] + ",";
			}

			myData += "\n";
		}
		
		myData += "\n\n";

		#endregion
		
		if (ExportDataAwal)
		{
			StartCoroutine(Konstanta.ExportCSV("report", myData));
		}

		agentSemut = new ANCOS("Semut " + totalSemut);
		newAgent.name = agentSemut.NamaSemut;
		agentSemut.PheromoneLokal = pheromoneGlobal;
		
		for (int i = 0; i < daftarKota.Length; i++)
		{
			if (startPoint != i)
			{
				agentSemut.ListKotaBelumDitempati.Add(i);
			}
			else
			{
				agentSemut.ListKotaDitempati.Add(i);
			}
		}
		
		UpdateNextKota();

    }

    // Update is called once per frame
    void Update()
    {
	    for (int i = 0; i < Agents.Count; i++)
	    {
		    if (KotaAgentDistance(Agents[i].transform, daftarKota[kotaSelanjutnya].transform))
		    {
			    UpdateNextKota();
		    }
	    }
    }

    private void UpdateNextKota()
    {
	    if (agentSemut.ListKotaBelumDitempati.Count != 0)
	    {
		    kotaTerdekat.Clear();
		    for (int kotaTujuan = 0; kotaTujuan < daftarKota.Length; kotaTujuan++)
		    {
			    if (agentSemut.ListKotaDitempati.Contains(kotaTujuan))
			    {
				    kotaTerdekat.Add(0f);
			    }
			    else
			    {
				    kotaTerdekat.Add(agentSemut.CalcTemporary(startPoint, kotaTujuan, pheromoneGlobal, inversJarakAntarKota, Konstanta.beta));
			    }
		    }
			// Mengambil Nilai Maksimal Pada Tabel Temporary 
		    kotaSelanjutnya = kotaTerdekat.IndexOf(kotaTerdekat.Max());

		    agentSemut.UpdatePheromoneLokal(startPoint, kotaSelanjutnya, jarakAntarKota[startPoint][kotaSelanjutnya], 
			    daftarKota.Length, Konstanta.q0);
		    agentSemut.PindahKota(kotaSelanjutnya);
        
		    startPoint = kotaSelanjutnya;

		    for (int i = 0; i < Agents.Count; i++)
		    {
			    Agents[i].target = daftarKota[kotaSelanjutnya].transform;
		    }
	    }
	    else
	    {
		    agentSemut.ExportData();
		    StartCoroutine(Konstanta.ExportCSV("report", myData));
		    totalSemut++;
		    NewAgent();
	    }
    }

    private bool KotaAgentDistance(Transform _agent, Transform _target)
    {
        return Vector3.Distance(_agent.position, _target.position) < 1.5f;
    }

    private void NewAgent()
    {
	    pheromoneGlobal = agentSemut.PheromoneLokal;
	    agentSemut.ListKotaDitempati.Clear();
	    agentSemut.ListKotaBelumDitempati.Clear();
	    agentSemut.NamaSemut = "Semut " + totalSemut;
	    
	    startPoint = Random.Range(0, Konstanta.jumlahKota);
	    
	    Destroy(GameObject.FindWithTag("Agent"));
	    Agents.Clear();;
	    GameObject newAgent = Instantiate(_prefabsSemut, daftarKota[startPoint].transform.position,
		    Quaternion.identity);
	    newAgent.transform.parent = parentAgent.transform;
	    newAgent.name = agentSemut.NamaSemut;
	    Agents.Add(newAgent.GetComponent<AgentBehaviour>());
	    
	    for (int i = 0; i < daftarKota.Length; i++)
	    {
		    if (startPoint != i)
		    {
			    agentSemut.ListKotaBelumDitempati.Add(i);
		    }
		    else
		    {
			    agentSemut.ListKotaDitempati.Add(i);
		    }
	    }
	    
	    UpdateNextKota();
    }
}
