namespace ClientesWebService.Models
{
    public class OutBook
    {
        public int Id_cliente { get; set; } = 0;

        public string NombreCompleto { get; set; } = string.Empty;
        public string FechaSalida { get; set; } = string.Empty;

        public List<Stock> UserBooks { get; set; } = new List<Stock>();

        public bool Procesed { get; set; } = false;

        public OutBook() { }


    }
}
