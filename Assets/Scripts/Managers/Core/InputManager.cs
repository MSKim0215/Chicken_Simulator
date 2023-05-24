using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Unit")))
            {
                UI_UnitInfo info = Managers.UI.ShowPopupUI<UI_UnitInfo>();
                info.SetTarget(hit.collider.GetComponent<Brain>());
            }
            else
            {
                Managers.UI.ClosePopupUI();
            }
        }
    }
}