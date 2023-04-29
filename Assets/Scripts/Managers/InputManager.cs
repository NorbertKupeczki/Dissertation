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

    /// <summary>
    /// Returns the currently active agent.
    /// </summary>
    /// <returns></returns>
    public Agent GetSelectedAgent()
    {
        return _selectedAgent;
    }

    /// <summary>
    /// Updates the state of the mouse.
    /// </summary>
    private void UpdateMouse()
    {
        PointerEventData eventData = new(EventSystem.current);

        // If the mouse cursor is above a UI element at the time of clicking.
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

    /// <summary>
    /// Updates the keyboard inputs.
    /// </summary>
    private void UpdateKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectAgent();
        }
    }

    /// <summary>
    /// Updates the camera control inputs.
    /// </summary>
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

    /// <summary>
    /// Invoke the event to deselect an agent.
    /// </summary>
    private void DeselectAgent()
    {
        EventManager.OnDeselect();
        _selectedAgent = null;
    }

    /// <summary>
    /// Checks whether the pointer is over a UI element when a mouse button is pressed.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns>Bool</returns>
    private static bool IsMouseOverUi(PointerEventData eventData)
    {
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Checks whether any mouse button has been pressed.
    /// </summary>
    /// <returns>Bool</returns>
    private static bool AnyMouseButtonClicked()
    {
        return Input.GetMouseButtonDown(InputData.LEFT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(InputData.RIGHT_MOUSE_BUTTON) || 
               Input.GetMouseButtonDown(InputData.MIDDLE_MOUSE_BUTTON);
    }

    /// <summary>
    /// Checks whether an agent has been clicked on.
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns>Agent</returns>
    private static Agent CheckAgentClicked(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Physics.Raycast(ray, out RaycastHit hit);
        if (!hit.collider) return null;
        hit.collider.gameObject.TryGetComponent(out Agent agent);
        return agent;
    }

    /// <summary>
    /// Checks whether the ground plane has been clicked.
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <returns>Bool</returns>
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

    /// <summary>
    /// Invokes the positive area effect.
    /// </summary>
    public void PositiveAreaEffect()
    {
        EventManager.OnPositiveAreaEffect();
    }

    /// <summary>
    /// Invokes the negative area effect.
    /// </summary>
    public void NegativeAreaEffect()
    {
        EventManager.OnNegativeAreaEffect();
    }
}
