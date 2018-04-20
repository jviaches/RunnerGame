using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.scripts.DialogModule
{
    public class ModalDialog
    {
        private string _title;
        private GameObject _modalPanel;
        private Dictionary<Button, UnityAction> _buttonsAndMethods;
        private GameObject _parentModalPanel;

        public ModalDialog(string title, GameObject modalPanel, Dictionary<Button, UnityAction> buttonsAndMethods, GameObject parentModalPanel)
        {
            _title = title;
            _modalPanel = modalPanel;
            _buttonsAndMethods = buttonsAndMethods;
            _parentModalPanel = parentModalPanel;

            for (int i = 0; i < buttonsAndMethods.Count; i++)
                buttonsAndMethods.ElementAt(i).Key.onClick.AddListener(buttonsAndMethods.ElementAt(i).Value);
        }

        public string Title { get { return _title; } }
        public GameObject ModalPanel { get { return _modalPanel; } }
        public GameObject ParentModalPanel { get { return _parentModalPanel; } }
    }
}
