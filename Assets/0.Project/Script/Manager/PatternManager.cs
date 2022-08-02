using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PG.Event;

namespace PG.Battle
{
    public class PatternManager : MonoBehaviour
    {
        [SerializeField]
        List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        List<int> _nodeHistory = new List<int>();

        //1-4�� ���� ���� ������ ������ �ɰ�.
        [SerializeField]
        int _lastNode = 1;
        [SerializeField]
        List<int> _inactivatedNode;
        public static PatternManager _instance;
        [SerializeField]
        float _damage = 10;
        [SerializeField]
        ParticleSystem _signParticle;

        // Start is called before the first frame update
        void Awake()
        {
        }
        private void Start()
        {
            if (_instance == null)
                _instance = this;
            _inactivatedNode = _defaultNode.ToList();
            Global_BattleEventSystem._on��Ʋ���� += StartTriggerNode;
        }
        // Update is called once per frame

        private void OnDestroy()
        {
            // Update is called once per frame
            Global_BattleEventSystem._on��Ʋ���� -= StartTriggerNode;
        }



        void StartTriggerNode() 
        {
            if (_lastNode != 4) 
            {

                _nodeHistory = new List<int>(1) { 4 };
                SetTriggerNodeByID(4);
            }

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


            //���� ȣ��
            VibrationManager.CallVibration();

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
                _signParticle.Play();
                Vector3 _targetpos = new Vector3((_IDDic[i].x - 1) * 1.75f, (-_IDDic[i].y) * 1.75f, 0);
                _signParticle.gameObject.transform.position = _targetpos;
                _patternNodes[i].SetIsReachable(true);
                _inactivatedNode.Remove(i);
            }
            else
                Debug.LogError("Wrong node Error: Already Exist");
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