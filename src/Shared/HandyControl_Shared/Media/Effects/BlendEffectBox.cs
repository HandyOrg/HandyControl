using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Effects;

namespace HandyControl.Media.Effects
{
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public class BlendEffectBox : Control
    {
        private readonly ContentPresenter _effectBottomPresenter;

        private readonly ContentPresenter _effectTopPresenter;

        private bool _isInternalAction;

        public BlendEffectBox()
        {
            _effectTopPresenter = new ContentPresenter();
            ActualContent = _effectTopPresenter;

            _effectBottomPresenter = new ContentPresenter();
            _effectBottomPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding(ContentProperty.Name)
            {
                Source = this
            });

            var effects = new ObservableCollection<Effect>();
            effects.CollectionChanged += OnEffectsChanged;
            Effects = effects;
        }

        private void OnEffectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Effects == null || Effects.Count == 0)
            {
                ClearEffect(_effectTopPresenter);
                return;
            }

            if (_isInternalAction) return;

            _isInternalAction = true;
            AddEffect(_effectTopPresenter, Effects.Count);
            _isInternalAction = false;
        }

        private void ClearEffect(ContentPresenter presenter)
        {
            if (presenter == null) return;

            if (ReferenceEquals(_effectBottomPresenter, presenter))
            {
                _effectBottomPresenter.SetCurrentValue(EffectProperty, null);
                return;
            }

            presenter.SetCurrentValue(EffectProperty, null);
            ClearEffect(presenter.Content as ContentPresenter);
        }

        private void AddEffect(ContentPresenter presenter, int count)
        {
            var newCount = --count;

            if (newCount >= 0)
            {
                presenter.Effect = Effects[newCount];

                var nextCount = --count;
                if (nextCount >= 1)
                {
                    var content = new ContentPresenter();
                    presenter.Content = content;
                    AddEffect(content, newCount);
                }
                else if (nextCount >= 0)
                {
                    _effectBottomPresenter.Effect = Effects[0];
                    presenter.Content = _effectBottomPresenter;
                }
                else
                {
                    presenter.Content = _effectBottomPresenter;
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Bindable(true)]
        public Collection<Effect> Effects { get; }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(FrameworkElement), typeof(BlendEffectBox), new PropertyMetadata(default(FrameworkElement)));

        public FrameworkElement Content
        {
            get => (FrameworkElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        internal static readonly DependencyProperty ActualContentProperty = DependencyProperty.Register(
            "ActualContent", typeof(FrameworkElement), typeof(BlendEffectBox), new PropertyMetadata(default(FrameworkElement)));

        internal FrameworkElement ActualContent
        {
            get => (FrameworkElement) GetValue(ActualContentProperty);
            set => SetValue(ActualContentProperty, value);
        }
    }
}