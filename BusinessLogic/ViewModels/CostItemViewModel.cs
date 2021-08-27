using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLogic.ViewModels
{
    public class CostItemViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название")]
        public string CostItemName { get; set; }

        [DisplayName("Количество")]
        public int CostItemCount { get; set; }
    }
}
