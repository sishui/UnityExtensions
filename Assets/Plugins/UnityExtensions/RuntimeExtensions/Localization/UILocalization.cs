using UnityEngine;

#if TEXT_MESH_PRO
using UIText = TMPro.TMP_Text;
#else
using UIText = UnityEngine.UI.Text;
#endif

namespace UnityExtensions
{
    /// <summary>
    /// UI 本地化组件. 使用时，在 UIText 组件上填写文本名称
    /// </summary>
    [AddComponentMenu("Unity Extensions/UI Localization")]
    [RequireComponent(typeof(UIText))]
    public class UILocalization : ScriptableComponent
    {
        [SerializeField] bool _autoUpdateWidth;
        [SerializeField] bool _autoUpdateHeight;

        string _textName;
        UIText _target;
        int _languageIndex = -1;


        void Awake()
        {
            _target = GetComponent<UIText>();
            _textName = _target.text;
        }


        void OnEnable()
        {
            if (_languageIndex != LocalizationManager.languageIndex)
            {
                UpdateLanguage(_languageIndex, LocalizationManager.languageIndex);
            }

            LocalizationManager.onLanguageChanged += UpdateLanguage;
        }


        void OnDisable()
        {
            LocalizationManager.onLanguageChanged -= UpdateLanguage;
        }


        protected virtual void UpdateLanguage(int oldLanguage, int newLanguage)
        {
            _languageIndex = newLanguage;
            LocalizationManager.UpdateUI(_target, _textName);

            if (_autoUpdateWidth || _autoUpdateHeight)
            {
                var size = this.rectTransform().sizeDelta;
                if (_autoUpdateWidth) size.x = _target.preferredWidth;
                if (_autoUpdateHeight) size.y = _target.preferredHeight;
                this.rectTransform().sizeDelta = size;
            }
        }

    } // class UILocalization

} // UnityExtensions