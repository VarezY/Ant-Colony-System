using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ANCOS
    {
        #region Atribut
        
        // List kota yang telah di tempati
        public static  List<int> listKotaDitempati = new List<int>();
        
        // List dari kota yang belum di tempati
        public static List<int> listKotaBelumDitempati = new List<int>();

        private List<List<float>> pheromoneLokal = new List<List<float>>();
        
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

        #endregion

        #region Private Function

        

        #endregion

        #region Public Function

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
        
        #endregion

        #region Constructor



        #endregion
    }
}