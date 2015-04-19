using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace AppShell.Desktop
{
    public class DesktopDataTemplateFactory : IDataTemplateFactory
    {
        public object GetDataTemplate(Type viewType)
        {               
            string xaml = string.Format("<DataTemplate><v:{0} /></DataTemplate>", viewType.Name);

            ParserContext context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);            
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");            
            context.XmlnsDictionary.Add("v", "v");

            return (DataTemplate)XamlReader.Parse(xaml, context);
        }
    }
}
