using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackPatternData", menuName = "Data/AttackPatternData")]
public class AttackPatternData : ScriptableObject
{
    public AnimationClip[] clip;
}
