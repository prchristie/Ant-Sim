using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AntFSM : MonoBehaviour
{
    public float forwardSpeed = 1f;
    public float rotationSpeed = 45f;
    public LayerMask foodLayer;
    public GameObject homeNest;

    bool hasFood;
    GameObject foodTarget;
    StateMachine fsm;

    // Start is called before the first frame update
    void Start()
    {
        hasFood = false;

        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        fsm = new StateMachine();
        InitFSMStates();
        InitFSMTransitions();
        fsm.SetStartState("Search");
        fsm.Init();
    }

    private void InitFSMTransitions()
    {
        fsm.AddTransition(new Transition(
            "Search", "RetrieveFood",
            (transition) => foodTarget != null, true
        ));

        fsm.AddTransition(new Transition(
            "RetrieveFood", "Search",
            (Transition) => foodTarget == null, true
        ));

        fsm.AddTransition(new Transition(
            "RetrieveFood", "ReturnHome",
            (Transition) => hasFood, true
        ));

        fsm.AddTransition(new Transition(
            "ReturnHome", "Search",
            (Transition) => !hasFood, true
        ));
    }

    private void InitFSMStates()
    {
        fsm.AddState("Search", new State(
            onLogic: (state) => SearchForFood()
        ));

        fsm.AddState("RetrieveFood", new State(
            onLogic: (State) => RetrieveFood()
        ));

        fsm.AddState("ReturnHome", new State(
            onLogic: (State) => ReturnHome()
        ));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!hasFood && other.tag == "Food") {
            other.GetComponent<FoodSource>().TakeFood();
            hasFood = true;

        }

        if(hasFood && other.gameObject == homeNest) {
            hasFood = false;
        }
    }

    void ReturnHome() {
        // TODO: Replace with phermone stuff

        transform.right = homeNest.transform.position - transform.position;
        transform.Translate(transform.right * Time.deltaTime * forwardSpeed, Space.World);
    }

    void LookForFood() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 30, foodLayer);

        if(hit.collider != null && hit.transform.tag == "Food") {
            foodTarget = hit.transform.gameObject;
            Debug.DrawLine(transform.position, hit.transform.position, Color.green);
        } else {
            foodTarget = null;
        }
    }

    void RetrieveFood() {
        transform.right = foodTarget.transform.position - transform.position;
        transform.Translate(transform.right * Time.deltaTime * forwardSpeed, Space.World);

        LookForFood();
    }

    void SearchForFood() {
        // TODO: Add stuff about phermones
        transform.Translate(transform.right * Time.deltaTime * forwardSpeed, Space.World);
        Quaternion target = Quaternion.Euler(0, 0, Random.Range(0, 360));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed);

        LookForFood();
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnLogic();
    }
}
