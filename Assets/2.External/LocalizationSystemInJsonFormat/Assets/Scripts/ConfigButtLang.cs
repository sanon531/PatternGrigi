using UnityEngine;
using UnityEngine.UI;

public class ConfigButtLang : MonoBehaviour
{
    [SerializeField] private Text textField;
    [SerializeField] public string key_lang;

    public string title
    {
        set {
            textField.text = value;
        }
    }
    public void SwitchLanguage()
    {
        PlayerPrefs.SetString("lang", key_lang);
        FindObjectOfType<GetJson>().ReloadLang();
    }

}
