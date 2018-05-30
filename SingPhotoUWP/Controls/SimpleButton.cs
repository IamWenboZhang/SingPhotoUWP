using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SingPhotoUWP
{
    [TemplateVisualState(Name = StateNormal, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StatePointerOver, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StatePressed, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateDisabled, GroupName = GroupCommon)]
    public class SimpleButton : ContentControl
    {
        internal const string StateNormal = "Normal";
        internal const string StatePointerOver = "PointerOver";
        internal const string StatePressed = "Pressed";
        internal const string StateDisabled = "Disabled";
        internal const string GroupCommon = "CommonStates";
        internal const string StateFocused = "Focused";


        private bool _isPointerCaptured;


        public SimpleButton()
        {
            DefaultStyleKey = typeof(SimpleButton);
            IsEnabledChanged += OnIsEnabledChanged;
        }

        public bool IsPressed { get; private set; }

        public bool IsPointerOver { get; private set; }

        public event RoutedEventHandler Click;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        internal void UpdateVisualState(bool useTransitions = true)
        {
            if (IsEnabled == false)
                VisualStateManager.GoToState(this, StateDisabled, useTransitions);
            else if (IsPressed)
                VisualStateManager.GoToState(this, StatePressed, useTransitions);
            else if (IsPointerOver)
                VisualStateManager.GoToState(this, StatePointerOver, useTransitions);
            else
                VisualStateManager.GoToState(this, StateNormal, useTransitions);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.Handled)
                return;

            if (IsEnabled == false)
                return;

            e.Handled = true;
            _isPointerCaptured = CapturePointer(e.Pointer);
            if (_isPointerCaptured == false)
                return;

            IsPressed = true;
            Focus(FocusState.Pointer);
            UpdateVisualState();
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (e.Handled)
                return;

            if (IsEnabled == false)
                return;

            e.Handled = true;
            if (IsPressed)
                Click?.Invoke(this, new RoutedEventArgs());

            IsPressed = false;
            ReleasePointerCapture(e.Pointer);
            _isPointerCaptured = false;
            UpdateVisualState();
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_isPointerCaptured == false)
                return;

            var position = e.GetCurrentPoint(this).Position;
            if (position.X < 0 || position.Y < 0 || position.X > ActualWidth || position.Y > ActualHeight)
                IsPressed = false;
            else
                IsPressed = true;

            UpdateVisualState();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            IsPointerOver = true;
            UpdateVisualState();
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            IsPointerOver = false;
            UpdateVisualState();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsEnabled)
            {
                IsPressed = false;
                IsPointerOver = false;
                _isPointerCaptured = false;
            }
            UpdateVisualState();
        }
    }
}