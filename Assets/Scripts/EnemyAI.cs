
using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    private State state;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        state = State.Running;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void SetState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }
}
