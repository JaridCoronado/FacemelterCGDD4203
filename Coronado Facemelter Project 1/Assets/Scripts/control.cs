using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    public Transform player;
    [SerializeField] private int speed;
    bool PressScreen = false;
    Vector2 CenterPoint;
    Vector2 MovePoint;
    public Transform OutCricle;
    public Transform InCricle;
    private float OutWidth;
    private float OutHeight;
    private float InWidth;
    private float InHeight;
    public int size;
    private Vector2 ScreenBounds;
    //private GameManger gameManger;

    private void Start()
    {
        //gameManger = GameObject.FindGameObjectWithTag("GameManger").GetComponent<GameManger>();
        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        OutWidth = OutCricle.transform.GetComponent<SpriteRenderer>().bounds.extents.x; 
        OutHeight = OutCricle.transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        InWidth = InCricle.transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        InHeight = InCricle.transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            CenterPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            OutCricle.transform.position = CenterPoint * -1;
            InCricle.transform.position = CenterPoint * -1;
            //if (gameManger.gameing)
            //{
            //    OutCricle.GetComponent<SpriteRenderer>().enabled = true;
            //    InCricle.GetComponent<SpriteRenderer>().enabled = true;
            //}
            
        }
        if (Input.GetMouseButton(0)){
            PressScreen = true;
            MovePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            PressScreen = false;
        }
    }

    private void FixedUpdate()
    {
        if (PressScreen)
        {
            Vector2 offset = MovePoint - CenterPoint;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            movment(direction*-1);
            Vector3 OutC = OutCricle.transform.position;
            OutC.x = Mathf.Clamp(OutC.x, ScreenBounds.x + OutWidth, ScreenBounds.x * -1 - OutWidth);
            OutC.y = Mathf.Clamp(OutC.y, ScreenBounds.y + OutHeight, ScreenBounds.y * -1 - OutHeight);
            OutCricle.transform.position = OutC;
            Vector3 InC =InCricle.transform.position;
            InC.x = Mathf.Clamp(InC.x, ScreenBounds.x + InWidth, ScreenBounds.x * -1 - InWidth);
            InC.y = Mathf.Clamp(InC.y, ScreenBounds.y + InHeight, ScreenBounds.y * -1 - InHeight);
            InCricle.transform.position = InC;
            InCricle.transform.position = new Vector2(CenterPoint.x + direction.x, CenterPoint.y + direction.y)*-1;
        }
        else
        {
            OutCricle.GetComponent<SpriteRenderer>().enabled = false;
            InCricle.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void movment(Vector2 direction)
    {
        player.Translate(direction * speed * Time.deltaTime);
    }
}
