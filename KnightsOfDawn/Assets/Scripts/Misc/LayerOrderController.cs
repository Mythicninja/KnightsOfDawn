using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayerController : MonoBehaviour
{
    public string targetSortingLayer = "YourTargetSortingLayer"; // what layer do you want to send it to
    public int newOrderInLayer = 0; // Adjust this value as needed
    public float returnTime = 2f;   // Time to return to the original order
    private int originalOrderInLayer;
    private bool isActionInProgress = false;
    private void Start()
    {
        // Get the original order in layer
        originalOrderInLayer = GetComponent<SpriteRenderer>().sortingOrder;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Check for the condition to change the order in layer
        {
            if (!isActionInProgress)
            {
                isActionInProgress = true;
                ChangeOrderInLayer(newOrderInLayer);
                StartCoroutine(ReturnOrginalOrder(returnTime));
            }
        }
    }
    private void ChangeOrderInLayer(int newOrder)
    {
        GetComponent<SpriteRenderer>().sortingLayerName = targetSortingLayer;  // Set the target sorting layer
        GetComponent<SpriteRenderer>().sortingOrder = newOrder; // Change the order in layer
    }

    private IEnumerator ReturnOrginalOrder(float delay) {
        yield return new WaitForSeconds(delay);
        ChangeOrderInLayer(originalOrderInLayer);
        isActionInProgress = false;
    }
}

