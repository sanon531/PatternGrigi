using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //����� �� �־ �ڵ�ȭ �Ͽ��� ���߿� ������ ������Ʈ�� 
        // �ϴ� 
        
        [SerializeField]
        GameObject _lightningBoltPrefab;
        
        List<Transform> _nodes = new List<Transform>();
        List<GameObject> _listOfShow;


        protected override void CallOnAwake()
        {
        }
        private void Start()
        {
            //��� 9�� ��ġ�� ����Ʈ�� ����
            foreach (PatternNodeScript node in PatternManager._instance._patternNodes)
            {
                _nodes.Add(node.transform);
            }
        }

        public static void SetPresetPatternList(List<int> presetNodes)
        {
            _instance._listOfShow = new List<GameObject>();

            GameObject temp, start, end;

            //������ ����ȣ�� �޾Ƽ� �ش��ϴ� ������ ����� ����Ʈ�� �߰�
            for (int i=0; i < presetNodes.Count-1; i++)
            {
                temp = Instantiate(_instance._lightningBoltPrefab, _instance.transform);
                start = temp.GetComponent<LightningBoltScript>().StartObject;
                end = temp.GetComponent<LightningBoltScript>().EndObject;
                start.transform.position = _instance._nodes[presetNodes[i]].position;
                end.transform.position = _instance._nodes[presetNodes[i+1]].position;
                _instance._listOfShow.Add(temp);
            }
        }

        public static void ShowPresetPatternAll() 
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(true);
        }
        public static void HidePresetPatternByID(int id)
        {
            _instance._listOfShow[id].SetActive(false);
        }

        public static void HidePresetPatternAll(int id)
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(false);
        }

    }


}
