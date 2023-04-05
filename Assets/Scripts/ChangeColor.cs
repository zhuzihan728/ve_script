using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public GameObject button;
    public Renderer buttonRenderer;
    [SerializeField] private Color newColor = Color.white;
    [SerializeField] private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        buttonRenderer = button.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeColorButton(float duration = 0.1f)
    {
        StartCoroutine(ChangeColorCoroutine(duration));
    }
    // public void ActivateColorforReplay()
    // {
    //     buttonRenderer.material.color = newColor;
    // }
    // public void DeactivateColorforReplay()
    // {
    //     buttonRenderer.material.color = originalColor;
    // }

    private IEnumerator ChangeColorCoroutine(float duration)
    {
        Color originalColor = buttonRenderer.material.color;
        buttonRenderer.material.color = newColor;
        yield return new WaitForSeconds(duration);
        buttonRenderer.material.color = originalColor;
    }

}
