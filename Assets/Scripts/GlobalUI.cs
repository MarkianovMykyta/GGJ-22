using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUI : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private GameObject _loadingIcon;
    [SerializeField] private GameObject _pressAnyKeyPanel;

    public void Initialize()
    {
        _loadingScreen.gameObject.SetActive(false);
        _pressAnyKeyPanel.gameObject.SetActive(false);
    }

    public void ShowLoadingScreen(bool isActive)
    {
        _loadingScreen.gameObject.SetActive(isActive);
    }

    public void ShowPressAnyKey(bool isActive)
    {
        _pressAnyKeyPanel.gameObject.SetActive(isActive);

        EndLoading();
    }

    private void EndLoading()
    {
        _loadingText.text = "R e a d y";//hide of any action
        _loadingIcon.SetActive(false);//hide of any action
    }
}
