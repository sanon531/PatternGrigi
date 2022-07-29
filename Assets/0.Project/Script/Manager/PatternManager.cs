using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MoreMountains.NiceVibrations;
using PG.Event;

namespace PG.Battle
{
    public class PatternManager : MonoBehaviour
    {
        [SerializeField]
        List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //1-4�� ���� ���� ������ ������ �ɰ�.
        [SerializeField]
        int _lastNode = 1;
        [SerializeField]
        List<int> _inactivatedNode;
        public static PatternManager _instance;
        [SerializeField]
        float _damage = 10;

        // Start is called before the first frame update
        void Awake()
        {
            if (_instance == null)
                _instance = this;
            _inactivatedNode = _defaultNode.ToList();
            Global_BattleEventSystem._on��Ʋ���� += StartTriggerNode;
        }
        private void Start()
        {
            //��� ���������� �����Ѵ�.
        }
        // Update is called once per frame
        void Update()
        {
            CheckVibration();
        }





        void StartTriggerNode() 
        {
            SetTriggerNodeByID(4);

        }
        //���߿� �߰��� ���� �̺�Ʈ�� ���� ���� ���ؼ�.
        public void SetTriggerNodeByID(int id)
        {
            SetNodeToNextReach(id);
        }

        //�������� ���� �Ǿ������� ����.
        public static void DamageCall(int nodeID)
        {
            float _resultDamage = _instance.GetNodePositionByID(_instance._lastNode, nodeID) * _instance._damage;
            _instance._lastNode = nodeID;
            _instance.ReachTriggeredNode_Random(nodeID);

            //�׾����� ��� ��� ���� �ʱ�ȭ �Ѵ�.
            if (!Enemy_Script.Damage(_resultDamage))
                _instance.ResetAllNode();
            LineTracer.instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            Global_BattleEventSystem.CallOn����ġȹ��(10f);
        }

        //���߿� �̺�Ʈ�� �־ 
        //���� �������� ��ġ�� ���� �Ǿ��ִٸ� �װŴ� �ȵȴٰ� ǥ����.
        public void ReachTriggeredNode_Random(int reachedNode)
        {
            //Debug.Log("reached : " + _reachedNode);
            ResetAllNode();
            //������ ������ ��ġ�� ���Ұ��� �������Ѵ�.
            _inactivatedNode.Remove(reachedNode);
            //���� �������� �������� �������Ҷ��� ���ؼ� �������� �Ѵ�.
            RandomNodeSet();
        }

        //�̷��� ����°Ŵ� ���� ���� ��ǥ���� 2���̻��϶� �������� ��ġ�Ҷ� ����Ұ�.
        void RandomNodeSet()
        {
            int i = _inactivatedNode.Count;
            int _deleteTarget = Random.Range(0, i);
            //Debug.Log(i + "set" + _deleteTarget);
            SetNodeToNextReach(_inactivatedNode[_deleteTarget]);
        }

        void ResetAllNode()
        {
            _inactivatedNode = _defaultNode.ToList();
            foreach (PatternNodeScript _nodes in _patternNodes)
                _nodes.SetIsReachable(false);

        }

        void SetNodeToNextReach(int i)
        {
            //Debug.Log("input " + i );
            if (_inactivatedNode.Contains(i))
            {
                CallVib();
                _patternNodes[i].SetIsReachable(true);
                _inactivatedNode.Remove(i);
            }
            else
                Debug.LogError("Wrong node Error: Already Exist");
        }

     


        float _leftTime = 0f;
        [SerializeField]
        float _limitTime = 0.25f;
        bool _vibAlive = false;
        [SerializeField]
        float _currentIntensity, _currentSharpness = 1;

        void CallVib()
        {
            MMVibrationManager.StopContinuousHaptic(true);
            _vibAlive = true;
            _leftTime = _limitTime;
            MMVibrationManager.ContinuousHaptic(_currentIntensity, _currentIntensity, _leftTime, HapticTypes.LightImpact, this, true, -1, true);

        }
        void CallVib(float time)
        {
            _vibAlive = true;
            _leftTime = time;
        }
        void CheckVibration()
        {
            if (!_vibAlive)
                return;

            if (_leftTime > 0f)
            {
                _leftTime -= Time.deltaTime;
                //ShowDebugtextScript.SetDebug("time left" + _leftTime);
            }
            else
            {
                _vibAlive = false;
            }


        }

        public void SetIntensity(Slider s)
        {
            _currentIntensity = s.value;
        }
        public void SetSharpness(Slider s)
        {
            _currentSharpness = s.value;
        }
        public void SetTimeSlider(Slider s)
        {
            _limitTime = s.value;
        }


        Dictionary<int, Vector2Int> _IDDic = new Dictionary<int, Vector2Int>()
    {
        {0,new Vector2Int(0,0) },
        {1,new Vector2Int(1,0) },
        {2,new Vector2Int(2,0) },
        {3,new Vector2Int(0,1) },
        {4,new Vector2Int(1,1) },
        {5,new Vector2Int(2,1) },
        {6,new Vector2Int(0,2) },
        {7,new Vector2Int(1,2) },
        {8,new Vector2Int(2,2) }
    };
        float GetNodePositionByID(int startID, int endID)
        {
            float _xval = Mathf.Pow(_IDDic[startID].x - _IDDic[endID].x, 2);
            float _yval = Mathf.Pow(_IDDic[startID].y - _IDDic[endID].y, 2);
            //Debug.Log(startID + " and " + endID);
            //Debug.Log(_IDDic[startID] + " + "+ _IDDic[endID] + ":"+ _xval +"+" + _yval);
            return Mathf.Sqrt(_xval + _yval);
        }

    }
}