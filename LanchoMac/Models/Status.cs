namespace LanchoMac.Models
{
    public class Status
    {
        public int Id { get; set; }
        public StoreStatus StatusLoja { get; set; }


        public enum StoreStatus
        {
            Aberto,
            Fechado
        }

    }
}
