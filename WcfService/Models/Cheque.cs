using System;

namespace WcfService.Models
{
    /// <summary>
    /// Модель чека
    /// </summary>
    public class Cheque
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Summ { get; set; }
        /// <summary>
        /// Скидка
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Артикли
        /// </summary>
        public string[] Articles { get; set; }
    }
}
