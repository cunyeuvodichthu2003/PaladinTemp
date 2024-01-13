using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private bool canAttack;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    public void CreateClone(Transform _clonePosition, Vector3 offset)
    {   
        GameObject newClone = Instantiate(clonePrefab);
        
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition,cloneDuration,canAttack,offset);
    }
}
