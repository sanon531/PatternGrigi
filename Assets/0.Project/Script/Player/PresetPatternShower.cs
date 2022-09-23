using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Data;
namespace PG.Battle
{
    public class PresetPatternShower : MonoSingleton<PresetPatternShower>
    {
        //����� �� �־ �ڵ�ȭ �Ͽ��� ���߿� ������ ������Ʈ�� 
        // �ϴ� d
        
        [SerializeField]
        GameObject _LazerPrefab;
        
        List<Transform> _nodes = new List<Transform>();
        List<GameObject> _listOfShow;

        [SerializeField]
        LaserIDObjectDic _laserIDDic = new LaserIDObjectDic();

        [SerializeField]
        LaserIDObjectListDic _objectPoolDic = new LaserIDObjectListDic();


        private void Start()
        {
            //��� 9�� ��ġ�� ����Ʈ�� ����
            foreach (PatternNodeScript node in PatternManager._instance._patternNodes)
            {
                _nodes.Add(node.transform);
            }
        }

        public static void SetPresetPatternList(List<int> presetNodes,LaserKindID laserKindID)
        {
            _instance._listOfShow = new List<GameObject>();

            GameObject temp;

            //������ ����ȣ�� �޾Ƽ� �ش��ϴ� ������ ����� ����Ʈ�� �߰�
            for (int i=0; i < presetNodes.Count-1; i++)
            {
                temp = Instantiate(_instance._laserIDDic[laserKindID], _instance.transform) ;

                if(temp.GetComponent<LazerParticle>() != null) 
                {
                    temp.GetComponent<LazerParticle>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LazerParticle>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }
                else
                {
                    temp.GetComponent<LazerLine>()._StartPos = _instance._nodes[presetNodes[i]].position;
                    temp.GetComponent<LazerLine>()._EndPos = _instance._nodes[presetNodes[i + 1]].position;
                }

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
            Destroy(_instance._listOfShow[id]);
        }

        public static void HidePresetPatternAll(int id)
        {
            foreach (GameObject obj in _instance._listOfShow)
                obj.SetActive(false);
        }

    }


}
