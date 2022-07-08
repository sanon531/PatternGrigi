using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using MoreMountains.NiceVibrations;

public class PatternManager : MonoBehaviour
{
    [SerializeField]
    List<PatternNodeScript> _patternNodes = new List<PatternNodeScript>();

    [SerializeField]
    List<int> _defaultNode = new List<int>(9) { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    [SerializeField]
    List<int> _inactivatedNode;
    public static PatternManager _instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(_instance ==null)
            _instance = this;
        _inactivatedNode = _defaultNode.ToList();

    }
    private void Start()
    {
        //��� ���������� �����Ѵ�.
        SetTriggerNodeById(4);
    }
    // Update is called once per frame
    void Update()
    {
        CheckVibration();
    }




    //���߿� �߰��� ���� �̺�Ʈ�� ���� ���� ���ؼ�.
    public void SetTriggerNodeById(int _id) 
    {
        SetNodeToNextReach(_id);
    }

    //���߿� �̺�Ʈ�� �־ 
    //���� �������� ��ġ�� ���� �Ǿ��ִٸ� �װŴ� �ȵȴٰ� ǥ����.
    public void ReachTriggeredNode_Random(int _reachedNode) 
    {
        //Debug.Log("reached : " + _reachedNode);
        ResetAllNode();
        //������ ������ ��ġ�� ���Ұ��� �������Ѵ�.
        _inactivatedNode.Remove(_reachedNode);
        //���� �������� �������� �������Ҷ��� ���ؼ� �������� �Ѵ�.
        RandomNodeSet();
    }
    public

    //�̷��� ����°Ŵ� ���� ���� ��ǥ���� 2���̻��϶� �������� ��ġ�Ҷ� ����Ұ�.
    void RandomNodeSet() 
    {
        int i = _inactivatedNode.Count ;
        int _deleteTarget = Random.Range(0, i);
        Debug.Log(i+"set" + _deleteTarget);
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
    void CallVib(float _time)
    {
        _vibAlive = true;
        _leftTime = _time;
    }
    void CheckVibration()
    {
        if (!_vibAlive)
            return;

        if (_leftTime > 0f)
        {
            _leftTime -= Time.deltaTime;
            ShowDebugtextScript._instance.SetDebug("time left" + _leftTime);
        }
        else
        {
            _vibAlive = false;
        }


    }

    public void SetIntensity(Slider _s) 
    {
        _currentIntensity = _s.value;
    }
    public void SetSharpness(Slider _s)
    {
        _currentSharpness = _s.value;
    }
    public void SetTime_Slider(Slider _s)
    {
        _limitTime = _s.value;
    }



}
