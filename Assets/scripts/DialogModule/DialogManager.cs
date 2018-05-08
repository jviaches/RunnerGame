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

    public List<ModalDialog> ModalsDialogList
    {
        get
        {
            return modalsDialogList;
        }
    }

    public void AddDialog(ModalDialog dialog)
    {
        bool isFound = ModalsDialogList.Contains(dialog);
        if (!isFound)
            ModalsDialogList.Add(dialog);
    }

    public void ShowModalDialog(ModalDialog modalPanel)
    {
        var foundModalPanel = ModalsDialogList.First(dialog => dialog == modalPanel);
        if (foundModalPanel == null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] does not found!", modalPanel));
            return;
        }

        CloseAllOpenedModalDialogs();

        foundModalPanel.ModalPanel.transform.position = modalPanel.ParentModalPanel.transform.position;
        foundModalPanel.ModalPanel.SetActive(true);
    }

    public void HideModalDialog(GameObject modalPanel)
    {
        var foundModalPanel = ModalsDialogList.First(dialog => dialog.ModalPanel == modalPanel);
        if (foundModalPanel == null)
        {
            Debug.Log(String.Format("Error: Requested modalpanel [{0}] does not found!", modalPanel));
            return;
        }

        foundModalPanel.ModalPanel.SetActive(false);
    }

    public void CloseAllOpenedModalDialogs()
    {
        foreach (var dialog in ModalsDialogList)
            dialog.ModalPanel.SetActive(false);
    }
}
