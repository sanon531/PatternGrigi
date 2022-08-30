using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetValueLang : MonoBehaviour
{
    public enum Types
    {
        DefaultText = 0,
        MeshProText = 1
    }

    public Types TypeTextField;

    [SerializeField] private string key;

    private Text _textFieldDefault;
    private TextMeshProUGUI _textFieldMesh;

    private void Awake()
    {
        if (TypeTextField == Types.DefaultText)
            _textFieldDefault = GetComponent<Text>();
        else if (TypeTextField == Types.MeshProText)
            _textFieldMesh = GetComponent<TextMeshProUGUI>();
    }
    void Start() => SetValueText();
 
    public void SetValueText()
    {
        JsonStructure item = FindObjectOfType<GetJson>().Items.Find(x => x.key == key);
        if (item != null)
        {
            if(_textFieldDefault)
                _textFieldDefault.text = item.value;
            if(_textFieldMesh)
                _textFieldMesh.text = item.value;
        }
    }

}
