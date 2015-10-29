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
      <asp:PlaceHolder ID="FeatureTogglePlaceholder" runat="server" Visible="False">
      <h1>Feature Toggles</h1>
      
      <p>Listed below is the list of feature toggles currently configured for this site.</p>
      
      <asp:Repeater ID="FeatureToggleRepeater" runat="server">
        <HeaderTemplate>
          <table>
                  <thead>
                      <tr>
                          <th align="left"><b>Name</b></th>
                          <th align="left"><b>Enabled?</b></th>
                      </tr>
                  </thead>
        </HeaderTemplate>
        <ItemTemplate>
                <tr>
                    <td align="left">
                        <%#Eval("Name")%>
                    </td>
                    <td align="left">
                        <%#Eval("Enabled")%>
                    </td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
      </asp:Repeater>

      </asp:PlaceHolder>
  </body>
</html>