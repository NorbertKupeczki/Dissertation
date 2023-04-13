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

        if (IsMouseOverUi(eventData)) return;
        

        if (Input.GetMouseButtonDown(Data.LEFT_MOUSE_BUTTON))
        {   
            Agent agent = CheckAgentClicked(eventData);
            if (agent == null || _selectedAgent == agent) return;
            EventManager.OnAgentSelected(agent);
            _selectedAgent = agent;
        }
        else if (Input.GetMouseButtonDown(Data.RIGHT_MOUSE_BUTTON))
        {
            EventManager.OnDeselect();
        }
        else if (Input.GetMouseButtonDown(Data.MIDDLE_MOUSE_BUTTON))
        {
            Debug.Log("Place AoE event marker");
        }
    }

    private static void UpdateKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.OnDeselect();
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
        return Input.GetMouseButtonDown(Data.LEFT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(Data.RIGHT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(Data.MIDDLE_MOUSE_BUTTON);
    }

    private static Agent CheckAgentClicked(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Physics.Raycast(ray, out RaycastHit hit);
        if (!hit.collider) return null;
        hit.collider.gameObject.TryGetComponent(out Agent agent);
        return agent;

    }
}
