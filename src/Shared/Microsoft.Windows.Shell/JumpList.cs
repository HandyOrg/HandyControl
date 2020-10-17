using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Standard;

namespace Microsoft.Windows.Shell
{
	// Token: 0x02000006 RID: 6
	[ContentProperty("JumpItems")]
	public sealed class JumpList : ISupportInitialize
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002186 File Offset: 0x00000386
		public static void AddToRecentCategory(string itemPath)
		{
			Verify.FileExists(itemPath, "itemPath");
			itemPath = Path.GetFullPath(itemPath);
			NativeMethods.SHAddToRecentDocs(itemPath);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021A1 File Offset: 0x000003A1
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		public static void AddToRecentCategory(JumpPath jumpPath)
		{
			Verify.IsNotNull<JumpPath>(jumpPath, "jumpPath");
			JumpList.AddToRecentCategory(jumpPath.Path);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000021BC File Offset: 0x000003BC
		public static void AddToRecentCategory(JumpTask jumpTask)
		{
			Verify.IsNotNull<JumpTask>(jumpTask, "jumpTask");
			if (Utility.IsOSWindows7OrNewer)
			{
				IShellLinkW shellLinkW = JumpList.CreateLinkFromJumpTask(jumpTask, false);
				try
				{
					if (shellLinkW != null)
					{
						NativeMethods.SHAddToRecentDocs(shellLinkW);
					}
				}
				finally
				{
					Utility.SafeRelease<IShellLinkW>(ref shellLinkW);
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002208 File Offset: 0x00000408
		public static void SetJumpList(Application application, JumpList value)
		{
			Verify.IsNotNull<Application>(application, "application");
			lock (JumpList.s_lock)
			{
				JumpList jumpList;
				if (JumpList.s_applicationMap.TryGetValue(application, out jumpList) && jumpList != null)
				{
					jumpList._application = null;
				}
				JumpList.s_applicationMap[application] = value;
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

		// Token: 0x06000013 RID: 19 RVA: 0x00002284 File Offset: 0x00000484
		public static JumpList GetJumpList(Application application)
		{
			Verify.IsNotNull<Application>(application, "application");
			JumpList result;
			JumpList.s_applicationMap.TryGetValue(application, out result);
			return result;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000022AB File Offset: 0x000004AB
		public JumpList() : this(null, false, false)
		{
			this._initializing = null;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022C2 File Offset: 0x000004C2
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
			this._initializing = new bool?(false);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002300 File Offset: 0x00000500
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002308 File Offset: 0x00000508
		public bool ShowFrequentCategory { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002311 File Offset: 0x00000511
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002319 File Offset: 0x00000519
		public bool ShowRecentCategory { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002322 File Offset: 0x00000522
		[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public List<JumpItem> JumpItems
		{
			get
			{
				return this._jumpItems;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0000232A File Offset: 0x0000052A
		private bool _IsUnmodified
		{
			get
			{
				return this._initializing == null && this.JumpItems.Count == 0 && !this.ShowRecentCategory && !this.ShowFrequentCategory;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002359 File Offset: 0x00000559
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BeginInit")]
		public void BeginInit()
		{
			if (!this._IsUnmodified)
			{
				throw new InvalidOperationException("Calls to BeginInit cannot be nested.");
			}
			this._initializing = new bool?(true);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000237C File Offset: 0x0000057C
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EndInit")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BeginInit")]
		public void EndInit()
		{
			if (this._initializing != true)
			{
				throw new NotSupportedException("Can't call EndInit without first calling BeginInit.");
			}
			this._initializing = new bool?(false);
			this.ApplyFromApplication();
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023C8 File Offset: 0x000005C8
		private static string _RuntimeId
		{
			get
			{
				string result;
				HRESULT hrLeft = NativeMethods.GetCurrentProcessExplicitAppUserModelID(out result);
				if (hrLeft == HRESULT.E_FAIL)
				{
					hrLeft = HRESULT.S_OK;
					result = null;
				}
				hrLeft.ThrowIfFailed();
				return result;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023FC File Offset: 0x000005FC
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "JumpList")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "EndInit")]
		public void Apply()
		{
			if (this._initializing == true)
			{
				throw new InvalidOperationException("The JumpList can't be applied until EndInit has been called.");
			}
			this._initializing = new bool?(false);
			this._ApplyList();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002444 File Offset: 0x00000644
		private void ApplyFromApplication()
		{
			if (this._initializing != true && !this._IsUnmodified)
			{
				this._initializing = new bool?(false);
			}
			if (this._application == Application.Current && this._initializing == false)
			{
				this._ApplyList();
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000024B4 File Offset: 0x000006B4
		[SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Standard.Verify.IsApartmentState(System.Threading.ApartmentState,System.String)")]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "JumpLists")]
		private void _ApplyList()
		{
			Verify.IsApartmentState(ApartmentState.STA, "JumpLists can only be effected on STA threads.");
			if (!Utility.IsOSWindows7OrNewer)
			{
				this.RejectEverything();
				return;
			}
			List<JumpItem> jumpItems;
			List<JumpList._RejectedJumpItemPair> list;
			List<JumpList._ShellObjectPair> list2;
			try
			{
				this._BuildShellLists(out jumpItems, out list, out list2);
			}
			catch (Exception)
			{
				this.RejectEverything();
				return;
			}
			this._jumpItems = jumpItems;
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			EventHandler<JumpItemsRemovedEventArgs> jumpItemsRemovedByUser = this.JumpItemsRemovedByUser;
			if (list.Count > 0 && jumpItemsRejected != null)
			{
				List<JumpItem> list3 = new List<JumpItem>(list.Count);
				List<JumpItemRejectionReason> list4 = new List<JumpItemRejectionReason>(list.Count);
				foreach (JumpList._RejectedJumpItemPair rejectedJumpItemPair in list)
				{
					list3.Add(rejectedJumpItemPair.JumpItem);
					list4.Add(rejectedJumpItemPair.Reason);
				}
				jumpItemsRejected(this, new JumpItemsRejectedEventArgs(list3, list4));
			}
			if (list2.Count > 0 && jumpItemsRemovedByUser != null)
			{
				List<JumpItem> list5 = new List<JumpItem>(list2.Count);
				foreach (JumpList._ShellObjectPair shellObjectPair in list2)
				{
					if (shellObjectPair.JumpItem != null)
					{
						list5.Add(shellObjectPair.JumpItem);
					}
				}
				if (list5.Count > 0)
				{
					jumpItemsRemovedByUser(this, new JumpItemsRemovedEventArgs(list5));
				}
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002628 File Offset: 0x00000828
		private void _BuildShellLists(out List<JumpItem> successList, out List<JumpList._RejectedJumpItemPair> rejectedList, out List<JumpList._ShellObjectPair> removedList)
		{
			List<List<JumpList._ShellObjectPair>> list = null;
			removedList = null;
			ICustomDestinationList customDestinationList = CLSID.CoCreateInstance<ICustomDestinationList>("77f10cf0-3db5-4966-b520-b7c54fd35ed6");
			try
			{
				string runtimeId = JumpList._RuntimeId;
				if (!string.IsNullOrEmpty(runtimeId))
				{
					customDestinationList.SetAppID(runtimeId);
				}
				Guid guid = new Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9");
				uint num;
				IObjectArray shellObjects = (IObjectArray)customDestinationList.BeginList(out num, ref guid);
				removedList = JumpList.GenerateJumpItems(shellObjects);
				successList = new List<JumpItem>(this.JumpItems.Count);
				rejectedList = new List<JumpList._RejectedJumpItemPair>(this.JumpItems.Count);
				list = new List<List<JumpList._ShellObjectPair>>
				{
					new List<JumpList._ShellObjectPair>()
				};
				foreach (JumpItem jumpItem in this.JumpItems)
				{
					if (jumpItem == null)
					{
						rejectedList.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = jumpItem,
							Reason = JumpItemRejectionReason.InvalidItem
						});
					}
					else
					{
						object obj = null;
						try
						{
							obj = JumpList.GetShellObjectForJumpItem(jumpItem);
							if (obj == null)
							{
								rejectedList.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.InvalidItem,
									JumpItem = jumpItem
								});
							}
							else if (JumpList.ListContainsShellObject(removedList, obj))
							{
								rejectedList.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.RemovedByUser,
									JumpItem = jumpItem
								});
							}
							else
							{
								JumpList._ShellObjectPair item = new JumpList._ShellObjectPair
								{
									JumpItem = jumpItem,
									ShellObject = obj
								};
								if (string.IsNullOrEmpty(jumpItem.CustomCategory))
								{
									list[0].Add(item);
								}
								else
								{
									bool flag = false;
									foreach (List<JumpList._ShellObjectPair> list2 in list)
									{
										if (list2.Count > 0 && list2[0].JumpItem.CustomCategory == jumpItem.CustomCategory)
										{
											list2.Add(item);
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										list.Add(new List<JumpList._ShellObjectPair>
										{
											item
										});
									}
								}
								obj = null;
							}
						}
						finally
						{
							Utility.SafeRelease<object>(ref obj);
						}
					}
				}
				list.Reverse();
				if (this.ShowFrequentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.FREQUENT);
				}
				if (this.ShowRecentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.RECENT);
				}
				foreach (List<JumpList._ShellObjectPair> list3 in list)
				{
					if (list3.Count > 0)
					{
						string customCategory = list3[0].JumpItem.CustomCategory;
						JumpList.AddCategory(customDestinationList, customCategory, list3, successList, rejectedList);
					}
				}
				customDestinationList.CommitList();
				successList.Reverse();
			}
			finally
			{
				Utility.SafeRelease<ICustomDestinationList>(ref customDestinationList);
				if (list != null)
				{
					foreach (List<JumpList._ShellObjectPair> list4 in list)
					{
						JumpList._ShellObjectPair.ReleaseShellObjects(list4);
					}
				}
				JumpList._ShellObjectPair.ReleaseShellObjects(removedList);
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029BC File Offset: 0x00000BBC
		private static bool ListContainsShellObject(List<JumpList._ShellObjectPair> removedList, object shellObject)
		{
			if (removedList.Count == 0)
			{
				return false;
			}
			IShellItem shellItem = shellObject as IShellItem;
			if (shellItem != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair in removedList)
				{
					IShellItem shellItem2 = shellObjectPair.ShellObject as IShellItem;
					if (shellItem2 != null && shellItem.Compare(shellItem2, (SICHINT)805306368U) == 0)
					{
						return true;
					}
				}
				return false;
			}
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellLinkW != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair2 in removedList)
				{
					IShellLinkW shellLinkW2 = shellObjectPair2.ShellObject as IShellLinkW;
					if (shellLinkW2 != null)
					{
						string a = JumpList.ShellLinkToString(shellLinkW2);
						string b = JumpList.ShellLinkToString(shellLinkW);
						if (a == b)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002ABC File Offset: 0x00000CBC
		private static object GetShellObjectForJumpItem(JumpItem jumpItem)
		{
			JumpPath jumpPath = jumpItem as JumpPath;
			JumpTask jumpTask = jumpItem as JumpTask;
			if (jumpPath != null)
			{
				return JumpList.CreateItemFromJumpPath(jumpPath);
			}
			if (jumpTask != null)
			{
				return JumpList.CreateLinkFromJumpTask(jumpTask, true);
			}
			return null;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private static List<JumpList._ShellObjectPair> GenerateJumpItems(IObjectArray shellObjects)
		{
			List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
			Guid guid = new Guid("00000000-0000-0000-C000-000000000046");
			uint count = shellObjects.GetCount();
			for (uint num = 0U; num < count; num += 1U)
			{
				object at = shellObjects.GetAt(num, ref guid);
				JumpItem jumpItem = null;
				try
				{
					jumpItem = JumpList.GetJumpItemForShellObject(at);
				}
				catch (Exception ex)
				{
					if (ex is NullReferenceException || ex is SEHException)
					{
						throw;
					}
				}
				list.Add(new JumpList._ShellObjectPair
				{
					ShellObject = at,
					JumpItem = jumpItem
				});
			}
			return list;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B8C File Offset: 0x00000D8C
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList)
		{
			JumpList.AddCategory(cdl, category, jumpItems, successList, rejectionList, true);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B9C File Offset: 0x00000D9C
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList, bool isHeterogenous)
		{
			IObjectCollection objectCollection = (IObjectCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("2d3468c1-36a7-43b6-ac24-d3f02fd9607a")));
			foreach (JumpList._ShellObjectPair shellObjectPair in jumpItems)
			{
				objectCollection.AddObject(shellObjectPair.ShellObject);
			}
			HRESULT hrLeft;
			if (string.IsNullOrEmpty(category))
			{
				hrLeft = cdl.AddUserTasks(objectCollection);
			}
			else
			{
				hrLeft = cdl.AppendCategory(category, objectCollection);
			}
			if (hrLeft.Succeeded)
			{
				int num = jumpItems.Count;
				while (--num >= 0)
				{
					successList.Add(jumpItems[num].JumpItem);
				}
				return;
			}
			if (isHeterogenous && hrLeft == HRESULT.DESTS_E_NO_MATCHING_ASSOC_HANDLER)
			{
				Utility.SafeRelease<IObjectCollection>(ref objectCollection);
				List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
				foreach (JumpList._ShellObjectPair shellObjectPair2 in jumpItems)
				{
					if (shellObjectPair2.JumpItem is JumpPath)
					{
						rejectionList.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = shellObjectPair2.JumpItem,
							Reason = JumpItemRejectionReason.NoRegisteredHandler
						});
					}
					else
					{
						list.Add(shellObjectPair2);
					}
				}
				if (list.Count > 0)
				{
					JumpList.AddCategory(cdl, category, list, successList, rejectionList, false);
					return;
				}
			}
			else
			{
				foreach (JumpList._ShellObjectPair shellObjectPair3 in jumpItems)
				{
					rejectionList.Add(new JumpList._RejectedJumpItemPair
					{
						JumpItem = shellObjectPair3.JumpItem,
						Reason = JumpItemRejectionReason.InvalidItem
					});
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D64 File Offset: 0x00000F64
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private static IShellLinkW CreateLinkFromJumpTask(JumpTask jumpTask, bool allowSeparators)
		{
			if (string.IsNullOrEmpty(jumpTask.Title) && (!allowSeparators || !string.IsNullOrEmpty(jumpTask.CustomCategory)))
			{
				return null;
			}
			IShellLinkW shellLinkW = (IShellLinkW)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046")));
			IShellLinkW result;
			try
			{
				string path = JumpList._FullName;
				if (!string.IsNullOrEmpty(jumpTask.ApplicationPath))
				{
					path = jumpTask.ApplicationPath;
				}
				shellLinkW.SetPath(path);
				if (!string.IsNullOrEmpty(jumpTask.WorkingDirectory))
				{
					shellLinkW.SetWorkingDirectory(jumpTask.WorkingDirectory);
				}
				if (!string.IsNullOrEmpty(jumpTask.Arguments))
				{
					shellLinkW.SetArguments(jumpTask.Arguments);
				}
				if (jumpTask.IconResourceIndex != -1)
				{
					string pszIconPath = JumpList._FullName;
					if (!string.IsNullOrEmpty(jumpTask.IconResourcePath))
					{
						if ((long)jumpTask.IconResourcePath.Length >= 260L)
						{
							return null;
						}
						pszIconPath = jumpTask.IconResourcePath;
					}
					shellLinkW.SetIconLocation(pszIconPath, jumpTask.IconResourceIndex);
				}
				if (!string.IsNullOrEmpty(jumpTask.Description))
				{
					shellLinkW.SetDescription(jumpTask.Description);
				}
				IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
				using (PROPVARIANT propvariant = new PROPVARIANT())
				{
					PKEY pkey = default(PKEY);
					if (!string.IsNullOrEmpty(jumpTask.Title))
					{
						propvariant.SetValue(jumpTask.Title);
						pkey = PKEY.Title;
					}
					else
					{
						propvariant.SetValue(true);
						pkey = PKEY.AppUserModel_IsDestListSeparator;
					}
					propertyStore.SetValue(ref pkey, propvariant);
				}
				propertyStore.Commit();
				IShellLinkW shellLinkW2 = shellLinkW;
				shellLinkW = null;
				result = shellLinkW2;
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				Utility.SafeRelease<IShellLinkW>(ref shellLinkW);
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002F30 File Offset: 0x00001130
		private static IShellItem2 GetShellItemForPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			Guid guid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
			object obj;
			HRESULT hrLeft = NativeMethods.SHCreateItemFromParsingName(path, null, ref guid, out obj);
			if (hrLeft == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || hrLeft == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
			{
				hrLeft = HRESULT.S_OK;
				obj = null;
			}
			hrLeft.ThrowIfFailed();
			return (IShellItem2)obj;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002FA0 File Offset: 0x000011A0
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private static IShellItem2 CreateItemFromJumpPath(JumpPath jumpPath)
		{
			try
			{
				return JumpList.GetShellItemForPath(Path.GetFullPath(jumpPath.Path));
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002FD8 File Offset: 0x000011D8
		private static JumpItem GetJumpItemForShellObject(object shellObject)
		{
			IShellItem2 shellItem = shellObject as IShellItem2;
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellItem != null)
			{
				return new JumpPath
				{
					Path = shellItem.GetDisplayName((SIGDN)2147647488U)
				};
			}
			if (shellLinkW != null)
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				shellLinkW.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
				StringBuilder stringBuilder2 = new StringBuilder(1024);
				shellLinkW.GetArguments(stringBuilder2, stringBuilder2.Capacity);
				StringBuilder stringBuilder3 = new StringBuilder(1024);
				shellLinkW.GetDescription(stringBuilder3, stringBuilder3.Capacity);
				StringBuilder stringBuilder4 = new StringBuilder(260);
				int iconResourceIndex;
				shellLinkW.GetIconLocation(stringBuilder4, stringBuilder4.Capacity, out iconResourceIndex);
				StringBuilder stringBuilder5 = new StringBuilder(260);
				shellLinkW.GetWorkingDirectory(stringBuilder5, stringBuilder5.Capacity);
				JumpTask jumpTask = new JumpTask
				{
					ApplicationPath = stringBuilder.ToString(),
					Arguments = stringBuilder2.ToString(),
					Description = stringBuilder3.ToString(),
					IconResourceIndex = iconResourceIndex,
					IconResourcePath = stringBuilder4.ToString(),
					WorkingDirectory = stringBuilder5.ToString()
				};
				using (PROPVARIANT propvariant = new PROPVARIANT())
				{
					IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
					PKEY title = PKEY.Title;
					propertyStore.GetValue(ref title, propvariant);
					jumpTask.Title = (propvariant.GetValue() ?? "");
				}
				return jumpTask;
			}
			return null;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003158 File Offset: 0x00001358
		private static string ShellLinkToString(IShellLinkW shellLink)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			shellLink.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
			string text = null;
			using (PROPVARIANT propvariant = new PROPVARIANT())
			{
				IPropertyStore propertyStore = (IPropertyStore)shellLink;
				PKEY title = PKEY.Title;
				propertyStore.GetValue(ref title, propvariant);
				text = (propvariant.GetValue() ?? "");
			}
			StringBuilder stringBuilder2 = new StringBuilder(1024);
			shellLink.GetArguments(stringBuilder2, stringBuilder2.Capacity);
			return stringBuilder.ToString().ToUpperInvariant() + text.ToUpperInvariant() + stringBuilder2.ToString();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003204 File Offset: 0x00001404
		private void RejectEverything()
		{
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			if (jumpItemsRejected == null)
			{
				this._jumpItems.Clear();
				return;
			}
			if (this._jumpItems.Count > 0)
			{
				List<JumpItemRejectionReason> list = new List<JumpItemRejectionReason>(this.JumpItems.Count);
				for (int i = 0; i < this.JumpItems.Count; i++)
				{
					list.Add(JumpItemRejectionReason.InvalidItem);
				}
				JumpItemsRejectedEventArgs e = new JumpItemsRejectedEventArgs(this.JumpItems, list);
				this._jumpItems.Clear();
				jumpItemsRejected(this, e);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00003284 File Offset: 0x00001484
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x000032BC File Offset: 0x000014BC
		public event EventHandler<JumpItemsRejectedEventArgs> JumpItemsRejected;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000030 RID: 48 RVA: 0x000032F4 File Offset: 0x000014F4
		// (remove) Token: 0x06000031 RID: 49 RVA: 0x0000332C File Offset: 0x0000152C
		public event EventHandler<JumpItemsRemovedEventArgs> JumpItemsRemovedByUser;

		// Token: 0x0400000A RID: 10
		private static readonly object s_lock = new object();

		// Token: 0x0400000B RID: 11
		private static readonly Dictionary<Application, JumpList> s_applicationMap = new Dictionary<Application, JumpList>();

		// Token: 0x0400000C RID: 12
		private Application _application;

		// Token: 0x0400000D RID: 13
		private bool? _initializing;

		// Token: 0x0400000E RID: 14
		private List<JumpItem> _jumpItems;

		// Token: 0x0400000F RID: 15
		private static readonly string _FullName = NativeMethods.GetModuleFileName(IntPtr.Zero);

		// Token: 0x02000007 RID: 7
		private class _RejectedJumpItemPair
		{
			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000032 RID: 50 RVA: 0x00003361 File Offset: 0x00001561
			// (set) Token: 0x06000033 RID: 51 RVA: 0x00003369 File Offset: 0x00001569
			public JumpItem JumpItem { get; set; }

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x06000034 RID: 52 RVA: 0x00003372 File Offset: 0x00001572
			// (set) Token: 0x06000035 RID: 53 RVA: 0x0000337A File Offset: 0x0000157A
			public JumpItemRejectionReason Reason { get; set; }
		}

		// Token: 0x02000008 RID: 8
		private class _ShellObjectPair
		{
			// Token: 0x1700000C RID: 12
			// (get) Token: 0x06000037 RID: 55 RVA: 0x0000338B File Offset: 0x0000158B
			// (set) Token: 0x06000038 RID: 56 RVA: 0x00003393 File Offset: 0x00001593
			public JumpItem JumpItem { get; set; }

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x06000039 RID: 57 RVA: 0x0000339C File Offset: 0x0000159C
			// (set) Token: 0x0600003A RID: 58 RVA: 0x000033A4 File Offset: 0x000015A4
			public object ShellObject { get; set; }

			// Token: 0x0600003B RID: 59 RVA: 0x000033B0 File Offset: 0x000015B0
			public static void ReleaseShellObjects(List<JumpList._ShellObjectPair> list)
			{
				if (list != null)
				{
					foreach (JumpList._ShellObjectPair shellObjectPair in list)
					{
						object shellObject = shellObjectPair.ShellObject;
						shellObjectPair.ShellObject = null;
						Utility.SafeRelease<object>(ref shellObject);
					}
				}
			}
		}
	}
}
