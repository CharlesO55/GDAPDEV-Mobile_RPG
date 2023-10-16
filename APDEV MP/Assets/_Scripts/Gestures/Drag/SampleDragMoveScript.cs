using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleDragMoveScript : MonoBehaviour , IDraggable
{

    Vector3 targetPos = Vector3.zero;
    void FixedUpdate()
    {
        //this.transform.position = Vector2.MoveTowards(targetPos);
    }

    public void OnDragInterface(DragEventArgs args)
    {
        Debug.Log("Dragging");


        if (args.ObjHit == this.gameObject)
        {
            Vector2 fingerPos = args.TrackedFinger.screenPosition;

            
            
            Ray ray = Camera.main.ScreenPointToRay(fingerPos);
            Vector3 worldPos = ray.GetPoint(5);

            this.targetPos = worldPos;
            this.transform.position = worldPos;
        }

        /*Vector2 fingerPos = args.TrackedFinger.screenPosition;
        Vector3 posInReal = Camera.main.ScreenToWorldPoint(fingerPos);


        
        this.transform.position = Vector2.MoveTowards(this.transform.position, posInReal, 5);*/
    }
}