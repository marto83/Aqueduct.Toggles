<%@ Page language="c#" %>
<%@ Import Namespace="Aqueduct.Toggles" %>
<script runat="server">
    
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Sitecore.Context.User.IsAdministrator)
        {
            SetFeatureToggles();
            FeatureTogglePlaceholder.Visible = true;
        }
    }

    private void SetFeatureToggles()
    {
        var features = FeatureToggles.GetAllFeatures();

        FeatureToggleRepeater.DataSource = features;
        FeatureToggleRepeater.DataBind();
    }

</script>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
    <title>Feature Toggles</title>
  </head>
  <body>
      <asp:Placeholder ID="FeatureTogglePlaceholder" runat="server" Visible="False">
      <h1>Feature Toggles</h1>
      
      <p>Listed below is the list of feature toggles currently configured for this site.</p>
      
      <asp:Repeater ID="FeatureToggleRepeater" runat="server">
          <ItemTemplate>
              <table>
                  <thead>
                      <th>
                          <td><b>Name</b></td>
                          <td><b>Enabled?</b></td>
                      </th>
                  </thead>
                  <tr>
                      <td>
                          <%#Eval("Name")%>
                      </td>
                      <td>
                          <%#Eval("Enabled")%>
                      </td>
                  </tr>
              </table>
          </ItemTemplate>
      </asp:Repeater>

      </asp:Placeholder>
  </body>
</html>