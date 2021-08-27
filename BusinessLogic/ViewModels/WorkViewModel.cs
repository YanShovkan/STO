using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class WorkViewModel
    {
        public int Id { get; set; }

        public int WorkerId { get; set; }

        [DisplayName("Название")]
        public string WorkName { get; set; }

        [DisplayName("Цена")]
        public decimal WorkPrice { get; set; }
    }
}
