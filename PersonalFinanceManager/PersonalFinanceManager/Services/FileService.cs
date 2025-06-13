using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Services
{
    public class FileService
    {
        public void ExportToXml(List<Transaction> transactions, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Transaction>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, transactions);
            }
        }

        public List<Transaction> ImportFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Transaction>));
            using (var reader = new StreamReader(filePath))
            {
                return (List<Transaction>)serializer.Deserialize(reader);
            }
        }
    }
}