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

    public void SetLineStart(Transform value)
    {
        _lineStart = value;
    }

    public void SetLineEnd(Transform value)
    {
        _lineEnd = value;
        UpdateLineEndpoints();
    }
    
    private void UpdateLineEndpoints()
    {
        if (_lineStart == null || _lineEnd == null) return;
        _line.SetPosition(0, _lineStart.position + _lineHeightOffset);
        _line.SetPosition(1, _lineEnd.position + _lineHeightOffset);
    }

    public void ToggleLine(bool value)
    {
        _line.enabled = value;
    }

}
