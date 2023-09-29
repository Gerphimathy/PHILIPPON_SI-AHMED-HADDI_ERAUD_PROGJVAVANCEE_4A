using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MCTSSettings : ScriptableObject
{
    public float explorationFactor = .8f;
    public int nbSearch = 200;
    public int nbSimulation = 30;
    public float deltaTime = 1/10f;
}
