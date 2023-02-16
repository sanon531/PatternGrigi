using System.Collections;
using System.Collections.Generic;
using PG.Battle;
using UnityEngine;

public class InspirationGaugeSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PatternManager는 Awake라서 가능
        GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
        transform.position = PatternManager._instance._patternNodes[4].transform.position;
    }

    public void OnAnimationEnd()
    {
        BattleExtraAttackManager.OnCircleAnimationEnd();
    }
}
