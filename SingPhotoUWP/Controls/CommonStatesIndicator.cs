using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SingPhotoUWP
{
    public class CommonStatesIndicator : ContentControl
    {
        public CommonStatesIndicator()
        {
            DefaultStyleKey = typeof(CommonStatesIndicator);
        }

        /// <summary>
        ///     标识 AttachedElement 依赖属性。
        /// </summary>
        public static readonly DependencyProperty AttachedElementProperty =
            DependencyProperty.Register("AttachedElement", typeof(Control), typeof(CommonStatesIndicator), new PropertyMetadata(null, OnAttachedElementChanged));

        /// <summary>
        ///     获取或设置AttachedElement的值
        /// </summary>
        public Control AttachedElement
        {
            get { return (Control)GetValue(AttachedElementProperty); }
            set { SetValue(AttachedElementProperty, value); }
        }

        private static void OnAttachedElementChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as CommonStatesIndicator;
            var oldValue = (Control)args.OldValue;
            var newValue = (Control)args.NewValue;
            if (oldValue != newValue)
                target.OnAttachedElementChanged(oldValue, newValue);
        }

        protected virtual void OnAttachedElementChanged(Control oldValue, Control newValue)
        {
            if (newValue == null)
                return;

            var rootElement = AttachedElement.GetVisualDescendants().OfType<FrameworkElement>().FirstOrDefault();
            if (rootElement == null)
            {
                newValue.SizeChanged += OnAttachedElementSizeChanged;
                return;
            }

            var group = VisualStateManager.GetVisualStateGroups(rootElement).FirstOrDefault(g => g.Name == "CommonStates");
            if (group == null)
            {
                newValue.SizeChanged += OnAttachedElementSizeChanged;
                return;
            }

            group.CurrentStateChanged += (s, e) =>
            {
                Content = e.NewState.Name;
            };

            if (group.CurrentState != null)
                Content = group.CurrentState.Name;
        }

        private void OnAttachedElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AttachedElement.SizeChanged -= OnAttachedElementSizeChanged;
            var rootElement = AttachedElement.GetVisualDescendants().OfType<FrameworkElement>().FirstOrDefault();
            if (rootElement == null)
                return;

            var group = VisualStateManager.GetVisualStateGroups(rootElement).FirstOrDefault(g => g.Name == "CommonStates");
             if (group == null)
                return;

            group.CurrentStateChanged += (s, args) =>
            {
                Content = args.NewState.Name;
            };

            if (group.CurrentState != null)
                Content = group.CurrentState.Name;
        }
    }
}