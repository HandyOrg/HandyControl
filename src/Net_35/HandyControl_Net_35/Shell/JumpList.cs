namespace Microsoft.Windows.Shell
{
    using Standard;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Markup;

    [ContentProperty("JumpItems")]
    public sealed class JumpList : ISupportInitialize
    {
        private Application _application;
        private static readonly string _FullName = Standard.NativeMethods.GetModuleFileName(IntPtr.Zero);
        private bool? _initializing;
        private List<JumpItem> _jumpItems;
        private static readonly Dictionary<Application, JumpList> s_applicationMap = new Dictionary<Application, JumpList>();
        private static readonly object s_lock = new object();

        public event EventHandler<JumpItemsRejectedEventArgs> JumpItemsRejected;

        public event EventHandler<JumpItemsRemovedEventArgs> JumpItemsRemovedByUser;

        public JumpList() : this(null, false, false)
        {
            this._initializing = null;
        }

        public JumpList(IEnumerable<JumpItem> items, bool showFrequent, bool showRecent)
        {
            if (items != null)
            {
                this._jumpItems = new List<JumpItem>(items);
            }
            else
            {
                this._jumpItems = new List<JumpItem>();
            }
            this.ShowFrequentCategory = showFrequent;
            this.ShowRecentCategory = showRecent;
            this._initializing = false;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId="Standard.Verify.IsApartmentState(System.Threading.ApartmentState,System.String)"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="JumpLists")]
        private void _ApplyList()
        {
            Standard.Verify.IsApartmentState(ApartmentState.STA, "JumpLists can only be effected on STA threads.");
            if (!Standard.Utility.IsOSWindows7OrNewer)
            {
                this.RejectEverything();
            }
            else
            {
                List<JumpItem> list;
                List<_RejectedJumpItemPair> list2;
                List<_ShellObjectPair> list3;
                try
                {
                    this._BuildShellLists(out list, out list2, out list3);
                }
                catch (Exception)
                {
                    this.RejectEverything();
                    return;
                }
                this._jumpItems = list;
                EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
                EventHandler<JumpItemsRemovedEventArgs> jumpItemsRemovedByUser = this.JumpItemsRemovedByUser;
                if ((list2.Count > 0) && (jumpItemsRejected != null))
                {
                    List<JumpItem> rejectedItems = new List<JumpItem>(list2.Count);
                    List<JumpItemRejectionReason> reasons = new List<JumpItemRejectionReason>(list2.Count);
                    foreach (_RejectedJumpItemPair pair in list2)
                    {
                        rejectedItems.Add(pair.JumpItem);
                        reasons.Add(pair.Reason);
                    }
                    jumpItemsRejected(this, new JumpItemsRejectedEventArgs(rejectedItems, reasons));
                }
                if ((list3.Count > 0) && (jumpItemsRemovedByUser != null))
                {
                    List<JumpItem> removedItems = new List<JumpItem>(list3.Count);
                    foreach (_ShellObjectPair pair2 in list3)
                    {
                        if (pair2.JumpItem != null)
                        {
                            removedItems.Add(pair2.JumpItem);
                        }
                    }
                    if (removedItems.Count > 0)
                    {
                        jumpItemsRemovedByUser(this, new JumpItemsRemovedEventArgs(removedItems));
                    }
                }
            }
        }

        private void _BuildShellLists(out List<JumpItem> successList, out List<_RejectedJumpItemPair> rejectedList, out List<_ShellObjectPair> removedList)
        {
            List<List<_ShellObjectPair>> list = null;
            removedList = null;
            Standard.ICustomDestinationList cdl = Standard.CLSID.CoCreateInstance<Standard.ICustomDestinationList>("77f10cf0-3db5-4966-b520-b7c54fd35ed6");
            try
            {
                uint num;
                string str = _RuntimeId;
                if (!string.IsNullOrEmpty(str))
                {
                    cdl.SetAppID(str);
                }
                Guid riid = new Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9");
                Standard.IObjectArray shellObjects = (Standard.IObjectArray) cdl.BeginList(out num, ref riid);
                removedList = GenerateJumpItems(shellObjects);
                successList = new List<JumpItem>(this.JumpItems.Count);
                rejectedList = new List<_RejectedJumpItemPair>(this.JumpItems.Count);
                list = new List<List<_ShellObjectPair>> {
                    new List<_ShellObjectPair>()
                };
                foreach (JumpItem item in this.JumpItems)
                {
                    if (item == null)
                    {
                        _RejectedJumpItemPair pair = new _RejectedJumpItemPair {
                            JumpItem = item,
                            Reason = JumpItemRejectionReason.InvalidItem
                        };
                        rejectedList.Add(pair);
                    }
                    else
                    {
                        object shellObject = null;
                        try
                        {
                            shellObject = GetShellObjectForJumpItem(item);
                            if (shellObject == null)
                            {
                                _RejectedJumpItemPair pair2 = new _RejectedJumpItemPair {
                                    Reason = JumpItemRejectionReason.InvalidItem,
                                    JumpItem = item
                                };
                                rejectedList.Add(pair2);
                            }
                            else if (ListContainsShellObject(removedList, shellObject))
                            {
                                _RejectedJumpItemPair pair3 = new _RejectedJumpItemPair {
                                    Reason = JumpItemRejectionReason.RemovedByUser,
                                    JumpItem = item
                                };
                                rejectedList.Add(pair3);
                            }
                            else
                            {
                                _ShellObjectPair pair4 = new _ShellObjectPair {
                                    JumpItem = item,
                                    ShellObject = shellObject
                                };
                                if (string.IsNullOrEmpty(item.CustomCategory))
                                {
                                    list[0].Add(pair4);
                                }
                                else
                                {
                                    bool flag = false;
                                    foreach (List<_ShellObjectPair> list3 in list)
                                    {
                                        if ((list3.Count > 0) && (list3[0].JumpItem.CustomCategory == item.CustomCategory))
                                        {
                                            list3.Add(pair4);
                                            flag = true;
                                            break;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        list.Add(new List<_ShellObjectPair> { pair4 });
                                    }
                                }
                                shellObject = null;
                            }
                        }
                        finally
                        {
                            Standard.Utility.SafeRelease<object>(ref shellObject);
                        }
                    }
                }
                list.Reverse();
                if (this.ShowFrequentCategory)
                {
                    cdl.AppendKnownCategory(Standard.KDC.FREQUENT);
                }
                if (this.ShowRecentCategory)
                {
                    cdl.AppendKnownCategory(Standard.KDC.RECENT);
                }
                foreach (List<_ShellObjectPair> list5 in list)
                {
                    if (list5.Count > 0)
                    {
                        string customCategory = list5[0].JumpItem.CustomCategory;
                        AddCategory(cdl, customCategory, list5, successList, rejectedList);
                    }
                }
                cdl.CommitList();
                successList.Reverse();
            }
            finally
            {
                Standard.Utility.SafeRelease<Standard.ICustomDestinationList>(ref cdl);
                if (list != null)
                {
                    foreach (List<_ShellObjectPair> list7 in list)
                    {
                        _ShellObjectPair.ReleaseShellObjects(list7);
                    }
                }
                _ShellObjectPair.ReleaseShellObjects(removedList);
            }
        }

        private static void AddCategory(Standard.ICustomDestinationList cdl, string category, List<_ShellObjectPair> jumpItems, List<JumpItem> successList, List<_RejectedJumpItemPair> rejectionList)
        {
            AddCategory(cdl, category, jumpItems, successList, rejectionList, true);
        }

        private static void AddCategory(Standard.ICustomDestinationList cdl, string category, List<_ShellObjectPair> jumpItems, List<JumpItem> successList, List<_RejectedJumpItemPair> rejectionList, bool isHeterogenous)
        {
            Standard.HRESULT hresult;
            Standard.IObjectCollection poa = (Standard.IObjectCollection) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("2d3468c1-36a7-43b6-ac24-d3f02fd9607a")));
            foreach (_ShellObjectPair pair in jumpItems)
            {
                poa.AddObject(pair.ShellObject);
            }
            if (string.IsNullOrEmpty(category))
            {
                hresult = cdl.AddUserTasks(poa);
            }
            else
            {
                hresult = cdl.AppendCategory(category, poa);
            }
            if (hresult.Succeeded)
            {
                int count = jumpItems.Count;
                while (--count >= 0)
                {
                    successList.Add(jumpItems[count].JumpItem);
                }
            }
            else if (isHeterogenous && (hresult == Standard.HRESULT.DESTS_E_NO_MATCHING_ASSOC_HANDLER))
            {
                Standard.Utility.SafeRelease<Standard.IObjectCollection>(ref poa);
                List<_ShellObjectPair> list = new List<_ShellObjectPair>();
                foreach (_ShellObjectPair pair2 in jumpItems)
                {
                    if (pair2.JumpItem is JumpPath)
                    {
                        _RejectedJumpItemPair item = new _RejectedJumpItemPair {
                            JumpItem = pair2.JumpItem,
                            Reason = JumpItemRejectionReason.NoRegisteredHandler
                        };
                        rejectionList.Add(item);
                    }
                    else
                    {
                        list.Add(pair2);
                    }
                }
                if (list.Count > 0)
                {
                    AddCategory(cdl, category, list, successList, rejectionList, false);
                }
            }
            else
            {
                foreach (_ShellObjectPair pair4 in jumpItems)
                {
                    _RejectedJumpItemPair pair5 = new _RejectedJumpItemPair {
                        JumpItem = pair4.JumpItem,
                        Reason = JumpItemRejectionReason.InvalidItem
                    };
                    rejectionList.Add(pair5);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId="0")]
        public static void AddToRecentCategory(JumpPath jumpPath)
        {
            Standard.Verify.IsNotNull<JumpPath>(jumpPath, "jumpPath");
            AddToRecentCategory(jumpPath.Path);
        }

        public static void AddToRecentCategory(JumpTask jumpTask)
        {
            Standard.Verify.IsNotNull<JumpTask>(jumpTask, "jumpTask");
            if (Standard.Utility.IsOSWindows7OrNewer)
            {
                Standard.IShellLinkW shellLink = CreateLinkFromJumpTask(jumpTask, false);
                try
                {
                    if (shellLink != null)
                    {
                        Standard.NativeMethods.SHAddToRecentDocs(shellLink);
                    }
                }
                finally
                {
                    Standard.Utility.SafeRelease<Standard.IShellLinkW>(ref shellLink);
                }
            }
        }

        public static void AddToRecentCategory(string itemPath)
        {
            Standard.Verify.FileExists(itemPath, "itemPath");
            itemPath = Path.GetFullPath(itemPath);
            Standard.NativeMethods.SHAddToRecentDocs(itemPath);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="JumpList"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="EndInit")]
        public void Apply()
        {
            if (this._initializing == true)
            {
                throw new InvalidOperationException("The JumpList can't be applied until EndInit has been called.");
            }
            this._initializing = false;
            this._ApplyList();
        }

        private void ApplyFromApplication()
        {
            if ((this._initializing != true) && !this._IsUnmodified)
            {
                this._initializing = false;
            }
            if ((this._application == Application.Current) && (this._initializing == false))
            {
                this._ApplyList();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="BeginInit")]
        public void BeginInit()
        {
            if (!this._IsUnmodified)
            {
                throw new InvalidOperationException("Calls to BeginInit cannot be nested.");
            }
            this._initializing = true;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Standard.IShellItem2 CreateItemFromJumpPath(JumpPath jumpPath)
        {
            try
            {
                return GetShellItemForPath(Path.GetFullPath(jumpPath.Path));
            }
            catch (Exception)
            {
            }
            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static Standard.IShellLinkW CreateLinkFromJumpTask(JumpTask jumpTask, bool allowSeparators)
        {
            Standard.IShellLinkW kw3;
            if (string.IsNullOrEmpty(jumpTask.Title) && (!allowSeparators || !string.IsNullOrEmpty(jumpTask.CustomCategory)))
            {
                return null;
            }
            Standard.IShellLinkW comObject = (Standard.IShellLinkW) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046")));
            try
            {
                string pszFile = _FullName;
                if (!string.IsNullOrEmpty(jumpTask.ApplicationPath))
                {
                    pszFile = jumpTask.ApplicationPath;
                }
                comObject.SetPath(pszFile);
                if (!string.IsNullOrEmpty(jumpTask.WorkingDirectory))
                {
                    comObject.SetWorkingDirectory(jumpTask.WorkingDirectory);
                }
                if (!string.IsNullOrEmpty(jumpTask.Arguments))
                {
                    comObject.SetArguments(jumpTask.Arguments);
                }
                if (jumpTask.IconResourceIndex != -1)
                {
                    string pszIconPath = _FullName;
                    if (!string.IsNullOrEmpty(jumpTask.IconResourcePath))
                    {
                        if (jumpTask.IconResourcePath.Length >= 260L)
                        {
                            return null;
                        }
                        pszIconPath = jumpTask.IconResourcePath;
                    }
                    comObject.SetIconLocation(pszIconPath, jumpTask.IconResourceIndex);
                }
                if (!string.IsNullOrEmpty(jumpTask.Description))
                {
                    comObject.SetDescription(jumpTask.Description);
                }
                Standard.IPropertyStore store = (Standard.IPropertyStore) comObject;
                using (PROPVARIANT propvariant = new PROPVARIANT())
                {
                    Standard.PKEY title = new Standard.PKEY();
                    if (!string.IsNullOrEmpty(jumpTask.Title))
                    {
                        propvariant.SetValue(jumpTask.Title);
                        title = Standard.PKEY.Title;
                    }
                    else
                    {
                        propvariant.SetValue(true);
                        title = Standard.PKEY.AppUserModel_IsDestListSeparator;
                    }
                    store.SetValue(ref title, propvariant);
                }
                store.Commit();
                Standard.IShellLinkW kw2 = comObject;
                comObject = null;
                kw3 = kw2;
            }
            catch (Exception)
            {
                kw3 = null;
            }
            finally
            {
                Standard.Utility.SafeRelease<Standard.IShellLinkW>(ref comObject);
            }
            return kw3;
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="EndInit"), SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId="BeginInit")]
        public void EndInit()
        {
            if (this._initializing != true)
            {
                throw new NotSupportedException("Can't call EndInit without first calling BeginInit.");
            }
            this._initializing = false;
            this.ApplyFromApplication();
        }

        private static List<_ShellObjectPair> GenerateJumpItems(Standard.IObjectArray shellObjects)
        {
            List<_ShellObjectPair> list = new List<_ShellObjectPair>();
            Guid riid = new Guid("00000000-0000-0000-C000-000000000046");
            uint count = shellObjects.GetCount();
            for (uint i = 0; i < count; i++)
            {
                object at = shellObjects.GetAt(i, ref riid);
                JumpItem jumpItemForShellObject = null;
                try
                {
                    jumpItemForShellObject = GetJumpItemForShellObject(at);
                }
                catch (Exception exception)
                {
                    if ((exception is NullReferenceException) || (exception is SEHException))
                    {
                        throw;
                    }
                }
                _ShellObjectPair pair = new _ShellObjectPair {
                    ShellObject = at,
                    JumpItem = jumpItemForShellObject
                };
                list.Add(pair);
            }
            return list;
        }

        private static JumpItem GetJumpItemForShellObject(object shellObject)
        {
            int num;
            Standard.IShellItem2 item = shellObject as Standard.IShellItem2;
            Standard.IShellLinkW kw = shellObject as Standard.IShellLinkW;
            if (item != null)
            {
                return new JumpPath { Path = item.GetDisplayName(unchecked((Standard.SIGDN) (-2147319808))) };
            }
            if (kw == null)
            {
                return null;
            }
            StringBuilder pszFile = new StringBuilder(260);
            kw.GetPath(pszFile, pszFile.Capacity, null, Standard.SLGP.RAWPATH);
            StringBuilder pszArgs = new StringBuilder(0x400);
            kw.GetArguments(pszArgs, pszArgs.Capacity);
            StringBuilder builder3 = new StringBuilder(0x400);
            kw.GetDescription(builder3, builder3.Capacity);
            StringBuilder pszIconPath = new StringBuilder(260);
            kw.GetIconLocation(pszIconPath, pszIconPath.Capacity, out num);
            StringBuilder pszDir = new StringBuilder(260);
            kw.GetWorkingDirectory(pszDir, pszDir.Capacity);
            JumpTask task = new JumpTask {
                ApplicationPath = pszFile.ToString(),
                Arguments = pszArgs.ToString(),
                Description = builder3.ToString(),
                IconResourceIndex = num,
                IconResourcePath = pszIconPath.ToString(),
                WorkingDirectory = pszDir.ToString()
            };
            using (PROPVARIANT propvariant = new PROPVARIANT())
            {
                Standard.IPropertyStore store = (Standard.IPropertyStore) kw;
                Standard.PKEY title = Standard.PKEY.Title;
                store.GetValue(ref title, propvariant);
                task.Title = propvariant.GetValue() ?? "";
            }
            return task;
        }

        public static JumpList GetJumpList(Application application)
        {
            JumpList list;
            Standard.Verify.IsNotNull<Application>(application, "application");
            s_applicationMap.TryGetValue(application, out list);
            return list;
        }

        private static Standard.IShellItem2 GetShellItemForPath(string path)
        {
            object obj2;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            Guid riid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
            Standard.HRESULT hresult = Standard.NativeMethods.SHCreateItemFromParsingName(path, null, ref riid, out obj2);
            if ((hresult == ((Standard.HRESULT) Standard.Win32Error.ERROR_FILE_NOT_FOUND)) || (hresult == ((Standard.HRESULT) Standard.Win32Error.ERROR_PATH_NOT_FOUND)))
            {
                hresult = Standard.HRESULT.S_OK;
                obj2 = null;
            }
            hresult.ThrowIfFailed();
            return (Standard.IShellItem2) obj2;
        }

        private static object GetShellObjectForJumpItem(JumpItem jumpItem)
        {
            JumpPath jumpPath = jumpItem as JumpPath;
            JumpTask jumpTask = jumpItem as JumpTask;
            if (jumpPath != null)
            {
                return CreateItemFromJumpPath(jumpPath);
            }
            if (jumpTask != null)
            {
                return CreateLinkFromJumpTask(jumpTask, true);
            }
            return null;
        }

        private static bool ListContainsShellObject(List<_ShellObjectPair> removedList, object shellObject)
        {
            if (removedList.Count != 0)
            {
                Standard.IShellItem item = shellObject as Standard.IShellItem;
                if (item != null)
                {
                    foreach (_ShellObjectPair pair in removedList)
                    {
                        Standard.IShellItem psi = pair.ShellObject as Standard.IShellItem;
                        if ((psi != null) && (item.Compare(psi, Standard.SICHINT.CANONICAL | Standard.SICHINT.TEST_FILESYSPATH_IF_NOT_EQUAL) == 0))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                Standard.IShellLinkW shellLink = shellObject as Standard.IShellLinkW;
                if (shellLink != null)
                {
                    foreach (_ShellObjectPair pair2 in removedList)
                    {
                        Standard.IShellLinkW kw2 = pair2.ShellObject as Standard.IShellLinkW;
                        if (kw2 != null)
                        {
                            string str = ShellLinkToString(kw2);
                            string str2 = ShellLinkToString(shellLink);
                            if (str == str2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void RejectEverything()
        {
            EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
            if (jumpItemsRejected == null)
            {
                this._jumpItems.Clear();
            }
            else if (this._jumpItems.Count > 0)
            {
                List<JumpItemRejectionReason> reasons = new List<JumpItemRejectionReason>(this.JumpItems.Count);
                for (int i = 0; i < this.JumpItems.Count; i++)
                {
                    reasons.Add(JumpItemRejectionReason.InvalidItem);
                }
                JumpItemsRejectedEventArgs e = new JumpItemsRejectedEventArgs(this.JumpItems, reasons);
                this._jumpItems.Clear();
                jumpItemsRejected(this, e);
            }
        }

        public static void SetJumpList(Application application, JumpList value)
        {
            Standard.Verify.IsNotNull<Application>(application, "application");
            lock (s_lock)
            {
                JumpList list;
                if (s_applicationMap.TryGetValue(application, out list) && (list != null))
                {
                    list._application = null;
                }
                s_applicationMap[application] = value;
                if (value != null)
                {
                    value._application = application;
                }
            }
            if (value != null)
            {
                value.ApplyFromApplication();
            }
        }

        private static string ShellLinkToString(Standard.IShellLinkW shellLink)
        {
            StringBuilder pszFile = new StringBuilder(260);
            shellLink.GetPath(pszFile, pszFile.Capacity, null, Standard.SLGP.RAWPATH);
            string str = null;
            using (PROPVARIANT propvariant = new PROPVARIANT())
            {
                Standard.IPropertyStore store = (Standard.IPropertyStore) shellLink;
                Standard.PKEY title = Standard.PKEY.Title;
                store.GetValue(ref title, propvariant);
                str = propvariant.GetValue() ?? "";
            }
            StringBuilder pszArgs = new StringBuilder(0x400);
            shellLink.GetArguments(pszArgs, pszArgs.Capacity);
            return (pszFile.ToString().ToUpperInvariant() + str.ToUpperInvariant() + pszArgs.ToString());
        }

        private bool _IsUnmodified
        {
            get
            {
                return (((!this._initializing.HasValue && (this.JumpItems.Count == 0)) && !this.ShowRecentCategory) && !this.ShowFrequentCategory);
            }
        }

        private static string _RuntimeId
        {
            get
            {
                string str;
                Standard.HRESULT currentProcessExplicitAppUserModelID = Standard.NativeMethods.GetCurrentProcessExplicitAppUserModelID(out str);
                if (currentProcessExplicitAppUserModelID == Standard.HRESULT.E_FAIL)
                {
                    currentProcessExplicitAppUserModelID = Standard.HRESULT.S_OK;
                    str = null;
                }
                currentProcessExplicitAppUserModelID.ThrowIfFailed();
                return str;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<JumpItem> JumpItems
        {
            get
            {
                return this._jumpItems;
            }
        }

        public bool ShowFrequentCategory { get; set; }

        public bool ShowRecentCategory { get; set; }

        private class _RejectedJumpItemPair
        {
            public Microsoft.Windows.Shell.JumpItem JumpItem { get; set; }

            public JumpItemRejectionReason Reason { get; set; }
        }

        private class _ShellObjectPair
        {
            public static void ReleaseShellObjects(List<JumpList._ShellObjectPair> list)
            {
                if (list != null)
                {
                    foreach (JumpList._ShellObjectPair pair in list)
                    {
                        object shellObject = pair.ShellObject;
                        pair.ShellObject = null;
                        Standard.Utility.SafeRelease<object>(ref shellObject);
                    }
                }
            }

            public Microsoft.Windows.Shell.JumpItem JumpItem { get; set; }

            public object ShellObject { get; set; }
        }
    }
}

