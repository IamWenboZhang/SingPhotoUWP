using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SingPhotoUWP
{
    public static class VisualTreeExtensions
    {
        public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return GetVisualDescendantsAndSelfIterator(element).Skip(1);
        }

        private static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator(DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            var remaining = new Queue<DependencyObject>();
            remaining.Enqueue(element);

            while (remaining.Count > 0)
            {
                var obj = remaining.Dequeue();
                yield return obj;

                foreach (DependencyObject child in obj.GetVisualChildren())
                    remaining.Enqueue(child);
            }
        }

        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return GetVisualChildrenAndSelfIterator(element).Skip(1);
        }

        private static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(this DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            yield return element;

            var count = VisualTreeHelper.GetChildrenCount(element);
            for (var i = 0; i < count; i++)
                yield return VisualTreeHelper.GetChild(element, i);
        }
    }
}