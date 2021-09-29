using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAndZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] private float minZoom = 1.0f;
    [SerializeField] private float maxZoom = 8.0f;
    [SerializeField] private float zoomIncrement = 0.01f;
    [SerializeField] private Canvas canvas; // The canvas
    [SerializeField] private float zoomSpeed = 0.5f;        // The rate of change of the canvas scale factor
    //MAKE SURE THIS GOES ONTO THE CAMERA GAME OBJECT!!!!!!!!!!!

    // Update is called once per frame
    void Update() { TouchZoom(); }

    void Zoom(float inc) { Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - inc, minZoom, maxZoom); }

    void TouchZoom()
    {
        //only true on the start of the touch
        if (Input.GetMouseButtonDown(0)) { touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); }

        ////This is for the pinch zoom effect for mobile
        //if (Input.touchCount == 2)
        //{
        //    Touch touchZero = Input.GetTouch(0);
        //    Touch touchOne = Input.GetTouch(1);

        //    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        //    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        //    float prevMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        //    float currentMag = (touchZero.position - touchOne.position).magnitude;

        //    float difference = currentMag - prevMag;

        //    Zoom(difference * zoomIncrement);
        //}

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // ... change the canvas size based on the change in distance between the touches.
            canvas.scaleFactor -= deltaMagnitudeDiff * zoomSpeed;

            // Make sure the canvas size never drops below 0.1
            canvas.scaleFactor = Mathf.Max(canvas.scaleFactor, 0.1f);
        }

        //returns true for the entirety of when the touch is still down
        else if (Input.GetMouseButton(0))
        {
            /*We subtract camera here becuase the camera needs to 
             * move in the opposite direction of where the touch is moving. */
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
}

/*MAKE
 * SURE
 * THIS
 * GOES
 * ON
 * CAMERA
 * GAME
 * OBJECT
 */
