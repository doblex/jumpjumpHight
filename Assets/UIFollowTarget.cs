using UnityEngine.UIElements;
using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] string elementName = "TargetElement"; // Name of the VisualElement to move

    UIDocument uiDocument;
    VisualElement elementToMove;

    Camera mainCamera;


    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned. Please assign a target in the inspector.");
            return;
        }

        mainCamera = Camera.main;

        uiDocument = GetComponent<UIDocument>();
        elementToMove = uiDocument.rootVisualElement.Q<VisualElement>(elementName);
    }

    void Update()
    {
        if (target == null || elementToMove == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        if (screenPos.z > 0)
        {
            // Flip Y because UI Toolkit origin is top-left
            float invertedY = Screen.height - screenPos.y;

            elementToMove.style.left = screenPos.x - (elementToMove.resolvedStyle.width / 2);
            elementToMove.style.top = invertedY - (elementToMove.resolvedStyle.height / 2);
            elementToMove.style.display = DisplayStyle.Flex;
        }
        else
        {
            elementToMove.style.display = DisplayStyle.None;
        }
    }
}
