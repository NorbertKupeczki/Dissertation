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
        UpdateKeyboardInput();
        UpdateCameraControlsInput();
    }

    public Agent GetSelectedAgent()
    {
        return _selectedAgent;
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
            DeselectAgent();
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

    private void UpdateKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectAgent();
        }
    }

    private static void UpdateCameraControlsInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.OnCameraControlMoveInput(InputData.LEFT_VECTOR);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            EventManager.OnCameraControlMoveInput(-InputData.LEFT_VECTOR);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EventManager.OnCameraControlMoveInput(InputData.RIGHT_VECTOR);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            EventManager.OnCameraControlMoveInput(-InputData.RIGHT_VECTOR);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager.OnCameraControlMoveInput(InputData.UP_VECTOR);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            EventManager.OnCameraControlMoveInput(-InputData.UP_VECTOR);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EventManager.OnCameraControlMoveInput(InputData.DOWN_VECTOR);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            EventManager.OnCameraControlMoveInput(-InputData.DOWN_VECTOR);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.OnCameraControlRotateInput(InputData.LEFT_ROTATION);
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            EventManager.OnCameraControlRotateInput(-InputData.LEFT_ROTATION);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EventManager.OnCameraControlRotateInput(InputData.RIGHT_ROTATION);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            EventManager.OnCameraControlRotateInput(-InputData.RIGHT_ROTATION);
        }
    }

    private void DeselectAgent()
    {
        EventManager.OnDeselect();
        _selectedAgent = null;
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
        }

        result = hit.point;
        return true;
    }

    public void PositiveAreaEffect()
    {
        EventManager.OnPositiveAreaEffect();
    }

    public void NegativeAreaEffect()
    {
        EventManager.OnNegativeAreaEffect();
    }
}
