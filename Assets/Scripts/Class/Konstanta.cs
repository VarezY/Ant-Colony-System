using System.Collections;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class Konstanta
    {
        public static int beta = 2;
        public static float q0 = 0.9f;
        public static int jumlahKota;

        public static float P
        {
            get { return 1 - q0; }
        }

        public static IEnumerator ExportCSV(string _namaFile, string _dataExternal, bool open = false)
        {
            string alamatFile = Application.dataPath.Replace("/Assets", "") + "/" + _namaFile + ".csv";

            if (File.Exists(alamatFile))
            {
                File.Delete(alamatFile);
            }

            TextWriter tw = new StreamWriter(alamatFile, false);

            /*// Input Data dari Simulation Manager
            #region Input Data Jarak Kota

            tw.WriteLine("JARAK ANTAR KOTA");

            for (int i = 0; i < jarakAntarKota.Count; i++)
            {
                for (int j = 0; j < jarakAntarKota.Count; j++)
                {
                    tw.Write(jarakAntarKota[i][j] + ",");
                }
                tw.WriteLine("");
            }

            #endregion

            tw.WriteLine("");

            #region Input Data Invers Jarak Kota

            tw.WriteLine("JARAK ANTAR KOTA");

            for (int i = 0; i < inversJarakAntarKota.Count; i++)
            {
                for (int j = 0; j < inversJarakAntarKota.Count; j++)
                {
                    tw.Write(inversJarakAntarKota[i][j] + ",");
                }
                tw.WriteLine("");
            }

            #endregion	*/  
	    
            tw.WriteLine(_dataExternal);
            
            tw.Close();

            yield return new WaitForSeconds(0.5f);

            if (open)
            {
                Application.OpenURL(alamatFile);
            }
        }
    }
    
    
}