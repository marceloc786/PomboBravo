using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;

    public LineRenderer lineFront, lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D passaroCol;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D spring;
    private Vector2 prevVel;
    private Rigidbody2D passaroRB;

    //limite do elastico
    private Transform catapult;
    private Ray rayToMT;

    // Use this for initialization
    void Start()
    {
        SetupLine();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        passaroCol = GetComponent<CircleCollider2D>();
        spring = GetComponent<SpringJoint2D>();
        passaroRB = GetComponent<Rigidbody2D>();

        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);
    }

    // Update is called once per frame
    void Update() {
        LineUpdate();
        SpringEffect();
        prevVel = passaroRB.velocity;
        if (passaroRB.isKinematic == false)
        {
            MataPassaro();
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;

        catapultToBird = cursorPosition - catapult.position;
        if (catapultToBird.magnitude > 3f)
        {
            rayToMT.direction = catapultToBird;
            cursorPosition = rayToMT.GetPoint(3f);
        }

        transform.position = cursorPosition;
        
    }

    void OnMouseUp()
    {
        passaroRB.isKinematic = false;
    }

    void SetupLine()
    {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);
    }

    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        pointL = leftCatapultRay.GetPoint(catapultToBird.magnitude + passaroCol.radius);

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);
    }

    void SpringEffect()
    {
        if(spring != null)
        {
            if(passaroRB.isKinematic == false)
            {
                if(prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude)
                {
                    lineFront.enabled = false;
                    lineBack.enabled = false;
                    Destroy(spring);
                    passaroRB.velocity = prevVel;
                }
            }
        }
    }

    void MataPassaro()
    {
        if (passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping())
        {
            StartCoroutine(TempoMorte());
        }
    }

    IEnumerator TempoMorte()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
