﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ayuda.aspx.cs" Inherits="ReunionesRevisionDireccion.Ayuda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="divRedondo">
        <div class="row">
            <%-- titulo pantalla --%>
            <div style="text-align: center">
                <asp:Label ID="lblAyuda" runat="server" Text="Ayuda" Font-Size="Large" ForeColor="Black"></asp:Label>
                <%-- fin titulo pantalla --%>
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                <hr />
            </div>

            <div class="col-md-12 col-xs-12 col-sm-12 col-lg-12">
                <asp:Label ID="lblTexto" runat="server" Text="Cualquier inconveniente contactar con los admnistradores del sistema al teléfono 2511-2503/2511-2536" Font-Size="Large" ForeColor="Black"></asp:Label>
            </div>

        </div>
    </div>
</asp:Content>