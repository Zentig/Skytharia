using UnityEngine;

public class LanguageChanger : MonoBehaviour
 {
    [SerializeField] private LocalizationTypes _language;
    public void ChangeLanguage()
    {
        HeadCL.Instance.SetLanguage(_language);
    }
}

