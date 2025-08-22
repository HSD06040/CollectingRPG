using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UnitPlan
{
    public ClassType Class;
    public int[] PlanLines;
}

[CreateAssetMenu(fileName = "UnitPlans", menuName = "Data/UnitPlans")]
public class UnitPlanData : ScriptableObject
{
    public UnitPlan[] UnitPlans;
}
