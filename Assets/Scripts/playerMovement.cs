using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class playerMovement : MonoBehaviour
{
    public bool started = false;
    public float SPEED = 100f;
    public float ROTATION_SPEED = 0.9f;
    bool isRotating = false;
    float roationDirection;

    Vector3 finalRotation;

    Queue<Vector3> finalPositions = new Queue<Vector3>();

    GameObject start;

    Animator animator;

    float startHeight;

    GameObject bloom;

    int score = 0;

    bool finished = false;
    public GameObject canvas;

    HashSet<GameObject> collectables = new HashSet<GameObject>();

    void Start(){
        startHeight = transform.position.y;
        animator = GetComponent<Animator>();
        finalRotation = transform.forward;

        bloom = PrefabUtility.LoadPrefabContents("Assets/Prefab/Bloom.prefab");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(finalPositions.Count);
        if(Input.GetKeyDown(KeyCode.Space)){
            started = !started;
            GetComponent<SphereCollider>().enabled = !GetComponent<SphereCollider>().enabled;
        }

        if(Input.GetKeyDown(KeyCode.R)){
            started = false;
            transform.position = new Vector3(start.transform.position.x,transform.position.y, start.transform.position.z);
            transform.rotation = start.transform.rotation;
            finalRotation = transform.forward;
            finalPositions.Clear();
            score = 0;
            GetComponent<SphereCollider>().enabled = false;
        }

        if(finalPositions.Count > 0){
            Debug.Log("DISTANCE");
            Debug.Log(Vector3.Distance(transform.position, finalPositions.Peek()));
            if(started && Vector3.Distance(transform.position, finalPositions.Peek()) > 0.001f){
                animator.SetBool("walking", true);
                transform.position = Vector3.MoveTowards(transform.position, finalPositions.Peek(), SPEED*Time.deltaTime);
            }
            else{
                finalPositions.Dequeue();
                if(finished){
                    canvas.SetActive(true);
                    canvas.GetComponent<WinUI>().setScore(score);
                    started = false;
                }
            } 
        }else
            animator.SetBool("walking", false);

        
        if(started && Vector3.Distance(transform.forward, finalRotation) > 0.001f){
            Vector3 newDir = Vector3.RotateTowards(transform.forward, finalRotation, ROTATION_SPEED*Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    void MoveStart(GameObject path){ 
        Vector3 finalPosition = path.gameObject.transform.position - transform.forward/2;
        finalPosition.y = startHeight;
        finalPositions.Enqueue(finalPosition);
        drawCircle(finalPosition,Color.red);
    }

    void MoveForward(GameObject path){
        Vector3 finalPosition;
        if(Vector3.Angle(path.transform.forward, transform.forward) > 45 && Vector3.Angle(path.transform.forward, transform.forward) < 135){
            Debug.Log("WRONG CONNECTION IN FORWARD");
            return;
        }
        if(Vector3.Angle(path.transform.forward, transform.forward) < 45)
            finalPosition = path.gameObject.transform.position - path.gameObject.transform.forward/2;
        else
            finalPosition = path.gameObject.transform.position + path.gameObject.transform.forward/2;
        finalPosition.y = startHeight;
        finalPositions.Enqueue(finalPosition);
        drawCircle(finalPosition,Color.red);
    }

    void MoveTurn(GameObject path){
        Vector3 finalPosition;
        Vector3 middlePosition = new Vector3(path.transform.position.x, transform.position.y, path.transform.position.z) + (path.transform.right - path.transform.forward)/5;
        middlePosition.y = startHeight;
        float angle = Vector3.Angle(path.transform.forward, transform.forward);
        if(angle > 180-45 && angle < 180+45){
            finalPosition = path.gameObject.transform.position + path.transform.right/2;
            finalPosition.y = startHeight;
            finalRotation = -path.transform.right;
        }else if(angle > 90-45 && angle < 90+45){
            finalPosition = path.gameObject.transform.position - path.transform.forward/2;
            finalPosition.y = startHeight;
            finalRotation = path.transform.forward;
        }
        else{
            Debug.Log("WRONG CONNECTION IN TURN");
            return;
        }
        finalPositions.Enqueue(middlePosition);
        finalPositions.Enqueue(finalPosition);
        drawCircle(middlePosition,Color.red);
        drawCircle(finalPosition,Color.red);
    }

    void MoveBridge(GameObject path){
        Vector3 firstPosition, secondPostion, finalPosition;
        Debug.Log(Vector3.Angle(path.transform.forward, transform.forward));
        if(Vector3.Angle(path.transform.forward, transform.forward) < 45 && Vector3.Angle(path.transform.forward, transform.forward) > 135){
            Debug.Log("WRONG CONNECTION IN BRIDGE "+Vector3.Angle(path.transform.forward, transform.forward));
            return;
        }
        if(Vector3.SignedAngle(path.transform.forward, transform.forward, path.transform.up) < 0){
            Debug.Log(">>>>>>>>>>>>>>>A");
            firstPosition = path.gameObject.transform.position - path.gameObject.transform.right/6;
            secondPostion = path.gameObject.transform.position + path.gameObject.transform.right/6;
            finalPosition = path.gameObject.transform.position + path.gameObject.transform.right/2;
        }
        else{
            Debug.Log(">>>>>>>>>>>>>>B");
            firstPosition = path.gameObject.transform.position + path.gameObject.transform.right/6;
            secondPostion = path.gameObject.transform.position - path.gameObject.transform.right/6;
            finalPosition = path.gameObject.transform.position - path.gameObject.transform.right/2;
        }
        finalPosition.y = startHeight;;
        firstPosition.y = secondPostion.y = startHeight + path.transform.lossyScale.y/4;
        drawCircle(firstPosition,Color.red);
        drawCircle(secondPostion,Color.red);
        drawCircle(finalPosition,Color.red);
        finalPositions.Enqueue(firstPosition);
        finalPositions.Enqueue(secondPostion);
        finalPositions.Enqueue(finalPosition);
    }

    void MoveEnd(GameObject path){
        finalPositions.Enqueue(path.transform.position);
        finished = true; 
    }

    void OnTriggerEnter(Collider other)
    {
        MyPath otherPath = other.GetComponent<MyPath>();

        Debug.Log("TRIGGERED");

        if(otherPath){
            Debug.Log("TRIGGERED PATH");
            switch(otherPath.path){
                case MyPath.PATHS.START:
                    Debug.Log("START CASE");
                    MoveStart(other.gameObject);
                    start = other.gameObject;
                    break;
                case MyPath.PATHS.FORWARD:
                    MoveForward(other.gameObject);
                    break;
                case MyPath.PATHS.BRIDGE:
                    MoveBridge(other.gameObject);
                    break;
                case MyPath.PATHS.TURN:
                    MoveTurn(other.gameObject);
                    break;
                case MyPath.PATHS.END:
                case MyPath.PATHS.ALLSIDES:
                    MoveEnd(other.gameObject);
                    break;
            }
        }

        if(other.GetComponent<Collectable>()){
            Instantiate(bloom, other.transform.position, other.transform.rotation);
            other.gameObject.GetComponent<Collectable>().Collect();
            score++;
            collectables.Add(gameObject);
        }
    }

    void drawCircle(Vector3 center, Color c){
        float size = 0.1f;
        for(int i = 0; i < 360; i+=15){
            Debug.DrawLine(center+new Vector3(Mathf.Cos(i)*size, Mathf.Sin(i)*size, 0), center+new Vector3(Mathf.Cos(i+1)*size, Mathf.Sin(i+1)*size, 0), c, 5);
            Debug.DrawLine(center+new Vector3(Mathf.Cos(i)*size, 0, Mathf.Sin(i)*size), center+new Vector3(Mathf.Cos(i+1)*size, 0, Mathf.Sin(i+1)*size), c, 5);
            Debug.DrawLine(center+new Vector3(0, Mathf.Cos(i)*size, Mathf.Sin(i)*size), center+new Vector3(0, Mathf.Cos(i+1)*size, Mathf.Sin(i+1)*size), c, 5);
        }
    }
}
