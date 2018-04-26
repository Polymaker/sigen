using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Localization
{
    [ToolboxItem(false), DesignTimeVisible(false), Serializable]
    public partial class LocalizableString : Component
    {
        [Localizable(true), Category("Design")]
        public string Text { get; set; }

        public LocalizableString()
        {
            InitializeComponent();
        }

        public LocalizableString(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public static implicit operator string(LocalizableString message)
        {
            return message.Text;
        }
    }
}
