using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public GameObject button;
    public Renderer buttonRenderer;
    [SerializeField] private Color newColor;
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
    public void ChangeColorButton()
    {
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        Color originalColor = buttonRenderer.material.color;
        buttonRenderer.material.color = newColor;
        yield return new WaitForSeconds(10);
        buttonRenderer.material.color = originalColor;
    }

}
