using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.WPF.Settings.Data.ToggleableSettings
{
    public abstract class ToggleableSettingModel
    {

        protected string Name { get; set; }
        protected ToggleType Type { get; set; }

        protected abstract void Activate();



    }
}
