Aqueduct.Toggles
================

A feature toggling library for use with Sitecore
   
Usage
----
This will all be in a Nuget file eventually, but for now you'll need to configure it manually!

Add the section definition to your Web.config

```xml
<configSections>
  <section name="featureToggles" type="Aqueduct.Toggles.Configuration.FeatureToggleConfigurationSection, Aqueduct.Toggles" />
</configSections>
```

Add the featureToggles section. I've encluded some examples of how to configure it.

```xml
<featureToggles>
  <feature name="CodeOnlyToggleExample" enabled="true" />
  <feature name="SwapWholeLayoutForItemExample" enabled="true">
    <layout item="<item id goes here>" new="<new layout id goes here>">
      <sublayout name="FirstRendering" placeholder="APlaceholderKey" id="<sublayout 1 id goes here>" />
      <sublayout name="SecondRendering" placeholder="AnotherPlaceholderKey" id="<sublayout 2 id goes here>" />
    </layout>
  </feature>
  <feature name="SwapWholeLayoutForTemplateExample" enabled="true">
    <layout template="<template id goes here>" new="<new layout id goes here>">
      <sublayout name="FirstRendering" placeholder="APlaceholderKey" id="<sublayout 1 id goes here>" />
      <sublayout name="SecondRendering" placeholder="AnotherPlaceholderKey" id="<sublayout 2 id goes here>" />
    </layout>
  </feature>
  <feature name="SwapSpecificRenderingsForAlternatives" enabled="true">
    <sublayouts>
      <sublayout name="NewLayout1" original="<id for old rendering 1 goes here>" new="<id for new rendering 1 goes here>"
      <sublayout name="NewLayout2" original="<id for old rendering 2 goes here>" new="<id for new rendering 2 goes here>"
    </sublayouts>
  </feature>
</featureToggles>
```

Find the pipelines section, then the httpRequestBegin element and add the new LayoutResolver. Add it immediately after the Sitecore Layout Resolver.

```xml
<httpRequestBegin>
  ...
  <processor type="Sitecore.Pipelines.HttpRequest.LayoutResolver, Sitecore.Kernel" />
  <processor type="Aqueduct.Toggles.Sitecore.LayoutResolver, Aqueduct.Toggles" />
  ...
</httpRequestBegin>
```

Still in the pipelines section, find the renderLayout element and add the new RenderLayoutProcessor. Add it immediately after the Sitecore InsertRenderings processor.

```xml
<renderLayout>
  ...
  <processor type="Sitecore.Pipelines.RenderLayout.InsertRenderings, Sitecore.Kernel" />
  <processor type="Aqueduct.Toggles.Sitecore.RenderLayoutProcessor, Aqueduct.Toggles" />
  ...
</renderLayout>
```

