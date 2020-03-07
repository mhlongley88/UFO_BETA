using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonSetter : MonoBehaviour
{
    public MenuDynamicController controller;

    IEnumerator Start()
    {
        for (int i = 0; i < 4; i++) yield return null;

        Button[] buttons = GetComponentsInChildren<Button>();
        controller.items = new MenuDynamicController.MenuItem[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            controller.items[i] = new MenuDynamicController.MenuItem();
            controller.items[i].selectable = buttons[i];
            controller.items[i].type = MenuDynamicController.SelectableType.BUTTON;

            controller.items[i].transform = controller.items[i].selectable.gameObject.transform;
            controller.items[i].originalScale = controller.items[i].transform.localScale;
        }
    }
}
