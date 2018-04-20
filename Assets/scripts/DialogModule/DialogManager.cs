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
    private List<ModalDialog> modalsDialogList = new List<ModalDialog>();

    public void AddDialog(ModalDialog dialog)
    {
        bool isFound = modalsDialogList.Contains(dialog);
        if (!isFound)
            modalsDialogList.Add(dialog);

        CloseAllOpenedModalDialogs();
    }

    public void ShowModalDialog(GameObject modalPanel, GameObject parentModalPanel)
    {
        var foundModalPanel = modalsDialogList.First(dialog => dialog.ModalPanel == modalPanel && dialog.ParentModalPanel == parentModalPanel);
        if (foundModalPanel == null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] does not found!", modalPanel));
            return;
        }

        CloseAllOpenedModalDialogs();

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

    public void CloseAllOpenedModalDialogs()
    {
        foreach (var dialog in modalsDialogList)
            dialog.ModalPanel.SetActive(false);
    }

    public void UpdateDialogTitle(ModalDialog dialog, string titleText)
    {
        bool isFound = modalsDialogList.Contains(dialog);
        if (!isFound)
            dialog.Title.text = titleText;
    }
}
