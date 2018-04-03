﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class StringsConfigurationEditor : LayoutPropertyEditor
    {
        public StringsConfigurationEditor()
        {
            InitializeComponent();
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            nbxNumberOfStrings.Enabled = (CurrentLayout != null);
            chkLeftHanded.Enabled = (CurrentLayout != null);

            if (CurrentLayout != null)
            {
                nbxNumberOfStrings.Value = CurrentLayout.NumberOfStrings;
                chkLeftHanded.Checked = CurrentLayout.LeftHanded;
            }
            else
            {
                nbxNumberOfStrings.Value = 6;
                chkLeftHanded.Checked = false;
            }
        }

        private void nbxNumberOfStrings_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.NumberOfStrings = (int)nbxNumberOfStrings.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void chkLeftHanded_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.LeftHanded = chkLeftHanded.Checked;
                CurrentLayout.RebuildLayout();
            }
        }
    }
}
