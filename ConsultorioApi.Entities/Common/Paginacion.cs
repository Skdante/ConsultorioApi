namespace ConsultorioApi.Entities
{
    public class Paginacion
    {
        public int Pagina { get; set; }
        public int CantidadRegistros { get; set; }

        public Paginacion()
        {
            Pagina = 1;
            CantidadRegistros = 100;
        }
    }
}
