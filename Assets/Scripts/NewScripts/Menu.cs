using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    private InputAction menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        menu = InputSystem.actions.FindAction("Menu");
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MenuOpen()
    {

    }
}
