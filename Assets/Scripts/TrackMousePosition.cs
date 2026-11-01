using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrackMousePosition : MonoBehaviour
{
    // private int n = 0;
    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // var mousePosition = Mouse.current.position.ReadValue();
        // textMesh.text = mousePosition.ToString();
        textMesh.text = CodeMonkeyTransparentOverlay.GetMousePosition().ToString();
    }

    public void DebugText(string content)
    {
        textMesh.text = content;
    }
}
