using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateFloat : MonoBehaviour
{
    [SerializeField] private TMP_InputField q0;
    
    public void GenerateNumber()
    {
        q0.text = Random.value.ToString();
    }
}
