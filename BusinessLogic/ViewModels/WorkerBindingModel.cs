using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class WorkerViewModel
    {
        public int Id { get; set; }

        [DisplayName("Имя")]
        public string WorkerName { get; set; }

        [DisplayName("Логин")]
        public string WorkerLogin { get; set; }

        [DisplayName("Пароль")]
        public string WorkerPassword { get; set; }
    }
}
