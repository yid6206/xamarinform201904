using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms
{
    public static class ElementExtensions
    {
        public static TElement GetParent<TElement>(this Element element) where TElement : Element
        {
            var target = element.Parent;
            while (target != null)
            {
                if (target is TElement)
                    return (TElement)target;
                target = target.Parent;
            }
            return null;
        }
    }
}
