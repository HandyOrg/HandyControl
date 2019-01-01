using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyControl.Controls
{
    internal static partial class VisualStates
    {
        /// <summary>
        /// Busyness group name.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";

        /// <summary>
        /// Busy state for BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// Idle state for BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// BusyDisplay group.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";

        /// <summary>
        /// Visible state name for BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Hidden state name for BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
    }
}
