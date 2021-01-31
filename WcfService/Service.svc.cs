using System.Collections.Generic;
using WcfService.Models;
using WcfService.Models.Repositories;

namespace WcfService
{
    /// <summary>
    /// Сервис
    /// </summary>
    public class Service : IService
    {
        /// <summary>
        /// Репозиторий чеков
        /// </summary>
        private IChequeRepository chequeRepository;

        /// <summary> 
        /// Инициализирует новый экземпляр класса <see cref="Service" /> 
        /// </summary>
        public Service()
        {
            if (Properties.Settings.Default.IsFakeRepository == true)
                this.chequeRepository = new FakeChequeRepository();
            else
                this.chequeRepository = new ChequeRepository(Properties.Settings.Default.ConnectionString);

        }

        /// <summary>
        /// Полученияе списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Список чеков</returns>
        public List<Cheque> GetChequesPack(int packSize) => chequeRepository.GetChequesPack(packSize);

        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        public void SaveCheque(Cheque cheque) => chequeRepository.SaveCheque(cheque);
    }
}
