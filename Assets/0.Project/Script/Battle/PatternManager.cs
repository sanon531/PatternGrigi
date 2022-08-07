using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PG.Event;
using PG.Data;

namespace PG.Battle
{
    public class PatternManager : MonoBehaviour , ISetNontotalPause
    {
        [SerializeField]
        List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

        [SerializeField]
        List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        List<int> _nodeHistory = new List<int>();

        //1-4로 들어가는 것이 최초의 공격이 될것.
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
            Global_BattleEventSystem._onBattleBegin += StartTriggerNode;
            Global_BattleEventSystem._onNonTotalPause += SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause += SetNonTotalPauseOff;

        }
        // Update is called once per frame

        private void OnDestroy()
        {
            // Update is called once per frame
            Global_BattleEventSystem._onBattleBegin -= StartTriggerNode;
            Global_BattleEventSystem._onNonTotalPause -= SetNonTotalPauseOn;
            Global_BattleEventSystem._offNonTotalPause -= SetNonTotalPauseOff;

        }
        private void Update()
        {
            CheckIsCharge();
        }


        //데미지가 산출 되었을때의 정보.(이벤트로 바꿀것.)
        public static void DamageCall(int nodeID)
        {
            float _resultDamage = _instance.GetNodePositionByID(_instance._lastNode, nodeID) * _instance._damage;

            //먼저 게이지를 채우고 만약 게이지가 다찼을경우 주어진 노드가 나오도록함. 
            _instance.SetGaugeChange();
            _instance.CheckNodeOnDamage(nodeID);
            //진동 호출
            VibrationManager.CallVibration();

            //죽었으면 모든 노드 값을 초기화 한다.
            if (!Enemy_Script.Damage(_resultDamage)) 
            {
                _instance.ResetAllNode();
                _instance._lastNode = -1;
            }
            LineTracer.instance.SetDrawLineEnd(_instance._patternNodes[nodeID].transform.position);
            Global_BattleEventSystem.CallOnGainEXP(10f);
        }

        #region//nodereach
        //전투가 시작될 때 사용하는 코드
        void StartTriggerNode()
        {
            if (_lastNode != 4)
            {
                _nodeHistory = new List<int>(1) { 4 };
                SetTriggerNodeByID(4);
            }

        }

        //확정으로 다음 노드를 지정해주는 코드
        public void SetTriggerNodeByID(int id)
        {
            SetNodeToNextReach(id);
        }


        //데미지가 가해졌을때 다음 노드를 결정하는 메소드
        void CheckNodeOnDamage(int nodeID)
        {
            if (_isRandomNodeSetMode)
            {
                _lastNode = nodeID;
                ReachTriggeredNode_Random(nodeID);
                //기존의 노드들을 그냥 랜덤으로 놓는 부분들을 만든다.
            }
            else
            {
                //만약
                if (_currentPresetNodeNumber < _presetNodes.Count) 
                {
                    ResetAllNode();
                    SetNodeToNextReach(_presetNodes[_currentPresetNodeNumber]);
                    if (_currentPresetNodeNumber != 0)
                        PresetPatternShower.HidePresetPatternByID(_currentPresetNodeNumber - 1);
                    _currentPresetNodeNumber++;
                }
                else
                {
                    _lastNode = nodeID;
                    _isRandomNodeSetMode = true;
                    ReachTriggeredNode_Random(nodeID);
                    ShowDebugtextScript.SetDebug("Pattern Success!");

                }
                //처음의 공격은 무시한다.
            }


        }


        //지금이 랜덤 노드를 선택하는 상황인가 아닌가.
        bool _isRandomNodeSetMode = true;
        //지금 패턴이 설치 되었는가.
        bool _IsCurrentNodeSetted = false;

        [SerializeField]
        List<int> _presetNodes = new List<int>();
        [SerializeField]
        int _currentPresetNodeNumber = 0;

        // 패턴 세팅을 하는곳 
        void SetSkillToPresetNodeFollow(EDrawPatternPreset drawPattern)
        {
            if (!_isRandomNodeSetMode && !_IsCurrentNodeSetted)
            {
                _currentPresetNodeNumber = 0;
                _presetNodes = S_PatternStorage.S_PatternPresetDic[drawPattern];
                PresetPatternShower.ShowPresetPatternAll();
                //presetDataDic 은 새로운 딕셔너리로 키값으로EPresetOfDrawPattern를 받는다.
                _IsCurrentNodeSetted = true;

            }
            else
            {
                Debug.Log("AlreadyAnother" + _isRandomNodeSetMode + _IsCurrentNodeSetted);
            }
        }


        //노드를 랜덤으로 배치하는 메소드 id는 겹치지않도록 하는것 
        public void ReachTriggeredNode_Random(int reachedNode)
        {
            //Debug.Log("reached : " + _reachedNode);
            ResetAllNode();
            //기존의 도달한 위치는 사용불가로 만들어야한다.
            _inactivatedNode.Remove(reachedNode);
            //추후 여러개의 도달점을 가져야할때를 위해서 무작위로 한다.
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
        #endregion



        #region//charge

        //일시정지용 코드임 ㅇㅅㅇ
        bool _isPaused = false;
        [SerializeField]
        float _maxCharge = 100;
        [SerializeField]
        float _currentCharge = 0;
        [SerializeField]
        float _chargeAmount = 25f;
        [SerializeField]
        float _chargeReduction = 10f;

        [SerializeField]
        bool _isChargeStart = false;

        void CheckIsCharge() 
        {
            // 만약 일시정지 상태면 그냥 넘김
            if (_isPaused)
                return;

            if (_isChargeStart)
            {
                //ShowDebugtextScript.SetDebug(_currentCharge.ToString());
                if (_currentCharge > 0)
                {
                    _currentCharge -= Time.deltaTime* _chargeReduction;
                    ChargeGaugeUIScript.SetChargeGauge(_currentCharge/ _maxCharge);
                }
                else 
                {
                    _currentCharge = 0;
                    Global_BattleEventSystem.CallOnChargeEnd();
                    EndChargePattern();
                }
            }

        }
        void SetGaugeChange()
        {
            if (_isChargeStart) return;
            _currentCharge += _chargeAmount;
            if (_maxCharge <= _currentCharge)
            {
                StartPatternSkill();
            }
            ChargeGaugeUIScript.SetChargeGauge(_currentCharge/ _maxCharge);
        }
        void StartPatternSkill()
        {
            Global_BattleEventSystem.CallOnChargeStart();
            ChargeGaugeUIScript.StartChargeSkill();
            CameraShaker.ShakeCamera(3f, 0.5f);

            _isRandomNodeSetMode = false;
            //플레이어에게 패턴을 받아온다.
            SetSkillToPresetNodeFollow(Player_Script.GetPlayerStatus()._currentChargePattern);
            _isChargeStart = true;
        }

        void EndChargePattern()
        {
            Global_BattleEventSystem.CallOnChargeEnd();
            ChargeGaugeUIScript.EndChargeSkill();
            _isChargeStart = false;
        }


        public void SetNonTotalPauseOn()
        {
            _isPaused = true;
        }

        public void SetNonTotalPauseOff()
        {
            _isPaused = false;
        }


        #endregion


        #region

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

        #endregion
    }
}