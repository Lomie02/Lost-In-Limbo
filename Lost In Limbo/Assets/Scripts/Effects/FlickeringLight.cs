using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField]float m_Duration;
    
    Light m_LightObject;
    float m_Timer = 0;

    void Start()
    {
        m_LightObject = GetComponent<Light>();
        m_Timer = m_Duration;
    }

    void FixedUpdate()
    {
        m_Timer -= Time.deltaTime;

        if (m_Timer <= 0)
        {
            float LightIntensity = Random.Range(0, 3);
            if (LightIntensity == 1)
            {
                m_LightObject.enabled = false;
            }
            else
            {
                m_LightObject.enabled = true;
            }

            m_Timer = m_Duration;
        }
    }
}
