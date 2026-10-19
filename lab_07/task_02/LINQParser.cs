using System;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;


namespace Lab07.Task02.ParserModule
{
    public static class LINQParser
    {
        public static void ReadActiveStudentSortByLastName()
        {
            string jsonString = File.ReadAllText("json/students.json");
            JArray jsonArray = JArray.Parse(jsonString);

            var activeStudents = 
                from s in jsonArray[0]["json_agg"] 
                where (bool?)s["is_active"] == true 
                orderby s["last_name"] 
                select new { FullName = $"{s["first_name"]} {s["last_name"]}" };

            foreach (var acs in activeStudents)
                Console.WriteLine(acs.FullName);
        }

        public static void UpdateMedicalCertificateExpireDate(int id, DateTime newExpireDate)
        {
            string jsonString = File.ReadAllText("json/medical_certificates.json");
            JArray jsonArray = JArray.Parse(jsonString);

            var targetCertificate = 
                (from mc in jsonArray[0]["json_agg"]
                where (int?)mc["id"] == id 
                select mc).FirstOrDefault();

            targetCertificate?["expire_date"] = newExpireDate;
            
            File.WriteAllText("json/medical_certificates.json", jsonArray.ToString());
        }

        public static void InsertNewCabinet(int cabinetNumber, int rowsQ, int colsQ)
        {
            string jsonString = File.ReadAllText("json/cabinets.json");
            JArray jsonArray = JArray.Parse(jsonString);
            JArray? cabinetsArray = (JArray?)jsonArray[0]["json_agg"];

            var sameNumberCabinet = 
                (from c in jsonArray[0]["json_agg"]
                where (int?)c["number"] == cabinetNumber
                select c).FirstOrDefault();

            if (sameNumberCabinet != null)
            {
                Console.WriteLine($"[ LINQParser ] Кабинет с номером {cabinetNumber} уже существует");
                return;
            }

            JObject newCabinet = new()
            {
              ["id"] = jsonArray[0]["json_agg"]?.Count(),
              ["number"] = cabinetNumber,
              ["rows_count"] = rowsQ,
              ["cols_count"] = colsQ  
            };

            cabinetsArray?.Add(newCabinet);
            File.WriteAllText("json/cabinets.json", jsonArray.ToString());
        }
    }
}