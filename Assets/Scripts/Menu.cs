using UnityEngine;
using UnityEngine.InputSystem;
public class Menu : MonoBehaviour
{
    public GameObject MenuUi;
    private bool isMenuOpen = false;

    private InputAction escAction; // ESC 키 입력을 위한 InputAction
    
    void Awake()
    {
        // InputAction을 생성하고 ESC 키에 바인딩
        escAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        escAction.performed += ctx => ToggleMenu(); // ESC 키가 눌렸을 때 ToggleMenu 메서드 호출
        escAction.Enable(); // InputAction 활성화
    }
    void Start()
    {
        MenuUi.SetActive(false); // 메뉴 UI를 처음에는 비활성화
    }
    void ToggleMenu()
        {
        if (isMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }
    public void OpenMenu()
    {
        MenuUi.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
        isMenuOpen = true;
    }

    public void CloseMenu()
    {
        MenuUi.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
        isMenuOpen = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
