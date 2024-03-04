using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorModule : MonoBehaviour
{
    [SerializeField] bool m_IsDoorLocked = false;

    [SerializeField] Animator m_DoorAnimator;

    [SerializeField] bool m_DoorState = true;

    private void Start()
    {
        m_DoorAnimator = GetComponent<Animator>();
        UpdateDoorState();
    }
    public bool GetDoorState()
    {
        return m_DoorState;
    }
    public bool IsDoorLocked()
    {
        return m_IsDoorLocked;
    }
    void UpdateDoorState()
    {
        if (!m_DoorState)
        {
            m_DoorState = true;
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
        else
        {
            m_DoorState = false;
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
    }

    public void LockDoor()
    {
        m_DoorState = false;
        m_DoorAnimator.SetBool("DoorState", m_DoorState);

        m_IsDoorLocked = true;
    }

    public void UnlockDoor()
    {
        m_IsDoorLocked = false;
    }

    public void CycleDoorNormal()
    {
        CycleDoorStates();
    }

    public bool CycleDoorStates()
    {
        if (m_IsDoorLocked)
            return false;

        if (!m_DoorState)
        {
            m_DoorState = true;
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }
        else
        {
            m_DoorState = false;
            m_DoorAnimator.SetBool("DoorState", m_DoorState);
        }


        return true;
    }
}
