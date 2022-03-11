using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XFDemoApp.Behaviors
{
    public class HighlightSelectedItemBehavior: Behavior<CollectionView>
    {
        protected override void OnAttachedTo(CollectionView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.SelectionChanged += Bindable_SelectionChanged;
        }

        private void Bindable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"\nSELECTION CHANGED: {sender}, {e.CurrentSelection.Count}");

            var collectionView = (CollectionView)sender;
            
        }

        protected override void OnDetachingFrom(CollectionView bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.SelectionChanged -= Bindable_SelectionChanged;
        }
    }
}
