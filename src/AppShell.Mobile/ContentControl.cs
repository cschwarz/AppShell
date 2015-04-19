using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create<ContentControl, DataTemplate>(x => x.ContentTemplate, null, propertyChanged: OnContentTemplateChanged);
        public static readonly BindableProperty ContentContextProperty = BindableProperty.Create<ContentControl, object>(x => x.ContentContext, null, propertyChanged: OnContentContextChanged);

        private static void OnContentTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ContentControl contentControl = (ContentControl)bindable;

            DataTemplate template = contentControl.ContentTemplate;
            if (template != null)
            {
                View content = (View)template.CreateContent();
                content.BindingContext = contentControl.ContentContext;
                contentControl.Content = content;                
            }
            else
            {
                contentControl.Content = null;
            }
        }

        private static void OnContentContextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ContentControl contentControl = (ContentControl)bindable;
            if (contentControl.Content != null)
                contentControl.Content.BindingContext = newValue;
        }        

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public object ContentContext
        {
            get { return (object)GetValue(ContentContextProperty); }
            set { SetValue(ContentContextProperty, value); }
        }
    }
}
