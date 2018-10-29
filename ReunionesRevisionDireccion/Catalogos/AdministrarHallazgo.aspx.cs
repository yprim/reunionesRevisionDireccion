﻿using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReunionesRevisionDireccion.Catalogos
{
    public partial class AdministrarHallazgo : System.Web.UI.Page
    {
        #region variables globales
        ReuniónServicios ReunionServicios = new ReuniónServicios();
        #endregion

        #region pageload
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el usuario
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPeromitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPeromitidos);

            if (!Page.IsPostBack)
            {
                Session["listaReunion"] = null;
                Session["ReunionEditar"] = null;
                Session["ReunionEliminar"] = null;
                cargarDatosTblReunions();

            }
        }
        #endregion

        #region logica
        /// <summary>
        /// Priscilla Mena
        /// 26/09/2018
        /// Efecto: Metodo para llenar los datos de la tabla con las reuniones que se encuentran en la base de datos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void cargarDatosTblReunions()
        {
            List<Reunion> listaReunion = new List<Reunion>();
            listaReunion = ReunionServicios.getReuniones();
            rpReunion.DataSource = listaReunion;
            rpReunion.DataBind();

            Session["listaReunion"] = listaReunion;

        }
        #endregion

        #region eventos

        /// <summary>
        /// Priscilla Mena
        /// 26/09/2018
        /// Efecto: Metodo que redirecciona a la pagina donde se ingresa una nueva Reunion,
        /// se activa cuando se presiona el boton de nuevo
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Catalogos/NuevaReunion.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Priscilla Mena
        /// 26/09/2018
        /// Efecto: Metodo que redirecciona a la pagina donde se edita una Reunion,
        /// se activa cuando se presiona el boton de nuevo
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnReunionHallazgo_Click(object sender, EventArgs e)
        {
            int idReunion = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Reunion> listaReuniones = (List<Reunion>)Session["listaReunion"];

            Reunion reunionEditar = new Reunion();

            foreach (Reunion reunion in listaReuniones)
            {
                if (reunion.idReunion == idReunion)
                {
                    reunionEditar = reunion;
                    break;
                }
            }

            Session["ReunionEditar"] = reunionEditar;

            String url = Page.ResolveUrl("~/Catalogos/EditarReunion.aspx");
            Response.Redirect(url);


        }

       


        #endregion
    }
}