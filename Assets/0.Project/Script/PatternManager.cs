using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
        //가운데 점에서부터 시작한다.
        SetTriggerNodeById(4);
    }
    // Update is called once per frame
    void Update()
    {
    }




    //나중에 추가할 만한 이벤트와 연관 짓기 위해서.
    public void SetTriggerNodeById(int _id) 
    {
        SetNodeToNextReach(_id);
    }

    //나중에 이벤트로 넣어서 
    //만약 여러개의 위치가 지정 되어있다면 그거는 안된다고 표시함.
    public void ReachTriggeredNode_Random(int _reachedNode) 
    {
        //Debug.Log("reached : " + _reachedNode);
        ResetAllNode();
        //기존의 도달한 위치는 사용불가로 만들어야한다.
        _inactivatedNode.Remove(_reachedNode);
        //추후 여러개의 도달점을 가져야할때를 위해서 무작위로 한다.
        RandomNodeSet();
    }
    public

    //이렇게 만드는거는 이제 다음 목표점이 2개이상일때 무작위로 배치할때 사용할것.
    void RandomNodeSet() 
    {
        int i = _inactivatedNode.Count;
        int _deleteTarget = Random.Range(0, i);
        Debug.Log(i+"set" + _deleteTarget);
        SetNodeToNextReach(_inactivatedNode[_deleteTarget]);
        _inactivatedNode.RemoveAt(_deleteTarget);
    }

    void ResetAllNode() 
    {
        _inactivatedNode = _defaultNode.ToList();
        foreach (PatternNodeScript _nodes in _patternNodes) 
            _nodes.SetIsReachable(false);

    }

    void SetNodeToNextReach(int i)
    {
        Debug.Log("input " + i );
        if (_inactivatedNode.Contains(i))
            _patternNodes[i].SetIsReachable(true);
        else
            Debug.LogError("Wrong node Error: Already Exist");  
    }

}
