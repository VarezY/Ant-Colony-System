using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class InputData : MonoBehaviour
{
    public TMP_InputField _inputKota, _inputSemut, _inputBeta, _inputQ0;

    public void InputSimulationData()
    {
        Konstanta.beta = int.Parse(_inputBeta.text);
        Konstanta.q0 = float.Parse(_inputQ0.text);
        Konstanta.jumlahKota = int.Parse(_inputKota.text);
        Konstanta.jumlahSemut = int.Parse(_inputSemut.text);
    }
}
