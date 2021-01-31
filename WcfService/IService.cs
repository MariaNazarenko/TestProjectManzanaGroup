using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using WcfService.Models;

namespace WcfService
{
    [ServiceContract]
    public interface IService
    {
        /// <summary>
        /// Полученияе списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Список чеков</returns>
        [OperationContract]
        [WebGet]
        List<Cheque> GetChequesPack(int packSize);

        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        [OperationContract]
        void SaveCheque(Cheque cheque);

        [OperationContract]
        string Test();
    }
}
