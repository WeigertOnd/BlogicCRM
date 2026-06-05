using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogicCRM.Models;

namespace BlogicCRM.Services
{
    public static class CsvExportService
    {
        private const char Sep = ';';

        static CsvExportService()
        {
            try { Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); } catch { }
        }

        private static string Escape(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var needsQuotes = value.Contains(Sep) || value.Contains('"') || value.Contains('\r') || value.Contains('\n');
            var escaped = value.Replace("\"", "\"\"");
            return needsQuotes ? '"' + escaped + '"' : escaped;
        }

        public static byte[] ToUtf8BomBytes(string text)
        {
            var enc = Encoding.UTF8;
            var pre = enc.GetPreamble();
            var bytes = enc.GetBytes(text);
            if (pre == null || pre.Length == 0)
            {
                return bytes;
            }
            var result = new byte[pre.Length + bytes.Length];
            Buffer.BlockCopy(pre, 0, result, 0, pre.Length);
            Buffer.BlockCopy(bytes, 0, result, pre.Length, bytes.Length);
            return result;
        }

        public static byte[] GenerateClientsCsv(IEnumerable<Client> clients)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jméno;Příjmení;E-mail;Telefon;Rodné číslo;Věk");
            foreach (var c in clients)
            {
                var phoneCell = string.IsNullOrEmpty(c.Phone) ? string.Empty : "=\"" + c.Phone + "\"";
                var birthCell = string.IsNullOrEmpty(c.BirthNumber) ? string.Empty : "=\"" + c.BirthNumber + "\"";
                var parts = new[] {
                    Escape(c.FirstName),
                    Escape(c.LastName),
                    Escape(c.Email),
                    Escape(phoneCell),
                    Escape(birthCell),
                    c.Age.ToString()
                };
                sb.AppendLine(string.Join(";", parts));
            }
            return ToUtf8BomBytes(sb.ToString());
        }

        public static byte[] GenerateAdvisorsCsv(IEnumerable<Advisor> advisors)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jméno;Příjmení;E-mail;Telefon;Rodné číslo;Věk");
            foreach (var a in advisors)
            {
                var phoneCell = string.IsNullOrEmpty(a.Phone) ? string.Empty : "=\"" + a.Phone + "\"";
                var birthCell = string.IsNullOrEmpty(a.BirthNumber) ? string.Empty : "=\"" + a.BirthNumber + "\"";
                var parts = new[] {
                    Escape(a.FirstName),
                    Escape(a.LastName),
                    Escape(a.Email),
                    Escape(phoneCell),
                    Escape(birthCell),
                    a.Age.ToString()
                };
                sb.AppendLine(string.Join(";", parts));
            }
            return ToUtf8BomBytes(sb.ToString());
        }

        public static byte[] GenerateContractsCsv(IEnumerable<Contract> contracts)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Evidenční číslo;Instituce;Klient;Správce;Datum uzavření;Datum platnosti;Datum ukončení;Stav");
            foreach (var c in contracts)
            {
                var clientName = c.Client != null ? $"{c.Client.FirstName} {c.Client.LastName}" : string.Empty;
                var managerName = c.ManagerAdvisor != null ? $"{c.ManagerAdvisor.FirstName} {c.ManagerAdvisor.LastName}" : string.Empty;
                string dateClosed = c.DateClosed.ToString("dd.MM.yyyy");
                string dateValid = c.DateValidFrom.ToString("dd.MM.yyyy");
                string dateEnded = c.DateEnded.HasValue ? c.DateEnded.Value.ToString("dd.MM.yyyy") : string.Empty;
                var isActive = !c.DateEnded.HasValue || c.DateEnded.Value > DateTime.Today;
                var status = isActive ? "Aktivní" : "Ukončená";

                var parts = new[] {
                    Escape(c.RegistrationNumber),
                    Escape(c.Institution),
                    Escape(clientName),
                    Escape(managerName),
                    Escape(dateClosed),
                    Escape(dateValid),
                    Escape(dateEnded),
                    Escape(status)
                };
                sb.AppendLine(string.Join(";", parts));
            }
            return ToUtf8BomBytes(sb.ToString());
        }

        public static byte[] GenerateSingleContractCsv(Contract c)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Evidenční číslo;Instituce;Klient;Správce;Účastníci;Datum uzavření;Datum platnosti;Datum ukončení;Stav");

            var clientName = c.Client != null ? $"{c.Client.FirstName} {c.Client.LastName}" : string.Empty;
            var managerName = c.ManagerAdvisor != null ? $"{c.ManagerAdvisor.FirstName} {c.ManagerAdvisor.LastName}" : string.Empty;
            var participants = c.ContractAdvisors != null ? string.Join(", ", c.ContractAdvisors.Select(ca => ca.Advisor != null ? $"{ca.Advisor.FirstName} {ca.Advisor.LastName}" : string.Empty)) : string.Empty;
            string dateClosed = c.DateClosed.ToString("dd.MM.yyyy");
            string dateValid = c.DateValidFrom.ToString("dd.MM.yyyy");
            string dateEnded = c.DateEnded.HasValue ? c.DateEnded.Value.ToString("dd.MM.yyyy") : string.Empty;
            var isActive = !c.DateEnded.HasValue || c.DateEnded.Value > DateTime.Today;
            var status = isActive ? "Aktivní" : "Ukončená";

            var parts = new[] {
                Escape(c.RegistrationNumber),
                Escape(c.Institution),
                Escape(clientName),
                Escape(managerName),
                Escape(participants),
                Escape(dateClosed),
                Escape(dateValid),
                Escape(dateEnded),
                Escape(status)
            };
            sb.AppendLine(string.Join(";", parts));
            return ToUtf8BomBytes(sb.ToString());
        }
    }
}
