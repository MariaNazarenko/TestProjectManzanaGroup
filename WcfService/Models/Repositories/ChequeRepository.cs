﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace WcfService.Models.Repositories
{
    /// <summary>
    /// Интерфейс рапозитория для работы с чеками
    /// </summary>
    public interface IChequeRepository
    {
        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        void SaveCheque(Cheque cheque);
        /// <summary>
        /// Получение списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Коллекция модели чека</returns>
        List<Cheque> GetChequesPack(int packSize);
    }
    /// <summary>
    /// Репозитоорий для работы с чеками
    /// </summary>
    public class ChequeRepository: IChequeRepository
    {
        private string connectionString;
        public ChequeRepository(string _connectionString)
        {
            connectionString = _connectionString;
        }
        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        public void SaveCheque(Cheque cheque)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("cheque_id", cheque.Id);
                dynamicParams.Add("cheque_number", cheque.Number);
                dynamicParams.Add("summ", cheque.Summ);
                dynamicParams.Add("discount", cheque.Discount);
                dynamicParams.Add("articles", String.Join(";", cheque.Articles));
                db.Execute("custom.save_cheques", dynamicParams, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// Получение списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Коллекция модели чека</returns>
        public List<Cheque> GetChequesPack(int packSize)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("pack_size", packSize);
                var result = db.Query<Cheque>("get_cheques_pack", dynamicParams, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }            
        }
    }
    /// <summary>
    /// Фейковый репозитоорий для работы с чеками
    /// </summary>
    public class FakeChequeRepository : IChequeRepository
    {
        /// <summary>
        /// Сохранение чека
        /// </summary>
        /// <param name="cheque">Чек</param>
        public void SaveCheque(Cheque cheque)
        {
            try
            {
                string path = "App_Data\\" + cheque.Number + ".txt";
                File.Create(path);
                using (var streamWriter = new StreamWriter(path))
                {
                    streamWriter.Write(cheque);
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Получение списка чеков
        /// </summary>
        /// <param name="packSize">Размер</param>
        /// <returns>Коллекция модели чека</returns>
        public List<Cheque> GetChequesPack(int packSize)
        {
            List<Cheque> cheques = new List<Cheque>();
            for(int i = 0; i <packSize; i++)
            {
                var cheque = new Cheque();
                cheque.Id = Guid.NewGuid();
                cheque.Number = i.ToString();
                cheque.Summ = 100;
                cheque.Discount = 10;
                cheque.Articles = new string[3]{ "12345", "123456", "1234567"};
                cheques.Add(cheque);
            }

            return cheques;
        }
    }
}