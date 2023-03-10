using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VsProject.Services
{
    public static class ViewMappings
    {
        private static readonly Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        public static void Initialize()
        {
            // Load mappings from XML file
            XDocument doc = XDocument.Load("ViewMappings.xml");
            foreach (XElement element in doc.Root.Elements("Mapping"))
            {
                Type viewModelType = Type.GetType((string)element.Attribute("ViewModelType"));
                Type viewType = Type.GetType((string)element.Attribute("ViewType"));
                _mappings[viewModelType] = viewType;
            }
        }

        public static Type GetViewType(Type viewModelType)
        {
            if (_mappings.TryGetValue(viewModelType, out Type viewType))
            {
                return viewType;
            }
            else
            {
                throw new InvalidOperationException($"No view type registered for view model type {viewModelType}");
            }
        }
    }

}
