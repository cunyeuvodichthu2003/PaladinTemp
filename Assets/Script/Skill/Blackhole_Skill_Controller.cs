using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    public float growSpeed;
    public float maxSize;
    public bool canGrow;
    public int amountOfAttack = 4;
    public float cloneAttackCoolDown=.3f;
    private float cloneAttackTimer = 0;
    private bool canAttack;
    private List<Transform> target = new List<Transform>();
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keys;
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime; 
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            canAttack = true; 
        }
        
        if ( cloneAttackTimer  <0 && canAttack)
        {
            if (amountOfAttack <= 0)
            {
                canAttack = false;
            }
            cloneAttackTimer = cloneAttackCoolDown;
            float xOffset;
            if (Random.Range(0, 100) < 50)
            { xOffset = -1; }
            else
                xOffset = 1;
            SkillManager.instance.clone.CreateClone(target[0].transform,new Vector3(xOffset,0,0));
            amountOfAttack--;
            
        }
        
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>()!=null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        if(keys.Count <= 0)
        {
            Debug.Log("Not Have Enough Key");
            return;
        }
        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        KeyCode chosenKey = keys[Random.Range(0, keys.Count)];
        keys.Remove(chosenKey);
        BlackholeHotkey newBlackHoleHotkey = newHotkey.GetComponent<BlackholeHotkey>();
        newBlackHoleHotkey.SetUpHotkey(chosenKey, collision.transform, this);

    }

    public void AddEnemyToList(Transform _enemyTransform)
    {
        target.Add(_enemyTransform);
    }
}
