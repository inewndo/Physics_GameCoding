using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //color object glows when player looks at it
    public Color highLightcolor = new Color(1f, 0.95f, 0.6f);

    //how strong the highlight color blends w original color 
    [Range(0f, 1f)]
    public float highLightStrength = .4f;

    private Renderer _objRenderer;//render component on this obj 
    private Color _originalColor; //original color before the highlight

    private bool _isHighlighted = false;

    void Awake()
    {
        _objRenderer = GetComponent<Renderer>(); //cache the renderer
        if (_objRenderer != null)
        {
            //store original color so we can restore it after highlighting 
            //we read from the renderer material color property, we used sharedmaterial to read base color without instancing 
            _originalColor = _objRenderer.sharedMaterial.color;
        }
        else
        {
            Debug.Log($"interactable object on {gameObject.name} has no renderer, highlight wont work");
        }
    }

    public void Highlight()
    {
        if (_isHighlighted || _objRenderer == null)
        {
            //Debug.Log("No obj rendeer & ishiglighted is true");
            return;
        }

        //color.lerp blends between the original color and the highlight color by the highlight strength amt
        //we used material not shared material to create a unique instance here
        //so we dont effect every obj using the same material!
        _objRenderer.material.color = Color.Lerp(_originalColor, highLightcolor, highLightStrength);

        _isHighlighted = true;
    }

    //called by objectgrabber when the player looks away --> restore original color
    public void Unhighlight()
    {
        if (!_isHighlighted || _objRenderer == null) return;
        _objRenderer.material.color = _originalColor;

        _isHighlighted = false;
    }
}
