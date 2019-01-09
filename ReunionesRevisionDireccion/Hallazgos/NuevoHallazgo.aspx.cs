﻿using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReunionesRevisionDireccion.Catalogos
{
    public partial class NuevoHallazgo : System.Web.UI.Page
    {
        #region variables globales
        ReuniónServicios reunionServicios = new ReuniónServicios();
        ElementoRevisarServicios elementoRevisarServicios = new ElementoRevisarServicios();
        ReunionElementoRevisarServicios reunionElementoRevisarServicios = new ReunionElementoRevisarServicios();
        ArchivoReunionServicios archivoReunionServicios = new ArchivoReunionServicios();
        UsuarioServicios usuarioServicios = new UsuarioServicios();
        EstadoServicios estadoServicios = new EstadoServicios();
        ReunionUsuarioServicios reunionUsuarioServicios = new ReunionUsuarioServicios();
        HallazgoServicios hallazgoServicios = new HallazgoServicios();
        ReunionElementoRevisarHallazgoServicios reunionElementoRevisarHallazgoServicios = new ReunionElementoRevisarHallazgoServicios();
        #endregion

        #region pageload
        protected void Page_Load(object sender, EventArgs e)
        {
            //controla los menus q se muestran y las pantallas que se muestras segun el rol que tiene el ElementoRevisar
            //si no tiene permiso de ver la pagina se redirecciona a login
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!IsPostBack)
            {
                Reunion reunionHallazgos = (Reunion)Session["ReunionHallazgos"];
                Session["listaArchivosReunionAsociados"] = null;
                Session["listaElementos"] = null;
                Session["listaUsuarios"] = null;
                Session["elementoSeleccionado"] = null;
                Session["usuarioSeleccionado"] = null;

                //archivos
                List<ArchivoReunion> listaArchivosReunion = archivoReunionServicios.getArchivosReunionPorIdReunion(reunionHallazgos);
                Session["listaArchivosReunionAsociados"] = listaArchivosReunion;
                cargarArchivosReunion();

                 // datos de la reunion 
                txtAnno.Text = reunionHallazgos.anno.ToString();
                txtConsecutivo.Text = reunionHallazgos.consecutivo.ToString();
                txtMes.Text = reunionHallazgos.mes.ToString();
                txtTipos.Text = reunionHallazgos.tipo.descripcion;
                // fin datos de la reunion
                cargarDatosTblElementos();
                cargarDatosTblUsuarios();
                llenarDdlEstados();

                txtElementoSeleccionado.Attributes.Add("oninput", "validarTexto(this)");
                txtUsuarioSeleccionado.Attributes.Add("oninput", "validarTexto(this)");
                txtObservaciones.Attributes.Add("oninput", "validarTexto(this)");
                txtCodigoAccion.Attributes.Add("oninput", "validarTexto(this)");
            }

        }

        #endregion

        #region logica

        

        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:Sirve para cargar los elementos  que van a estar en la tabla del modal
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void cargarDatosTblElementos()
        {
            List<ElementoRevisar> listaElementos = new List<ElementoRevisar>();
            listaElementos = elementoRevisarServicios.getElementosRevisar();
            rpElemento.DataSource = listaElementos;
            rpElemento.DataBind();

            Session["listaElementos"] = listaElementos;
        }


        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:Sirve para cargar los usuarios que van a estar en el modal
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        private void cargarDatosTblUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            listaUsuarios = usuarioServicios.getUsuarios();
            rpUsuario.DataSource = listaUsuarios;
            rpUsuario.DataBind();

            Session["listaUsuarios"] = listaUsuarios;
        }

        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:Metodo que carga todos los estados en el DropDownList
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void llenarDdlEstados()
        {
            List<Estado> listaTipoes = estadoServicios.getEstados();
            ddlEstados.DataSource = listaTipoes;
            ddlEstados.DataTextField = "descripcionEstado";
            ddlEstados.DataValueField = "idEstado";
            ddlEstados.DataBind();
        }

        private void descargar(string fileName, Byte[] file)
        {
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.BinaryWrite(file);
            Response.Flush();
            Response.End();

        }

        /// <summary>
        /// Priscilla Mena
        /// 24/10/2018
        /// Efecto:Crea una tabla donde se muestran los archivos asociados a una Reunion
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void cargarArchivosReunion()
        {
            Reunion reunion = (Reunion)Session["ReunionHallazgos"];

            List<ArchivoReunion> listaArchivosReunion = (List<ArchivoReunion>)Session["listaArchivosReunionAsociados"];

            if (listaArchivosReunion.Count == 0)
            {
                txtArchivos.Text = "No hay archivos asociados a esta Reunion";
                txtArchivos.Visible = true;
                rpArchivos.Visible = false;
            }
            else
            {
                txtArchivos.Visible = false;
                rpArchivos.Visible = true;
            }

            rpArchivos.DataSource = listaArchivosReunion;
            rpArchivos.DataBind();
        }

        #endregion

        #region eventos

        /// <summary>
        /// Priscilla Mena
        /// 24/10/2018
        /// Efecto:descarga el archivo para que el usuario lo pueda ver
        /// Requiere: clic en el enlace al archivo
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnVerArchivo_Click(object sender, EventArgs e)
        {
            String[] infoArchivo = (((LinkButton)(sender)).CommandArgument).ToString().Split(',');
            String nombreArchivo = infoArchivo[1];
            String rutaArchivo = infoArchivo[2];

            FileStream fileStream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] blobValue = binaryReader.ReadBytes(Convert.ToInt32(fileStream.Length));

            fileStream.Close();
            binaryReader.Close();

            descargar(nombreArchivo, blobValue);
        }

       
        /// <summary>
        /// Priscilla Mena
        /// 09/01/2019
        /// Efecto: Metodo que valida los campos que debe ingresar el usuario
        /// Requiere:-
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public Boolean validarCampos()
        {
            Boolean validados = true;

            divElementoIncorrecto.Style.Add("display", "none");
            divUsuarioIncorrecto.Style.Add("display", "none");
            divObservacionesIncorrecto.Style.Add("display", "none");
            divCodigoAccionIncorrecto.Style.Add("display", "none");
            divFechaIncorrecta.Style.Add("display", "none");

            txtElementoSeleccionado.CssClass = "form-control";
            txtUsuarioSeleccionado.CssClass = "form-control";
            txtFecha.CssClass = "form-control";
            txtObservaciones.CssClass = "form-control";
            txtCodigoAccion.CssClass = "form-control";

            #region validacion elemento a revisar

            String elementoArevisar = txtElementoSeleccionado.Text;

            if (elementoArevisar.Trim() == "")
            {
                txtElementoSeleccionado.CssClass = "form-control alert-danger";
                divElementoIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion usuarios

            String usuario = txtUsuarioSeleccionado.Text;

            if (usuario.Trim() == "")
            {
                txtUsuarioSeleccionado.CssClass = "form-control alert-danger";
                divUsuarioIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region observaciones

            String observaciones = txtObservaciones.Text;

            if (observaciones.Trim() == "")
            {
                txtObservaciones.CssClass = "form-control alert-danger";
                divObservacionesIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion fecha

            String fecha = txtFecha.Text;

            if (fecha.Trim() == "")
            {
                txtFecha.CssClass = "form-control alert-danger";
                divFechaIncorrecta.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            #region validacion codigo accion

            String codigo = txtCodigoAccion.Text;

            if (codigo.Trim() == "")
            {
                txtCodigoAccion.CssClass = "form-control alert-danger";
                divCodigoAccionIncorrecto.Style.Add("display", "block");

                validados = false;
            }
            #endregion

            return validados;
        }

        /// <summary>
        /// Priscilla Mena
        /// 27/09/2018
        /// Efecto:Metodo que se activa cuando se le da click al boton de eliminar
        /// y guarda un registro en la base de datos
        /// redireccion a la pantalla de Administracion de Reuniones y Hallazgos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarCampos())
            {
                Usuario usuario = (Usuario)Session["usuarioSeleccionado"];
                ElementoRevisar elemento = (ElementoRevisar)Session["elementoSeleccionado"];
                Reunion reunion = (Reunion)Session["ReunionHallazgos"];
                Estado estado = new Estado();
                estado.idEstado = Convert.ToInt32(ddlEstados.SelectedValue);
                estado.descripcionEstado = ddlEstados.SelectedItem.Text;

                DateTime fecha = Convert.ToDateTime(txtFecha.Text);

                Hallazgo hallazgo = new Hallazgo();

                hallazgo.codigoAccion = txtCodigoAccion.Text;

                hallazgo.estado = estado;
                hallazgo.fechaMaximaImplementacion = fecha;
                hallazgo.usuario = usuario;
                hallazgo.observaciones = txtObservaciones.Text;

                int codigoHallazgo = hallazgoServicios.insertarHallazgo(hallazgo);

                hallazgo.idHallazgo = codigoHallazgo;

                reunionElementoRevisarHallazgoServicios.insertarReunionElementoHallazgo(reunion, elemento, hallazgo);

                String url = Page.ResolveUrl("~/Hallazgos/AdministrarHallazgo.aspx");
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Priscilla Mena
        /// 18/10/2018
        /// Efecto:Metodo que se activa cuando se le da click al boton de cancelar
        /// redireccion a la pantalla de Administracion de Reuniones y Hallazgos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Hallazgos/AdministrarHallazgo.aspx");
            Response.Redirect(url);
        }





        /// <summary>
        /// Priscilla Mena
        /// 18/10/2018
        /// Efecto:Metodo que se activa cuando se le da click al boton de cancelar
        /// redireccion a la pantalla de Administracion de Reuniones y Hallazgos
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            String url = Page.ResolveUrl("~/Hallazgos/AdministrarHallazgo.aspx");
            Response.Redirect(url);
        }

        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:Metodo que se activa cuando se le da click al enlace de elemntos a revisar
        /// Carga un modal con los elementos a revisar
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnElemento_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalElementos();", true);

         
        }

        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:Metodo que se activa cuando se le da click al enlace de usuarios responsables
        /// Carga un modal con los usuarios
        /// Requiere: -
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnUsuario_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "activar", "activarModalUsuarios();", true);
        }


     
        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:se encarga de llenar el textbox con el nombre del elemento seleccionado
        /// además guarda ese elemento en memoria
        /// Requiere: clic en el boton de "✓"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnSeleccionarElemento_Click(object sender, EventArgs e)
        {

            int idElementoRevisar = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<ElementoRevisar> listaElementos = (List<ElementoRevisar>)Session["listaElementos"];

            ElementoRevisar elementoSeleccionar = new ElementoRevisar();

            foreach (ElementoRevisar elemento in listaElementos)
            {
                if (elemento.idElemento == idElementoRevisar)
                {
                    elementoSeleccionar = elemento;
                    break;
                }
            }

            txtElementoSeleccionado.Text = elementoSeleccionar.descripcionElemento;
        
            Session["elementoSeleccionado"] = elementoSeleccionar;
         

        }


        /// <summary>
        /// Priscilla Mena
        /// 29/11/2018
        /// Efecto:se encarga de llenar el textbox con el nombre del usuario seleccionado
        /// además guarda ese usuario en memoria
        /// Requiere: clic en el boton de "✓"
        /// Modifica: -
        /// Devuelve: -
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        protected void btnSeleccionarUsuario_Click(object sender, EventArgs e)
        {
            int idUsuario = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Usuario> listaUsuarios = (List<Usuario>)Session["listaUsuarios"];

            Usuario UsuarioSeleccionar = new Usuario();

            foreach (Usuario Usuario in listaUsuarios)
            {
                if (Usuario.idUsuario == idUsuario)
                {
                    UsuarioSeleccionar = Usuario;
                    break;
                }
            }

            txtUsuarioSeleccionado.Text = UsuarioSeleccionar.nombre;
            Session["usuarioSeleccionado"] = UsuarioSeleccionar;
        }

        #endregion

    }




}