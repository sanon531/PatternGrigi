using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
DamageTextScript.Create (new Vector3 (3.2f, 1.6f), 1, 0.3f, 123, Color.red); //-하면됨
*/
public class DamageTextScript : MonoBehaviour
{
    public List<Sprite> num;
    public float time = 1; //지속시간(second)
    private float remainTime; // 남은 시간
    public float yDelta = 1; //움직일 y축(정확하진 않음)
    public Color color = Color.red;

    // Use this for initialization
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpriteRenderer>().color = color;
        }
        remainTime = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainTime < 0)
        {
            Destroy(gameObject);
            return;
        }
        this.transform.position += Vector3.up * yDelta * (remainTime / time) * 2 * Time.deltaTime;
        remainTime -= Time.deltaTime;
        //change alpha
        Color col = GetComponent<SpriteRenderer>().color;
        col.a= remainTime / time;
        GetComponent<SpriteRenderer>().color = col;
        foreach (Transform child in this.transform)
        {
            col = child.GetComponent<SpriteRenderer>().color;
            col.a = remainTime / time;
            child.GetComponent<SpriteRenderer>().color = col;
        }
    }
    static GameObject damageText;
    public static void Create(Vector3 coord, float time, float yDelta, int text, Color color, float numberWidth = 1)
    {
        if (damageText == null)
            damageText = Resources.Load("DamageNum/DamageText", typeof(GameObject)) as GameObject;
        GameObject dmgBackground = Instantiate(damageText);
        dmgBackground.transform.position = coord;

        DamageTextScript dts = dmgBackground.GetComponent<DamageTextScript>();
        dts.time = time;
        dts.yDelta = yDelta;
        dts.color = color;
        //텍스트 이미지로 변경
        List<GameObject> nums = new List<GameObject>();
        int targetNum = text;
        int unit = 0;
        Debug.Log(targetNum);
        while (targetNum > 0.1)
        {
            int segment = targetNum % 10;
            targetNum /= 10;
            GameObject numImage = new GameObject();
            numImage.transform.parent = dmgBackground.transform;
            numImage.AddComponent<SpriteRenderer>();
            numImage.GetComponent<SpriteRenderer>().sprite = dts.num[segment];
            nums.Insert(0, numImage);
            unit++;

        }

        for (int i = 0; i < nums.Count; i++)
        {
            nums[i].transform.localPosition = new Vector3(numberWidth * (i - (nums.Count - 1) / 2.0f), 0, -0.1f);
        }

        //return dmgBackground;
    }
}
