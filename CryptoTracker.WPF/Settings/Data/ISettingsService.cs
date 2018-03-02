using CryptoTracker.WPF.Settings.Data.ToggleableSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.WPF.Settings.Data
{
    public interface ISettingsService
    {
        void LoadSettings();
        void ActivateSettings();
        void ToggleSetting(ToggleableSettingModel setting);
        void EditSetting(SettingModel setting);
        bool SaveChanges();

    }
}
