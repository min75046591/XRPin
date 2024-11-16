///****************************************************************************
//* Copyright 2019 Xreal Techonology Limited. All rights reserved.
//*                                                                                                                                                          
//* This file is part of NRSDK.                                                                                                          
//*                                                                                                                                                           
//* https://www.xreal.com/        
//* 
//*****************************************************************************/

//namespace NRKernal.Experimental.StreammingCast
//{
//    using UnityEngine;
//    using UnityEngine.UI;

//    public class FPSConfigView : MonoBehaviour
//    {
//        public SettingRegionTrigger m_TriggerRegion;
//        public delegate void OnButtonClick(FirstPersonStreammingCast.OnResponse response);
//        public event OnButtonClick OnStreamBtnClicked;
//        public event OnButtonClick OnRecordBtnClicked;
//        public Button m_StreamBtn;
//        public Button m_RecordBtn;
//        public Transform m_PanelRoot;
//        public Color NormalColor;
//        public Color ActiveColor;

//        void Start()
//        {
//            m_StreamBtn.onClick.AddListener(() =>
//            {
//                OnStreamBtnClicked?.Invoke(OnStreamButtonResponse);
//            });

//            m_RecordBtn.onClick.AddListener(() =>
//            {
//                OnRecordBtnClicked?.Invoke(OnRecordButtonResponse);
//            });

//            m_TriggerRegion.onPointerEnter.AddListener(ShowPanel);
//            m_TriggerRegion.onPointerOut.AddListener(HidePanel);

//            HidePanel();
//        }

//        private bool m_IsRecordButtonActive = false;
//        private void OnRecordButtonResponse(bool result)
//        {
//            if (!result)
//            {
//                return;
//            }
//            m_IsRecordButtonActive = !m_IsRecordButtonActive;
//            m_RecordBtn.GetComponent<Image>().color = m_IsRecordButtonActive ? ActiveColor : NormalColor;
//            m_StreamBtn.gameObject.SetActive(!m_IsRecordButtonActive);
//            HidePanel();
//        }

//        private bool m_IsStreamButtonActive = false;
//        private void OnStreamButtonResponse(bool result)
//        {
//            if (!result)
//            {
//                return;
//            }
//            m_IsStreamButtonActive = !m_IsStreamButtonActive;
//            m_StreamBtn.GetComponent<Image>().color = m_IsStreamButtonActive ? ActiveColor : NormalColor;
//            m_RecordBtn.gameObject.SetActive(!m_IsStreamButtonActive);
//            HidePanel();
//        }

//        /// <summary> Shows the panel. </summary>
//        private void ShowPanel()
//        {
//            m_PanelRoot.gameObject.SetActive(true);
//        }

//        /// <summary> Hides the panel. </summary>
//        private void HidePanel()
//        {
//            m_PanelRoot.gameObject.SetActive(false);
//        }
//    }
//}

namespace NRKernal.Experimental.StreammingCast
{
    using UnityEngine;

    public class FPSConfigView : MonoBehaviour
    {
        public delegate void OnButtonClick(FirstPersonStreammingCast.OnResponse response);
        public event OnButtonClick OnStreamBtnClicked;
        public Color ActiveColor;

        private bool m_IsStreamButtonActive = false;

        void Start()
        {
            ActivateStreamState(); // Start 메서드에서 바로 stream 상태 활성화
        }

        private void OnStreamButtonResponse(bool result)
        {
            if (!result)
            {
                return;
            }
            m_IsStreamButtonActive = true;
            // 이 메서드에서는 버튼이나 UI 요소 없이 필요한 스트림 활성화 로직만 처리
            // 필요한 경우, ActiveColor를 통해 상태를 시각적으로 나타낼 수 있습니다.
        }

        private void ActivateStreamState()
        {
            // 스트림 상태를 바로 활성화
            m_IsStreamButtonActive = true;
            OnStreamButtonResponse(true);
        }
    }
}
