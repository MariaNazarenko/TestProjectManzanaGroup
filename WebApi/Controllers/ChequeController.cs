using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApi.ServiceReference;

namespace WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с чеками
    /// </summary>
    public class ChequeController : ApiController
    {
        ServiceClient client = new ServiceClient();
        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        [HttpPost]
        public void SaveCheque([FromBody] Cheque cheque)
        {
            client.SaveCheque(cheque);
        }
        /// <summary>
        /// Получение чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Список чеков</returns>
        [HttpGet]
        public List<Cheque> GetChequesPack(int packSize)
        {
            return client.GetChequesPack(packSize).ToList();
        }
    }
}
