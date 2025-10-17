using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    [SerializeField] private List<Color> _colorPriorityList = new List<Color>();
    // Dictionary contains color and active pair
    private Dictionary<Color, bool> _colorDict = new Dictionary<Color, bool>();

    [SerializeField] private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        foreach (Color color in _colorPriorityList)
            _colorDict.Add(color, false);

        _material = _renderer.material;
        SetColorActive(_colorPriorityList.Count-1);
    }

    public void SetColorActive(int id)
    {
        _colorDict[_colorPriorityList[id]] = true;
        SetPriorityColor();
    }

    public void SetColorInactive(int id)
    {
        _colorDict[_colorPriorityList[id]] = false;
        SetPriorityColor();
    }
    
    private void SetPriorityColor()
    {
        foreach (Color color in _colorPriorityList)
        {
            if (_colorDict[color])
            {
                _material.color = color;
                return;
            } 
        }
    }
}
