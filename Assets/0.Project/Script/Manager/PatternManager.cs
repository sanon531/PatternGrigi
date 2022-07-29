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
        //1-4로 들어가는 것이 최초의 공격이 될것.
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
            Global_BattleEventSystem._on배틀시작 += StartTriggerNode;
        }
        private void Start()
        {
            //가운데 점에서부터 시작한다.
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
        //나중에 추가할 만한 이벤트와 연관 짓기 위해서.
        public void SetTriggerNodeByID(int id)
        {
            SetNodeToNextReach(id);
        }

        //데미지가 산출 되었을때의 정보.
        public static void DamageCall(int nodeID)
        {
            float _resultDamage = _instance.GetNodePositionByID(_instance._lastNode, nodeID) * _instance._damage;
            _instance._lastNode = nodeID;
            _instance.ReachTriggeredNode_Random(nodeID);

            //죽었으면 모든 노드 값을 초기화 한다.
            if (!Enemy_Script.Damage(_resultDamage))
                _instance.ResetAllNode();
            LineTracer.instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            Global_BattleEventSystem.CallOn경험치획득(10f);
        }

        //나중에 이벤트로 넣어서 
        //만약 여러개의 위치가 지정 되어있다면 그거는 안된다고 표시함.
        public void ReachTriggeredNode_Random(int reachedNode)
        {
            //Debug.Log("reached : " + _reachedNode);
            ResetAllNode();
            //기존의 도달한 위치는 사용불가로 만들어야한다.
            _inactivatedNode.Remove(reachedNode);
            //추후 여러개의 도달점을 가져야할때를 위해서 무작위로 한다.
            RandomNodeSet();
        }

        //이렇게 만드는거는 이제 다음 목표점이 2개이상일때 무작위로 배치할때 사용할것.
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