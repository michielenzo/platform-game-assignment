using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Obj/EnemyConfig", order = 0)]
public class EnemyConfig : ScriptableObject
{    
    public float targetLockDistance;
    public float chaseDistance;
    public int MaxHealth;
    public float movementForceFactor;
}
