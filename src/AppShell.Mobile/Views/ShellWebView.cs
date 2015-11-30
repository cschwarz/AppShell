using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppShell.Mobile
{
    public class ShellWebView : View
    {
        class Message
        {
            [JsonProperty("a")]
            public string Action { get; set; }
            [JsonProperty("d")]
            public object Data { get; set; }
        }
        
        public static readonly BindableProperty SourceProperty = BindableProperty.Create<ShellWebView, WebViewSource>(d => d.Source, null);
        public WebViewSource Source { get { return (WebViewSource)GetValue(SourceProperty); } set { SetValue(SourceProperty, value); } }

        public EventHandler LoadFinished;

        internal EventHandler<string> InjectJavaScriptRequested;
        internal Dictionary<string, Action<string>> Callbacks;

        public ShellWebView()
        {
            Callbacks = new Dictionary<string, Action<string>>();
        }

        public void InjectJavaScript(string script)
        {
            if (InjectJavaScriptRequested != null)
                InjectJavaScriptRequested(this, script);
        }

        public void RegisterCallback(string name, Action<string> callback)
        {
            Callbacks.Add(name, callback);
        }

        public void RemoveCallback(string name)
        {
            Callbacks.Remove(name);
        }

        internal void MessageReceived(string message)
        {
            Message m = JsonConvert.DeserializeObject<Message>(message);

            if (Callbacks.ContainsKey(m.Action))
                Callbacks[m.Action](m.Data.ToString());
        }

        internal void OnLoadFinished(object sender, EventArgs e)
        {
            if (LoadFinished != null)
                LoadFinished(sender, e);
        }
    }
}
