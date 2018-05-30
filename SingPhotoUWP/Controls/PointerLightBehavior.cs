using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Xaml.Interactivity;
using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace SingPhotoUWP
{
    public class PointerLightBehavior : Behavior<ContentControl>
    {
        private PointLight _pointLight;
        private float _fontSize;
        private UIElement _text;

        /// <summary>
        /// 获取或设置Color的值
        /// </summary>  
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// 标识 Color 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(PointerLightBehavior), new PropertyMetadata(Colors.White, OnColorChanged));

        private static void OnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PointerLightBehavior target = obj as PointerLightBehavior;
            Color oldValue = (Color)args.OldValue;
            Color newValue = (Color)args.NewValue;
            if (oldValue != newValue)
                target.OnColorChanged(oldValue, newValue);
        }

        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {
            if (_pointLight != null)
                _pointLight.Color = Color;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerMoved += OnPointerMoved;
            AssociatedObject.PointerExited += OnPointerExited;

            var text = AssociatedObject.FindDescendant<TextBlock>();
            if (text == null)
                AssociatedObject.Loaded += OnAssociatedObjectedLoaded;
            else
                CreatePointLight();
        }

        private void CreatePointLight()
        {
            var text = AssociatedObject.FindDescendant<TextBlock>();
            _text = (UIElement)text ?? AssociatedObject;
            var visual = ElementCompositionPreview.GetElementVisual(_text);
            var compositor = visual.Compositor;
            _fontSize = (float)(AssociatedObject.FontSize/0.7);
            _pointLight = compositor.CreatePointLight();
            _pointLight.Color = Color;
            _pointLight.CoordinateSpace = visual;
            _pointLight.Targets.Add(visual);

            _pointLight.Offset = new Vector3(-1000, (float)AssociatedObject.ActualHeight / 2, _fontSize);
        }

        private void OnAssociatedObjectedLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnAssociatedObjectedLoaded;
            CreatePointLight();
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(_text);
            //starts out to the left; vertically centered; light's z-offset is related to fontsize
            _pointLight.Offset = new Vector3((float) position.Position.X, (float) position.Position.Y, _fontSize);
            Debug.WriteLine(position.Position.X + "---" + position.Position.Y +"---" + _fontSize);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _pointLight.Offset = new Vector3(-(float) AssociatedObject.ActualWidth * 5, (float) AssociatedObject.ActualHeight / 2, (float) AssociatedObject.FontSize);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerMoved -= OnPointerMoved;
            AssociatedObject.PointerExited -= OnPointerExited;
            AssociatedObject.Loaded -= OnAssociatedObjectedLoaded;
        }
    }
}