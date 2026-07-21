using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NetworkManagerMenu : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startServerButton;
    [SerializeField] private Button startClientButton;

    [SerializeField] private GameObject stopMenu;
    [SerializeField] private Button stopButton;

    private void Awake()
    {
        BindStartButton(startHostButton, () => NetworkManager.Singleton.StartHost());
        BindStartButton(startServerButton, () => NetworkManager.Singleton.StartServer());
        BindStartButton(startClientButton, () => NetworkManager.Singleton.StartClient());

        stopButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            stopMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        });
    }

    private void BindStartButton(Button button, System.Func<bool> startAction)
    {
        button.onClick.AddListener(() =>
        {
            if (startAction()) stopMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        });
    }
}
