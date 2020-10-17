using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Windows.Shell
{
	// Token: 0x020000A2 RID: 162
	[DefaultEvent("Click")]
	public sealed class ThumbButtonInfo : Freezable, ICommandSource
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000837A File Offset: 0x0000657A
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfo();
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00008381 File Offset: 0x00006581
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00008393 File Offset: 0x00006593
		public Visibility Visibility
		{
			get
			{
				return (Visibility)base.GetValue(ThumbButtonInfo.VisibilityProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.VisibilityProperty, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x000083A6 File Offset: 0x000065A6
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x000083B8 File Offset: 0x000065B8
		public bool DismissWhenClicked
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.DismissWhenClickedProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DismissWhenClickedProperty, value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000083CB File Offset: 0x000065CB
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x000083DD File Offset: 0x000065DD
		public ImageSource ImageSource
		{
			get
			{
				return (ImageSource)base.GetValue(ThumbButtonInfo.ImageSourceProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.ImageSourceProperty, value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000083EB File Offset: 0x000065EB
		// (set) Token: 0x060002EB RID: 747 RVA: 0x000083FD File Offset: 0x000065FD
		public bool IsBackgroundVisible
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsBackgroundVisibleProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsBackgroundVisibleProperty, value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00008410 File Offset: 0x00006610
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00008422 File Offset: 0x00006622
		public string Description
		{
			get
			{
				return (string)base.GetValue(ThumbButtonInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DescriptionProperty, value);
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00008430 File Offset: 0x00006630
		private static object _CoerceDescription(DependencyObject d, object value)
		{
			string text = (string)value;
			if (text != null && text.Length >= 260)
			{
				text = text.Substring(0, 259);
			}
			return text;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00008464 File Offset: 0x00006664
		private object _CoerceIsEnabledValue(object value)
		{
			bool flag = (bool)value;
			return flag && this._CanExecute;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x00008489 File Offset: 0x00006689
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000849B File Offset: 0x0000669B
		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsEnabledProperty, value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000084AE File Offset: 0x000066AE
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000084C0 File Offset: 0x000066C0
		public bool IsInteractive
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsInteractiveProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsInteractiveProperty, value);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000084D4 File Offset: 0x000066D4
		private void _OnCommandChanged(DependencyPropertyChangedEventArgs e)
		{
			ICommand command = (ICommand)e.OldValue;
			ICommand command2 = (ICommand)e.NewValue;
			if (command == command2)
			{
				return;
			}
			if (command != null)
			{
				this._UnhookCommand(command);
			}
			if (command2 != null)
			{
				this._HookCommand(command2);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00008514 File Offset: 0x00006714
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x00008526 File Offset: 0x00006726
		private bool _CanExecute
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo._CanExecuteProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo._CanExecuteProperty, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002F7 RID: 759 RVA: 0x0000853C File Offset: 0x0000673C
		// (remove) Token: 0x060002F8 RID: 760 RVA: 0x00008574 File Offset: 0x00006774
		public event EventHandler Click;

		// Token: 0x060002F9 RID: 761 RVA: 0x000085AC File Offset: 0x000067AC
		internal void InvokeClick()
		{
			EventHandler click = this.Click;
			if (click != null)
			{
				click(this, EventArgs.Empty);
			}
			this._InvokeCommand();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000085D8 File Offset: 0x000067D8
		private void _InvokeCommand()
		{
			ICommand command = this.Command;
			if (command != null)
			{
				object commandParameter = this.CommandParameter;
				IInputElement commandTarget = this.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (routedCommand.CanExecute(commandParameter, commandTarget))
					{
						routedCommand.Execute(commandParameter, commandTarget);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000862A File Offset: 0x0000682A
		private void _UnhookCommand(ICommand command)
		{
			command.CanExecuteChanged -= this._commandEvent;
			this._commandEvent = null;
			this._UpdateCanExecute();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000864D File Offset: 0x0000684D
		private void _HookCommand(ICommand command)
		{
			this._commandEvent = delegate(object sender, EventArgs e)
			{
				this._UpdateCanExecute();
			};
			command.CanExecuteChanged += this._commandEvent;
			this._UpdateCanExecute();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00008674 File Offset: 0x00006874
		private void _UpdateCanExecute()
		{
			if (this.Command == null)
			{
				this._CanExecute = true;
				return;
			}
			object commandParameter = this.CommandParameter;
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = this.Command as RoutedCommand;
			if (routedCommand != null)
			{
				this._CanExecute = routedCommand.CanExecute(commandParameter, commandTarget);
				return;
			}
			this._CanExecute = this.Command.CanExecute(commandParameter);
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000086CF File Offset: 0x000068CF
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000086E1 File Offset: 0x000068E1
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ThumbButtonInfo.CommandProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandProperty, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000300 RID: 768 RVA: 0x000086EF File Offset: 0x000068EF
		// (set) Token: 0x06000301 RID: 769 RVA: 0x000086FC File Offset: 0x000068FC
		public object CommandParameter
		{
			get
			{
				return base.GetValue(ThumbButtonInfo.CommandParameterProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandParameterProperty, value);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000870A File Offset: 0x0000690A
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000871C File Offset: 0x0000691C
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(ThumbButtonInfo.CommandTargetProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandTargetProperty, value);
			}
		}

		// Token: 0x040005D7 RID: 1495
		private EventHandler _commandEvent;

		// Token: 0x040005D8 RID: 1496
		public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ThumbButtonInfo), new PropertyMetadata(Visibility.Visible));

		// Token: 0x040005D9 RID: 1497
		public static readonly DependencyProperty DismissWhenClickedProperty = DependencyProperty.Register("DismissWhenClicked", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(false));

		// Token: 0x040005DA RID: 1498
		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ThumbButtonInfo), new PropertyMetadata(null));

		// Token: 0x040005DB RID: 1499
		public static readonly DependencyProperty IsBackgroundVisibleProperty = DependencyProperty.Register("IsBackgroundVisible", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		// Token: 0x040005DC RID: 1500
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ThumbButtonInfo), new PropertyMetadata(string.Empty, null, new CoerceValueCallback(ThumbButtonInfo._CoerceDescription)));

		// Token: 0x040005DD RID: 1501
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, null, (DependencyObject d, object e) => ((ThumbButtonInfo)d)._CoerceIsEnabledValue(e)));

		// Token: 0x040005DE RID: 1502
		public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register("IsInteractive", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		// Token: 0x040005DF RID: 1503
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d)._OnCommandChanged(e);
		}));

		// Token: 0x040005E0 RID: 1504
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d)._UpdateCanExecute();
		}));

		// Token: 0x040005E1 RID: 1505
		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d)._UpdateCanExecute();
		}));

		// Token: 0x040005E2 RID: 1506
		private static readonly DependencyProperty _CanExecuteProperty = DependencyProperty.Register("_CanExecute", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ThumbButtonInfo.IsEnabledProperty);
		}));
	}
}
