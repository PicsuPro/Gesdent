using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace VsProject.Services
{
    public static class VMVMappings
    {
        private static readonly Dictionary<Type, (Type, Type?)> _mappings = new Dictionary<Type, (Type, Type?)>();
        private static string _mappingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "source/repos/Gesdent/VsProject/Resources/Data", "ViewModelViewMappings.xml");

        public static void Initialize()
        {
            // Load mappings from XML file
            if (!File.Exists(_mappingsFile))
            {
                GenerateFromFolder();
            }

            XDocument doc = XDocument.Load(_mappingsFile);
            XNamespace vmNamespace = doc.Root.GetNamespaceOfPrefix("vm");
            XNamespace vNamespace = doc.Root.GetNamespaceOfPrefix("v");

            foreach (XElement mappingElement in doc.Root.Elements("Mapping"))
            {
                string viewModelName = mappingElement.Element("ViewModel")?.Value;
                string viewName = mappingElement.Element("View")?.Value;
                string parentName = mappingElement.Element("Parent")?.Value;

                if (!string.IsNullOrEmpty(viewModelName) && !string.IsNullOrEmpty(viewName))
                {
                    viewModelName = viewModelName.Replace("vm:", $"{vmNamespace.NamespaceName}.");
                    viewName = viewName.Replace("v:", $"{vNamespace.NamespaceName}.");

                    Type viewModelType = Type.GetType(viewModelName);
                    Type viewType = Type.GetType(viewName);
                    Type parentType = null;

                    if (!string.IsNullOrEmpty(parentName))
                    {
                        parentName = parentName.Replace("v:", $"{vNamespace.NamespaceName}.");
                        parentType = Type.GetType(parentName);
                    }

                    if (viewModelType != null && viewType != null)
                    {
                        _mappings[viewModelType] = (viewType, parentType);
                    }
                }
            }
        }



        public static Type GetViewType(Type viewModelType)
        {
            if (_mappings.TryGetValue(viewModelType, out (Type viewType, Type?) mapping))
            {
                return mapping.viewType;
            }
            else
            {
                throw new InvalidOperationException($"No view type registered for view model type {viewModelType}");
            }
        }

        public static Type? GetParentType(Type viewModelType)
        {
            if (_mappings.TryGetValue(viewModelType, out (Type, Type? parentType) mapping))
            {
                return mapping.parentType;
            }
            else
            {
                return null; // No parent type registered
            }
        }

        public static (Type, Type?) GetViewAndParentTypes(Type viewModelType)
        {
            if (_mappings.TryGetValue(viewModelType, out (Type, Type? ) mapping))
            {
                return mapping;
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
                writer.WriteAttributeString("xmlns:vm", "clr-namespace:VsProject.ViewModels");
                writer.WriteAttributeString("xmlns:v", "clr-namespace:VsProject.Views");

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
                        writer.WriteElementString("ViewModel", $"vm:{viewModelName}");
                        writer.WriteElementString("View", $"v:{viewName}");

                        string parentName = viewModelName.Replace("ViewModel", "Model");
                        string parentFile = Path.Combine(viewModelFolder, parentName + ".cs");
                        if (File.Exists(parentFile))
                        {
                            string parentViewModelName = parentName.Replace("Model", "ViewModel");
                            writer.WriteElementString("Parent", $"v:{parentViewModelName}");
                        }

                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }

}
