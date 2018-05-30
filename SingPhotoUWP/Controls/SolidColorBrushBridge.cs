using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;

namespace SingPhotoUWP
{
    public class SolidColorBrushBridge : FrameworkElement
    {
        /// <summary>
        ///     标识 InputBrush 依赖属性。
        /// </summary>
        public static readonly DependencyProperty InputBrushProperty =
            DependencyProperty.Register("InputBrush", typeof(Brush), typeof(SolidColorBrushBridge), new PropertyMetadata(null, OnInputBrushChanged));

        /// <summary>
        ///     标识 OutputBrush 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OutputBrushProperty =
            DependencyProperty.Register("OutputBrush", typeof(SolidColorBrush), typeof(SolidColorBrushBridge), new PropertyMetadata(null));

        /// <summary>
        ///     标识 Value 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SolidColorBrushBridge), new PropertyMetadata(1d, OnValueChanged));

        /// <summary>
        ///     标识 Saturation 依赖属性。
        /// </summary>
        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(double), typeof(SolidColorBrushBridge), new PropertyMetadata(1d, OnSaturationChanged));

        /// <summary>
        ///     获取或设置InputBrush的值
        /// </summary>
        public Brush InputBrush
        {
            get { return (Brush) GetValue(InputBrushProperty); }
            set { SetValue(InputBrushProperty, value); }
        }


        /// <summary>
        ///     获取或设置OutputBrush的值
        /// </summary>
        public SolidColorBrush OutputBrush
        {
            get { return (SolidColorBrush) GetValue(OutputBrushProperty); }
            set { SetValue(OutputBrushProperty, value); }
        }


        /// <summary>
        ///     获取或设置Value的值
        /// </summary>
        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        /// <summary>
        ///     获取或设置Saturation的值
        /// </summary>
        public double Saturation
        {
            get { return (double) GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }

        private static void OnInputBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as SolidColorBrushBridge;
            var oldValue = (Brush) args.OldValue;
            var newValue = (Brush) args.NewValue;
            if (oldValue != newValue)
                target.OnInputBrushChanged(oldValue, newValue);
        }

        protected virtual void OnInputBrushChanged(Brush oldValue, Brush newValue)
        {
            UpdateOutputBrush();
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as SolidColorBrushBridge;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnValueChanged(oldValue, newValue);
        }

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            UpdateOutputBrush();
        }

        private static void OnSaturationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var target = obj as SolidColorBrushBridge;
            var oldValue = (double) args.OldValue;
            var newValue = (double) args.NewValue;
            if (oldValue != newValue)
                target.OnSaturationChanged(oldValue, newValue);
        }

        protected virtual void OnSaturationChanged(double oldValue, double newValue)
        {
            UpdateOutputBrush();
        }

        private void UpdateOutputBrush()
        {
            var brush = InputBrush as SolidColorBrush;
            if (brush == null)
            {
                OutputBrush = null;
            }
            else
            {
                var color = brush.Color.ToHsv();
                OutputBrush = new SolidColorBrush(ColorHelper.FromHsv(color.H, Saturation, Value));
            }
        }
    }
}