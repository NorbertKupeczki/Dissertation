using UnityEngine;
using GeneralData;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Agent _selectedAgent;
    
    private void Update()
    {
        if (AnyMouseButtonClicked()) UpdateMouse();
        UpdateKeyboard();
    }

    private void UpdateMouse()
    {
        PointerEventData eventData = new(EventSystem.current);

        // If the mouse cursor is above a UI element at the time of click.
        if (IsMouseOverUi(eventData)) return;

        // Left mouse click
        if (Input.GetMouseButtonDown(InputData.LEFT_MOUSE_BUTTON))
        {   
            Agent agent = CheckAgentClicked(eventData);
            if (agent == null || _selectedAgent == agent) return;
            EventManager.OnDeselect();
            EventManager.OnAgentSelected(agent);
            _selectedAgent = agent;
        }
        // Right mouse click
        else if (Input.GetMouseButtonDown(InputData.RIGHT_MOUSE_BUTTON))
        {
            EventManager.OnDeselect();
        }
        // Middle mouse click
        else if (Input.GetMouseButtonDown(InputData.MIDDLE_MOUSE_BUTTON))
        {
            if (CheckGroundHit(eventData, out Vector3 result))
            {
                EventManager.OnMiddleMouseClick(result);
            }
        }
    }

    private static void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.OnDeselect();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.OnPositiveAreaEffect();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager.OnNegativeAreaEffect();
        }
    }

    private static bool IsMouseOverUi(PointerEventData eventData)
    {
        
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private static bool AnyMouseButtonClicked()
    {
        return Input.GetMouseButtonDown(InputData.LEFT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(InputData.RIGHT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(InputData.MIDDLE_MOUSE_BUTTON);
    }

    private static Agent CheckAgentClicked(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Physics.Raycast(ray, out RaycastHit hit);
        if (!hit.collider) return null;
        hit.collider.gameObject.TryGetComponent(out Agent agent);
        return agent;
    }

    private static bool CheckGroundHit(PointerEventData eventData, out Vector3 result)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Physics.Raycast(ray, out RaycastHit hit,1000.0f,LayerMask.GetMask("Ground"));
        if (!hit.collider)
        {
            result = Vector3.zero;
            return false;
        };

        result = hit.point;
        return true;
    }
}
