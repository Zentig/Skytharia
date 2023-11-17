using UnityEngine;
using TMPro;
using Skytharia.SaveManagement;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBehaviour
{
    [SerializeField] private string _key;
    private TextMeshProUGUI _currentText;

    private void Start()
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
                path = $"Localizations/ua.data";
                break;
            case LocalizationTypes.en:
                path = $"Localizations/en.data";
                break;
        }
        LocalizatedObject serializedData = (LocalizatedObject)SaveManager.Load<LocalizatedObject>(path, false);
        _currentText.text = serializedData.Dictionary[_key];
    }
}