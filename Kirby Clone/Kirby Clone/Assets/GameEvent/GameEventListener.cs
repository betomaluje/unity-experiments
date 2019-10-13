using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public List<EventAndResponse> eventAndResponses = new List<EventAndResponse>();

    private void OnEnable()
    {
        for (int i = 0; i < eventAndResponses.Count; i++)
        {
            eventAndResponses[i].gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < eventAndResponses.Count; i++)
        {
            eventAndResponses[i].gameEvent.UnregisterListener(this);
        }
    }

    [ContextMenu("Raise Events")]
    public void OnEventRaised(GameEvent passedEvent)
    {
        for (int i = eventAndResponses.Count - 1; i >= 0; i--)
        {
            // Check if the passed event is the correct one
            if (passedEvent == eventAndResponses[i].gameEvent)
            {
                // Uncomment the line below for debugging the event listens and other details
                //Debug.Log("Called Event named: " + eventAndResponses[i].name + " on GameObject: " + gameObject.name);
                eventAndResponses[i].EventRaised();
            }
        }
    }
}

[System.Serializable]
public class EventAndResponse
{
    public string name;
    public GameEvent gameEvent;
    public UnityEvent response;
    public ResponseWithString responseForSentString;
    public ResponseWithInt responseForSentInt;
    public ResponseWithFloat responseForSentFloat;
    public ResponseWithBool responseForSentBool;
    public ResponseWithVector2 responseForSentVector2;
    public ResponseWithVector3 responseForSentVector3;
    public ResponseWithGameObject responseWithGameObject;
    public ResponseWithAttackEvent responseWithAttackEvent;

    public void EventRaised()
    {
        // default/generic
        if (response.GetPersistentEventCount() >= 1) // always check if at least 1 object is listening for the event
        {
            response.Invoke();
        }

        // string
        if (responseForSentString.GetPersistentEventCount() >= 1)
        {
            responseForSentString.Invoke(gameEvent.sentString);
        }

        // int
        if (responseForSentInt.GetPersistentEventCount() >= 1)
        {
            responseForSentInt.Invoke(gameEvent.sentInt);
        }

        // float
        if (responseForSentFloat.GetPersistentEventCount() >= 1)
        {
            responseForSentFloat.Invoke(gameEvent.sentFloat);
        }

        // bool
        if (responseForSentBool.GetPersistentEventCount() >= 1)
        {
            responseForSentBool.Invoke(gameEvent.sentBool);
        }

        // Vector2
        if (responseForSentVector2.GetPersistentEventCount() >= 1)
        {
            responseForSentVector2.Invoke(gameEvent.sentVector2);
        }

        // Vector3
        if (responseForSentVector3.GetPersistentEventCount() >= 1)
        {
            responseForSentVector3.Invoke(gameEvent.sentVector3);
        }

        // GameObject        
        if (responseWithGameObject.GetPersistentEventCount() >= 1)
        {
            responseWithGameObject.Invoke(gameEvent.sentGameObject);
        }

        // AttackEvent
        if (responseWithAttackEvent.GetPersistentEventCount() >= 1)
        {
            responseWithAttackEvent.Invoke(gameEvent.sentAttackEvent);
        }
    }
}

[System.Serializable]
public class ResponseWithString : UnityEvent<string>
{
}

[System.Serializable]
public class ResponseWithInt : UnityEvent<int>
{
}

[System.Serializable]
public class ResponseWithFloat : UnityEvent<float>
{
}

[System.Serializable]
public class ResponseWithBool : UnityEvent<bool>
{
}

[System.Serializable]
public class ResponseWithVector2 : UnityEvent<Vector2>
{
}

[System.Serializable]
public class ResponseWithVector3 : UnityEvent<Vector3>
{
}

[System.Serializable]
public class ResponseWithGameObject : UnityEvent<GameObject>
{
}

[System.Serializable]
public class ResponseWithAttackEvent : UnityEvent<AttackEvent>
{
}