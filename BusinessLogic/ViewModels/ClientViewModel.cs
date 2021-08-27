using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string ClientName { get; set; }

        [DisplayName("Логин")]
        public string ClientLogin { get; set; }

        [DisplayName("Пароль")]
        public string ClientPassword { get; set; }
    }
}
