using System;
using HandyControl.Data.Enum;

namespace HandyControl.Data
{
    public class GrowlInfo
    {
        public string Message { get; set; }

        public bool ShowDateTime { get; set; } = true;

        public string CancelStr { get; set; } = Properties.Langs.Lang.Cancel;

        public string ConfirmStr { get; set; } = Properties.Langs.Lang.Confirm;

        public Func<bool, bool> ActionBeforeClose { get; set; }

        internal InfoType Type { get; set; }

        internal string IconKey { get; set; }

        internal string IconBrushKey { get; set; }

        internal bool StaysOpen { get; set; }

        internal bool ShowCloseButton { get; set; } = true;
    }
}