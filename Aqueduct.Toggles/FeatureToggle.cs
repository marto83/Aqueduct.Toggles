using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles
{
    public class FeatureConfiguration
    {
        private IList<FeatureToggle> _features = new List<FeatureToggle>();

        internal void LoadFromConfiguration(FeatureToggleConfigurationSection config)
        {
            _features = config.Features.Cast<FeatureToggleConfigurationElement>().Select(FeatureToggle.FromConfig).ToList();
        }

        public IEnumerable<FeatureToggle> AllFeatures
        {
            get { return _features; }
        }

        public IEnumerable<FeatureToggle> EnabledFeatures
        {
            get { return _features.Where(x => x.Enabled); }
        }

        public bool IsEnabled(string name)
        {
            var feature = GetFeature(name);

            return feature != null && feature.Enabled;
        }

        public FeatureToggle GetFeature(string name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }
    }

    public class FeatureToggle
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }

        private string _languages;
        internal string Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                UpdateLanguagesList(value);
            }
        }

        private void UpdateLanguagesList(string value)
        {
            var languages = !string.IsNullOrEmpty(value) ? value.Split(',') : Enumerable.Empty<string>();

            LanguagesList = languages.Select(x =>x.Trim().ToLower()).ToList();
        }

        internal List<string> LanguagesList { get; set; } 


        public bool EnabledForLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return false;

            return LanguagesList.Count == 0 || LanguagesList.Contains(language.ToLower());
        }

        public IList<LayoutToggle> Templates { get; private set; }
        public IList<LayoutToggle> Items { get; private set; }

        public IList<RenderingToggle> Renderings { get; private set; }

        public FeatureToggle()
        {
            Templates = new List<LayoutToggle>();
            Items = new List<LayoutToggle>();
            Renderings = new List<RenderingToggle>();
            LanguagesList = new List<string>();
        }

        public static FeatureToggle FromConfig(FeatureToggleConfigurationElement element)
        {
            Contract.Assert(element != null);

            var feature = new FeatureToggle();
            feature.Enabled = element.Enabled;
            feature.Languages = element.Languages;
            feature.Name = element.Name;

            feature.Renderings =
                element.Renderings.Cast<FeatureRenderingConfigurationElement>()
                    .Select(RenderingToggle.FromConfig)
                    .ToList();
            feature.Templates =
                element.Templates.Cast<BaseFeatureToggleLayoutConfigurationElement>()
                    .Select(LayoutToggle.FromConfig)
                    .ToList();
            feature.Items =
                element.Items.Cast<BaseFeatureToggleLayoutConfigurationElement>()
                    .Select(LayoutToggle.FromConfig)
                    .ToList();

            return feature;
        }

    }

    public class RenderingToggle
    {
        public string Name { get; set; }
        public Guid Original { get; set; }
        public Guid New { get; set; }

        public static RenderingToggle FromConfig(FeatureRenderingConfigurationElement element)
        {
            return new RenderingToggle
            {
                Name = element.Name,
                Original = element.Original,
                New = element.New
            };
        }
    }

    public class LayoutToggleRendering
    {
        public string Name { get; set; }
        public string PlaceHolder { get; set; }
        public Guid SublayoutId { get; set; }

        internal static LayoutToggleRendering FromConfing(FeatureToggleLayoutRenderingConfigurationElement element)
        {
            return new LayoutToggleRendering
            {
                Name = element.Name,
                PlaceHolder = element.Placeholder,
                SublayoutId = element.SublayoutId
            };
        }
    }

    public class LayoutToggle
    {
        public Guid Id { get; set; }
        public Guid? New { get; set; }
        public IList<LayoutToggleRendering> Renderings { get; private set; }

        public LayoutToggle()
        {
            Renderings = new List<LayoutToggleRendering>();
        }

        internal static LayoutToggle FromConfig(BaseFeatureToggleLayoutConfigurationElement element)
        {
            return new LayoutToggle()
            {
                Id = element.Id,
                New = element.New,
                Renderings = element.Renderings.Cast<FeatureToggleLayoutRenderingConfigurationElement>().Select(LayoutToggleRendering.FromConfing).ToList()
            };
        }
    }
}
