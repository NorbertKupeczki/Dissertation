using UnityEngine;

public class RelationLine : MonoBehaviour
{
    private LineRenderer _line;
    private readonly Vector3 _lineHeightOffset = new(0.0f, 0.125f, 0.0f);
    private Transform _lineStart;
    private Transform _lineEnd;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _lineStart = gameObject.transform;
        ToggleLine(false);
    }

    /// <summary>
    /// Sets the starting point of the line.
    /// </summary>
    /// <param name="value"></param>
    public void SetLineStart(Transform value)
    {
        _lineStart = value;
    }

    /// <summary>
    /// Sets the end point of the line.
    /// </summary>
    /// <param name="value"></param>
    public void SetLineEnd(Transform value)
    {
        _lineEnd = value;
        UpdateLineEndpoints();
    }
    
    /// <summary>
    /// Updates the end points of the line that is to be drawn by the renderer
    /// </summary>
    private void UpdateLineEndpoints()
    {
        if (_lineStart == null || _lineEnd == null) return;
        _line.SetPosition(0, _lineStart.position + _lineHeightOffset);
        _line.SetPosition(1, _lineEnd.position + _lineHeightOffset);
    }

    /// <summary>
    /// Toggles the visibility of the line renderer.
    /// </summary>
    /// <param name="value"></param>
    public void ToggleLine(bool value)
    {
        _line.enabled = value;
    }

}
