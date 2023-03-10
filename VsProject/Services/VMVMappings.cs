using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace VsProject.Services
{
    public static class VMVMappings
    {
        private static readonly Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        private static string _mappingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source/repos/Gesdent/VsProject/Resources/Data", "ViewModelViewMappings.xml");
        public static void Initialize()
        {
            // Load mappings from XML file
            if(!File.Exists(_mappingsFile))
            {
                GenerateFromFolder();
            }
            XDocument doc = XDocument.Load(_mappingsFile);
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

        public static void GenerateFromFolder()
        {
  
            using (XmlWriter writer = XmlWriter.Create(_mappingsFile))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Mappings");
                Debug.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source\\repos\\Gesdent\\VsProject\\ViewModels"));

                string viewModelFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source\\repos\\Gesdent\\VsProject\\ViewModels");
                if (!Directory.Exists(viewModelFolder))
                {
                    Directory.CreateDirectory(viewModelFolder);
                }
                string viewFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source\\repos\\Gesdent\\VsProject\\Views");
                if (!Directory.Exists(viewFolder))
                {
                    Directory.CreateDirectory(viewFolder);
                }

                string[] viewModelFiles = Directory.GetFiles(viewModelFolder, "*.cs");
                string[] viewFiles = Directory.GetFiles(viewFolder, "*.xaml");

                foreach (string viewModelFile in viewModelFiles)
                {
                    string viewModelName = Path.GetFileNameWithoutExtension(viewModelFile);
                    string viewName = viewModelName.Replace("ViewModel", "View");

                    string viewFile = Path.Combine(viewFolder, viewName + ".xaml");
                    if (File.Exists(viewFile))
                    {
                        writer.WriteStartElement("Mapping");
                        writer.WriteAttributeString("ViewModelType", "VsProject.ViewModels." + viewModelName);
                        writer.WriteAttributeString("ViewType", "VsProject.Views." + viewName);
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                
            }

        }
    }

}
