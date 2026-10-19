using Lab07.Task02.ParserModule;
using Lab07.Task02.ConsoleInputModule;


namespace Lab07.Task02.Program
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== Подзадание №1 ===");
            LINQParser.ReadActiveStudentSortByLastName();

            Console.WriteLine("\n=== Подзадание №2 ===");
            Console.Write("Введите id медицинского сертификата для изменения: ");
            int medicalCertificateId = ConsoleInput.GetInt();
            Console.Write("Введите дату истечения медицинской справки: ");
            DateTime newExpiredDate = ConsoleInput.GetDateTime();
            LINQParser.UpdateMedicalCertificateExpireDate(medicalCertificateId, newExpiredDate);

            Console.WriteLine("\n=== Подзадание №3 ===");
            Console.Write("Введите номер нового кабинета: ");
            uint newCabinetNumber = ConsoleInput.GetUint();
            Console.Write("Введите кол-во рядов парт в кабинете: ");
            uint newCabinetRowsQ = ConsoleInput.GetUint();
            Console.Write("Введите кол-во линий парт в кабинете: ");
            uint newCabinetColsQ = ConsoleInput.GetUint();
            LINQParser.InsertNewCabinet((int)newCabinetNumber, (int)newCabinetRowsQ, (int)newCabinetColsQ);
        }
    }
}