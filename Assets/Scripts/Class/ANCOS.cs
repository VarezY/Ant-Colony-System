using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ANCOS
    {
        #region Atribut
        
        // List kota yang telah di tempati
        private List<int> listKotaDitempati = new List<int>();
        
        // List dari kota yang belum di tempati
        private  List<int> listKotaBelumDitempati = new List<int>();

        // Pheromone Lokal
        private List<List<float>> pheromoneLokal = new List<List<float>>();
        
        // Total Jarak yang di Tempuh
        private float totalJarakAgent = 0f;

        // Nama Semut
        private string namaSemut;
        
        #endregion

        #region Properties

        public List<int> ListKotaDitempati => listKotaDitempati;

        public List<int> ListKotaBelumDitempati
        {
            get => listKotaBelumDitempati;
            set => listKotaBelumDitempati = value;
        }

        public List<List<float>> PheromoneLokal
        {
            get => pheromoneLokal;
            set => pheromoneLokal = value;
        }

        public float TotalJarakAgent
        {
            get => totalJarakAgent;
            set => totalJarakAgent = value;
        }

        public string NamaSemut
        {
            get => namaSemut;
            set => namaSemut = value;
        }

        #endregion

        #region Private Function

        private float DeltaPheromone(float _jarakKota, int _banyakKota)
        {
            return 1 / (_jarakKota * _banyakKota);
        }

        #endregion

        #region Public Function

        // Menandai Kota yang telah di tempati
        public void PindahKota(int _kotaTujuan)
        {
            listKotaBelumDitempati.Remove(_kotaTujuan);
            listKotaDitempati.Add(_kotaTujuan);
        }
        
        // Menghitung Temporary
        public double CalcTemporary(int _Xaxis, int _Yaxis, List<List<float>> _pheromone, List<List<float>> _invers, int _beta)
        {
            return _pheromone[_Xaxis][_Yaxis] * Mathf.Pow(_invers[_Xaxis][_Yaxis], _beta);
        }

        // Menghitung Probability
        public double CalcProbability(double _Temp, double _totalTemp)
        {
            return _Temp / _totalTemp;
        }
        
        // Mengupdate Pheromone Lokal dan Total Jarak Semut
        public void UpdatePheromoneLokal(int _Xaxis, int _Yaxis, float _jarakKota, int _jumlahKota)  
        {
            float newPheromone = ((1 - Konstanta.P) * pheromoneLokal[_Xaxis][_Yaxis]) + (Konstanta.P * DeltaPheromone(_jarakKota, _jumlahKota));
            pheromoneLokal[_Xaxis][_Yaxis] = newPheromone;
            pheromoneLokal[_Yaxis][_Xaxis] = newPheromone;
            TotalJarakAgent += _jarakKota;
        }

        // Memasukan Pheromone Semut Lokal Ke Data Pheromone Global 
        public void ExportData()
        {
            SimulationManager.myData += "Pheromone " + namaSemut + "\n";
            for (int i = 0; i < pheromoneLokal.Count; i++)
            {
                for (int j = 0; j < pheromoneLokal.Count; j++)
                {
                    SimulationManager.myData += pheromoneLokal[i][j] + ",";
                }

                SimulationManager.myData += "\n";
            }
		
            SimulationManager.myData += "\n";
            SimulationManager.myData += "Urutan Kota yang dikunjungi: ";
            foreach (int i in listKotaDitempati)
            {
                SimulationManager.myData += i +"-";
            }
            SimulationManager.myData += "\n";
            SimulationManager.myData += "Total Jarak yang ditempuh: " + totalJarakAgent + "\n";
        }

        #endregion

        #region Constructor

        public ANCOS(string namaSemut)
        {
            this.namaSemut = namaSemut;
        }

        #endregion
    }
}