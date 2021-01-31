using System;

namespace WcfService.Models
{
    /// <summary>
    /// Модель чека
    /// </summary>
    public class Cheque
    {
        /// <summary>
        /// Получет или задает идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Получет или задает номер
        /// </summary>       
        public string Number { get; set; }

        /// <summary>
        /// Получет или задает сумму
        /// </summary>        
        public decimal Summ { get; set; }

        /// <summary>
        /// Получет или задает скидку
        /// </summary>        
        public decimal Discount { get; set; }

        /// <summary>
        /// Получет или задает артикли
        /// </summary>        
        public string[] Articles { get; set; }
    }
}
