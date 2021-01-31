using System.Collections.Generic;
using WcfService.Models;
using WcfService.Models.Repositories;

namespace WcfService
{
    public class Service : IService
    {
        IChequeRepository chequeRepository;
        public Service()
        {
            if (Properties.Settings.Default.IsFakeRepository == true)
                chequeRepository = new FakeChequeRepository();
            else
                chequeRepository = new ChequeRepository(Properties.Settings.Default.ConnectionString);
        }
        /// <summary>
        /// Полученияе списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Список чеков</returns>
        public List<Cheque> GetChequesPack(int packSize)=>chequeRepository.GetChequesPack(packSize);
        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        public void SaveCheque(Cheque cheque)=> chequeRepository.SaveCheque(cheque);
    }
}
