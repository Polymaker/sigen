﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Localization
{
    [Designer(typeof(LocalizableStringListDesigner))]
    public partial class LocalizableStringList : Component
    {
        private List<LocalizableString> _Items;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<LocalizableString> Items
        {
            get { return _Items; }
        }

        public LocalizableStringList()
        {
            _Items = new List<LocalizableString>();
            InitializeComponent();
        }

        public LocalizableStringList(IContainer container)
        {
            _Items = new List<LocalizableString>();
            container.Add(this);

            InitializeComponent();
        }
    }

    internal class LocalizableStringListDesigner : ComponentDesigner
    {
        private DesignerActionListCollection _ActionList;
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new DesignerActionListCollection();
                    _ActionList.Add(new LocalizableStringsActions(this));
                }
                return _ActionList;
            }
        }
    }

    internal class LocalizableStringsActions : DesignerActionList
    {
        private ComponentDesigner _Designer;

        public LocalizableStringsActions(ComponentDesigner designer)
            : base(designer.Component)
        {
            _Designer = designer;
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection
            {
                new DesignerActionMethodItem(this, "InvokeItemsDialog", "Edit strings...", "Properties", "Edit localizable strings.", true),
            };
        }

        public void InvokeItemsDialog()
        {
            ShowCollectionEditor(_Designer, base.Component, "Items");
        }

        private static MethodInfo EditValueMI;

        private static void ShowCollectionEditor(ComponentDesigner designer, object objectToChange, string propName)
        {
            if (EditValueMI == null)
            {
                var systemDesignAssem = Assembly.GetAssembly(typeof(ComponentDesigner));
                var editorServiceType = systemDesignAssem.GetType("System.Windows.Forms.Design.EditorServiceContext");
                EditValueMI = editorServiceType.GetMethod("EditValue", BindingFlags.Static | BindingFlags.Public);
            }
            if (EditValueMI != null)
            {
                EditValueMI.Invoke(null, new object[] { designer, objectToChange, propName });
            }
        }
    }
}
