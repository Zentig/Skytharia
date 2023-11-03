using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System.Text;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string _key;
    private TextMeshProUGUI _currentText;

    private void Awake()
    {
        _currentText = GetComponent<TextMeshProUGUI>();
        HeadCL.Instance.OnLanguageChanged += ChangeText;
    }
    private void OnDestroy()
    {
        HeadCL.Instance.OnLanguageChanged -= ChangeText;
    }
    public void ChangeText(LocalizationTypes languageKey)
    {
        string path = "";
        switch (languageKey)
        {
            case LocalizationTypes.ua:
                path = $"{Application.streamingAssetsPath}/Localizations/ua.json";
                break;
            case LocalizationTypes.en:
                path = $"{Application.streamingAssetsPath}/Localizations/en.json";
                break;
        }
        var text = File.ReadAllText(path, Encoding.UTF8);
        var serializedData = JsonConvert.DeserializeObject<LocalizatedObject>(text);
        _currentText.text = serializedData.Dictionary[_key];
    }
}