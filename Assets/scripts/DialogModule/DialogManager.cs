using Assets.scripts.DialogModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager
{
    List<ModalDialog> modalsDialogList = new List<ModalDialog>();

    public void AddDialog(string title, GameObject modalPanel, Dictionary<Button,UnityAction> buttonsAndMethods, GameObject parentModalPanel)
    {
        var foundModalPanel = modalsDialogList.First(dialog => dialog.ModalPanel == modalPanel && dialog.ParentModalPanel == parentModalPanel);
        if (foundModalPanel != null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] is already exist!", modalPanel));
            return;
        }

        modalsDialogList.Add(new ModalDialog(title, modalPanel, buttonsAndMethods, parentModalPanel));
    }

    public void ShowModalDialog(GameObject modalPanel, GameObject parentModalPanel)
    {
        var foundModalPanel = modalsDialogList.First(dialog => dialog.ModalPanel == modalPanel && dialog.ParentModalPanel == parentModalPanel);
        if (foundModalPanel == null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] does not found!", modalPanel));
            return;
        }

        closeAllOpenedModalDialogs();

        foundModalPanel.ModalPanel.transform.position = parentModalPanel.transform.position;
        foundModalPanel.ModalPanel.SetActive(true);
    }

    public void HideModalDialog(GameObject modalPanel, GameObject parentModalPanel)
    {
        var foundModalPanel = modalsDialogList.First(dialog => dialog.ModalPanel == modalPanel && dialog.ParentModalPanel == parentModalPanel);
        if (foundModalPanel != null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] does not found!", modalPanel));
            return;
        }

        foundModalPanel.ModalPanel.SetActive(false);
    }

    private void closeAllOpenedModalDialogs()
    {
        foreach (var dialog in modalsDialogList)
            dialog.ModalPanel.SetActive(false);
    }
}
