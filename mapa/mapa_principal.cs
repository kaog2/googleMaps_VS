using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;


namespace mapa
{
    public partial class mapa_principal : Form
    {

        GMarkerGoogle marker;
        GMapOverlay markerOverlay;
        DataTable dt; // almacenar direcciones

        int filaSeleccionada = 0;
        //Coordenadas iniciales (obtener por GPS)
        double LatInicial = -35.4263992;
        double LngInicial = -71.6554184
            ;


        public mapa_principal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Descripcion",typeof(string)));
            dt.Columns.Add(new DataColumn("Lat", typeof(string)));
            dt.Columns.Add(new DataColumn("Long", typeof(string)));

            // Insertar datos al dt para mostrar en la lista

            dt.Rows.Add("Ubicacion 1", LatInicial, LngInicial);
            dataGridView1.DataSource = dt;

            //desactivar las columnas de lat y long

            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            Mapa.DragButton = MouseButtons.Left; // to move maps with mause left click
            Mapa.CanDragMap = true; // to drag map
            Mapa.MapProvider = GMapProviders.GoogleMap;
            Mapa.Position = new PointLatLng(LatInicial, LngInicial);
            Mapa.MinZoom = 0;
            Mapa.MaxZoom = 24;
            Mapa.Zoom = 9;
            Mapa.AutoScroll = true; // scroll for the maps

            // Marker

            markerOverlay = new GMapOverlay("Marcador"); //agrega un marcador en una capa encima del mapa que generamos
            marker = new GMarkerGoogle(new PointLatLng(LatInicial,LngInicial),GMarkerGoogleType.green);
            markerOverlay.Markers.Add(marker); //to add to maps

            //agregar un tooltip de texto a los marcadores
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = string.Format("Ubicacion: \n Latitud:{0} \n Longitud: {1}", LatInicial, LngInicial);

            //ahora agregaremos al mapa el marcador al mapa control
            Mapa.Overlays.Add(markerOverlay);


         
        }

        private void SeleccionarRegistro(object sender, DataGridViewCellMouseEventArgs e)
        {
            filaSeleccionada = e.RowIndex; //Fila seleccionada
            // Recuperamos los datos del grid y los asignamos a los textbox
            txtdescripcion.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
            txtlatitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString(); 
            txtlongitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString();

            // se asignan los valores del grid al marcador

            marker.Position = new PointLatLng(Convert.ToDouble(txtlatitud.Text), Convert.ToDouble(txtlongitud.Text));

            // se posiciona en el foco del mapa en ese punto
            Mapa.Position = marker.Position;

        }

        private void Mapa_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // se obtienen los datos de lat y lng del mapa donde el User presiona
            double lat = Mapa.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = Mapa.FromLocalToLatLng(e.X, e.Y).Lng;

            // se posicionan en el txt de la lat y long

            txtlatitud.Text = lat.ToString();
            txtlongitud.Text = lng.ToString();

            // Creamos el marcador para moverlo al lugar indicado

            marker.Position = new PointLatLng(lat, lng);
            //Tambien se agrega el mensaje al marcador tooltip

            marker.ToolTipText = string.Format("Ubicacion: \n Latitud:{0} \n Longitud: {1}", lat, lng);

        }

        private void addButton(object sender, EventArgs e)
        {
            dt.Rows.Add(txtdescripcion.Text, txtlatitud.Text, txtlongitud.Text);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(filaSeleccionada);
        }
    }
}
