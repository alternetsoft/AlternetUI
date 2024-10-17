<%@ Page Language="C#" %>
<script runat="server">
void Button1_Click(Object sender, EventArgs e) 
{ 
    Label1.Text = "Clicked at " + DateTime.Now.ToString(); 
}
</script>
<html>
<head>
  <title>Single-File Page Model</title>
</head>
<body>
  <form runat="server">
    <div>
       <asp:Label id="Label1" 
         runat="server" Text="Label">
       </asp:Label>
       <br />
       <asp:Button id="Button1" 
         runat="server" 
         onclick="Button1_Click" 
         Text="Button">
      </asp:Button>
    </div>
  </form>
</body>
</html>