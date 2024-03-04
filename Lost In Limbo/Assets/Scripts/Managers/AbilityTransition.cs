using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Animations.Rigging;

public class AbilityTransition : MonoBehaviour
{
    [SerializeField] GameObject m_LimoViewHud;
    [SerializeField] Image m_LimboViewIconFill;

    [SerializeField] Animator m_BodyAnimations;

    [SerializeField] float m_ArmLerpSpeed = 2;
    float m_LerpTimer = 0;
    bool m_IsInLimboView = false;
    [SerializeField] Light m_HandGlow;

    [SerializeField] Volume m_LimboPost;
    PlayerController m_PlayersController;
    MultiAimConstraint m_AimComp;

    [SerializeField] bool m_CanUseLimbo = false;

    //Limbo Charging
    bool m_LimboRecharging = false;
    float m_LimboTimerCharger = 0;
    [SerializeField] float m_LimboUseRate = 1;
    void Start()
    {
        m_BodyAnimations = GetComponentInChildren<Animator>();
        m_AimComp = GetComponentInChildren<MultiAimConstraint>();

        m_PlayersController = GetComponent<PlayerController>();

        m_LimoViewHud.SetActive(m_CanUseLimbo);

    }

    public void SetLimboState(bool _state)
    {
        m_CanUseLimbo = _state;
        m_LimoViewHud.SetActive(_state);
    }

    void Update()
    {
        if (!m_PlayersController.GetMovementState())
            return;

        if (Input.GetKey(KeyCode.Mouse1) && m_CanUseLimbo && !m_LimboRecharging)
        {
            m_LerpTimer = Mathf.Lerp(m_LerpTimer, 2, m_ArmLerpSpeed * Time.deltaTime);
            m_HandGlow.gameObject.SetActive(true);

            m_LimboTimerCharger -= m_LimboUseRate * Time.deltaTime;

            if (m_LimboTimerCharger <= 0)
            {
                m_LimboRecharging = true;
            }

            m_IsInLimboView = true;
        }
        else
        {
            m_LimboTimerCharger += m_LimboUseRate * Time.deltaTime;
            m_LimboTimerCharger = Mathf.Clamp(m_LimboTimerCharger, 0, 1);

            if (m_LimboTimerCharger >= 1)
            {
                m_LimboRecharging = false;
            }

            m_LerpTimer = Mathf.Lerp(m_LerpTimer, -1, m_ArmLerpSpeed * Time.deltaTime);
            m_HandGlow.gameObject.SetActive(false);
            m_IsInLimboView = false;
        }

        m_LimboViewIconFill.fillAmount = m_LimboTimerCharger;

        m_LerpTimer = Mathf.Clamp(m_LerpTimer, 0, 1);

        m_LimboPost.weight = m_LerpTimer;
        m_AimComp.weight = m_LerpTimer;
        m_BodyAnimations.SetLayerWeight(2, m_LerpTimer);
    }

    public bool IsInLimbo()
    {
        return m_IsInLimboView;
    }
}
