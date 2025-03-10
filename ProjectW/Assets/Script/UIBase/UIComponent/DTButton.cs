using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.UI
{
    public class DTButton : Button
    {
        private event Action LongClickEvent;
        private event Action LongPressEvent;
        private event Action HoverStartEvent;
        private event Action HoverEndEvent;
        
        private double _onclickStartTime;
        
        private float minLongClickTime = 10f;
        public string touchSoundKey;
        
        private Coroutine _coCheckLongClick;
        private Coroutine CoCheckLongClick
        {
            get => _coCheckLongClick;
            set
            {
                if (_coCheckLongClick != null)
                {
                    StopCoroutine(_coCheckLongClick);
                    _coCheckLongClick = null;
                }
                _coCheckLongClick = value;
            }
        }
        
        private bool _isLongClick;
        private bool _isHovering = false;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            CoCheckLongClick = StartCoroutine(CheckLongPress());
            _isLongClick = false;
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_isLongClick)
            {
                OnLongClick();
            }
            else
            {
                base.OnPointerClick(eventData);
            }

            CoCheckLongClick = null;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            CoCheckLongClick = null;
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (_isHovering == false)
            {
                base.OnPointerEnter(eventData);
                OnHoverEventStart();
                _isHovering = true;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (_isHovering)
            {
                base.OnPointerExit(eventData);
                OnHoverEventEnd();
            }

            _isHovering = false;
        }
        
        public void SetLongClickEvent(Action longClickEvent)
        {
            LongClickEvent = longClickEvent;
        }        
        public void SetLongPressEvent(Action longPressEvent)
        {
            LongPressEvent = longPressEvent;
        }

        public void SetHoverEvent(Action startAction, Action endAction )
        {
            HoverStartEvent = startAction;
            HoverEndEvent = endAction;
        }
        IEnumerator CheckLongPress()
        {
            yield return new WaitForSeconds(minLongClickTime);
            _isLongClick = true;
            PressLong();
        }

        private void PressLong()
        {
            LongPressEvent?.Invoke();
  //          Debug.Log("long Press");
        }

        private void OnLongClick()
        {
            LongClickEvent?.Invoke();
            _isLongClick = false;
//            Debug.Log("long Click");
        }

        private void OnHoverEventStart()
        {
            HoverStartEvent?.Invoke();
        }
        private void OnHoverEventEnd()
        {
            HoverEndEvent?.Invoke();
        }
    }
}
