using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MCTSSettings : ScriptableObject
{
    public float explorationFactor;
    public int nbSearch;
    public int nbSimulation;
    public float deltaTime;
}
