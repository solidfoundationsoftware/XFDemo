using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XFDemoApp.Platform.Effects;

namespace XFDemoApp.Behaviors
{
    public class HighlightEntryOnFocusBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty HighlightColorProperty =
            BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(HighlightEntryOnFocusBehavior), Color.Black);

        public Color HighlightColor
        {
            get { return (Color)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Focused += OnFocus;
            bindable.Unfocused += OnLostFocus;
        }

        private void OnLostFocus(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;

            RemoveEffect(entry?.Parent);
        }

        private void OnFocus(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;

            AddEffect(entry?.Parent);
        }

        private void AddEffect(Element element)
        {
            if (element == null) return;

            var effect = GetEffect(element);
            if (effect == null)
            {
                element.Effects.Add(new DropShadowColorEffect { Color = HighlightColor });
            }
        }

        private void RemoveEffect(Element element)
        {
            if (element == null) return;

            var effect = GetEffect(element);
            if (effect != null)
            {
                element.Effects.Remove(effect);
            }
        }

        private DropShadowColorEffect GetEffect(Element element)
        {
            if (element == null) return null;

            return element.Effects.FirstOrDefault(p => p is DropShadowColorEffect) as DropShadowColorEffect;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Focused -= OnFocus;
            bindable.Unfocused -= OnLostFocus;

            RemoveEffect(bindable.Parent);

            base.OnDetachingFrom(bindable);

        }
    }
}
