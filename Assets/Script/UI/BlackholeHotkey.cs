using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkey : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private Blackhole_Skill_Controller blackHole;
    private void Start()
    {
    }
    public void SetUpHotkey(KeyCode _myHotKey,Transform _myEnemy, Blackhole_Skill_Controller _myBlackhole)
    {
        spr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myEnemy = _myEnemy;
        blackHole = _myBlackhole;   
        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
    }
    private void Update()
    {
        if(Input.GetKeyDown(myHotKey)) 
        {
            blackHole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
            spr.color = Color.clear;
        }
    }
}
