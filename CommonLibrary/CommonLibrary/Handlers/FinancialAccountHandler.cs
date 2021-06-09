using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public static class FinancialAccountHandler
    {
        public static string GetFinancialInstitutionCode(string bsb)
        {
            string original = bsb;
            bsb = bsb.Replace("-", "");

            if (bsb.Length == 6)
            {
                bsb = bsb.Substring(0, 3);
            }
            else if (bsb.Length != 3)
            {
                throw new Exception($"The BSB {original} isn't valid");
            }

            if (!int.TryParse(bsb, out int bsbNumber))
            {
                throw new Exception($"The BSB {original} isn't valid");
            }

            string financialNumber = bsb.Substring(0, 2);
            string branch = bsb.Substring(2, 1);

            return GetFinancialCode(financialNumber, branch);
        }

        private static string GetFinancialCode(string financialNumber, string branch)
        {
            switch (financialNumber)
            {
                // Australia and New Zealand Banking Group
                case "01":
                    return "ANZ";

                // Westpac Banking Corporation
                case "03":
                case "73":
                    return "WBC";

                // Commonwealth Bank of Australia
                case "06":
                case "76":
                    return "CBA";

                // National Australia Bank
                case "08":
                case "78":
                    return "NAB";

                // Reserve Bank of Australia
                case "09":
                    return "RBA";

                // BankSA (division of Westpac Bank)
                case "10":
                    return "BSA";

                // St George Bank (division of Westpac Bank)
                case "11":
                    return "STG";
                case "33":
                    return "SGP";

                // Bank of Queensland
                case "12":
                    return "BQL";

                // Rabobank
                case "14":
                    return "PIB";

                // Town & Country Bank
                case "15":
                    return "T&C";

                // Macquarie Bank
                case "18":
                    return "MBL";

                // Bank of Melbourne (division of Westpac Bank)
                case "19":
                    return "BOM";
                case "55":
                    return "BML";

                // JP Morgan Chase Bank
                case "21":
                    return "CMB";

                // BNP Paribas
                case "22":
                    return "BNP";

                // Bank of America
                case "23":
                    return "BAL";

                // Citibank & Citibank NA
                case "24":
                    return "CTI";

                // BNP Paribas Securities
                case "25":
                    return "BPS";

                // Bankers Trust Australia (division of Westpac Bank)
                case "26":
                    return "BTA";

                // MUFG Bank
                case "29":
                    return "BOT";

                // Bankwest (division of Commonwealth Bank)
                case "30":
                    return "BWA";

                // Bank Australia
                case "31":
                    return "MCU";

                // Beyond Bank
                case "32":
                    if (branch == "5")
                    {
                        return "BYB";
                    }
                    throw new Exception("Unkown Financial Institution");

                // HSBC Bank Australia && Bank of China
                case "34":
                    return "HBA";
                case "98":
                    if (branch == "5")
                    {
                        return "HSB";
                    }
                    if (branch == "0")
                    {
                        return "BCA";
                    }
                    throw new Exception("Unkown Financial Institution");
                case "35":
                    return "BOC";

                // Commonwealth Bank of Australia
                case "40":
                    return "CST";

                // Deutsche Bank
                case "41":
                    return "DBA";

                // Commonwealth Bank of Australia
                case "42":
                    return "TBT";
                case "52":
                    return "TBT";

                // OCBC Bank
                case "45":
                    return "OCB";

                // Advance Bank (division of Westpac Bank)
                case "46":
                    return "ADV";

                // Challenge Bank (division of Westpac Bank)
                case "47":
                    return "CBL";

                // Suncorp-Metway
                case "48":
                    return "MET";
                case "66":
                    if (branch == "4")
                    {
                        return "SUN";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "51":
                    if (branch == "0") // Citibank NA
                    {
                        return "CNA";
                    }
                    if (branch == "2") // Community First Credit Union
                    {
                        return "CFC";
                    }
                    if (branch == "4") // RACQ Bank
                    {
                        return "QTM";
                    }
                    if (branch == "7") // Volt Bank
                    {
                        return "VOL";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Bananacoast Community Credit Union
                case "53":
                    if (branch == "3")
                    {
                        return "BCC";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Australian Settlements
                case "57":
                    return "ASL";

                // Adelaide Bank (division of Bendigo and Adelaide Bank)
                case "61":
                    if (branch == "1") // Endeavour Mutual Bank 611 ... WHAT
                    {
                        return "SEL";
                    }
                    return "ADL";

                // Variety of banks
                case "63":
                    if (branch == "0") // Greater Bank
                    {
                        return "ABS";
                    }
                    if (branch == "7") // Greater Bank
                    {
                        return "GBS";
                    }
                    if (branch == "2") // B&E
                    {
                        return "BAE";
                    }
                    if (branch == "3") // Bendigo Bank
                    {
                        return "BBL";
                    }
                    if (branch == "4") // Uniting Financial Services
                    {
                        return "UFS";
                    }
                    if (branch == "8") // Heritage Bank
                    {
                        return "HBS";
                    }
                    if (branch == "9") // Home Building Society (division of Bank of Queensland)
                    {
                        return "HOM";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "64":
                    if (branch == "0") // Hume Bank
                    {
                        return "HUM";
                    }
                    if (branch == "1") // IMB
                    {
                        return "IMB";
                    }
                    if (branch == "7") // IMB
                    {
                        return "AUB";
                    }
                    if (branch == "2") // Australian Military Bank
                    {
                        return "ADC";
                    }
                    if (branch == "5") // Auswide Bank
                    {
                        return "MPB";
                    }
                    if (branch == "6") // Maitland Mutual Building Society
                    {
                        return "MMB";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "65":
                    if (branch == "6") // Auswide Bank
                    {
                        return "BAY";
                    }
                    if (branch == "0") // Newcastle Permanent Building Society
                    {
                        return "NEW";
                    }
                    if (branch == "3") // Pioneer Permanent Building Society (division of Bank of Queensland)
                    {
                        return "PPB";
                    }
                    if (branch == "4") // Queensland Country Credit Union
                    {
                        return "ECU";
                    }
                    if (branch == "5") // The Rock
                    {
                        return "ROK";
                    }
                    if (branch == "9") // G&C Mutual Bank
                    {
                        return "GCB";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Gateway Bank && Cuscal
                case "67":
                    if (branch == "6") // Gateway Bank
                    {
                        return "GTW";
                    }
                    if (branch == "0") // Cuscal
                    {
                        return "YOU";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Indue
                case "70":
                    return "CUS";

                // Variety of banks
                case "72":
                    if (branch == "1") // Holiday Coast Credit Union
                    {
                        return "HCC";
                    }
                    if (branch == "2") // Southern Cross Credit Union
                    {
                        return "SNX";
                    }
                    if (branch == "3") // Bank of Heritage Isle
                    {
                        return "HIC";
                    }
                    if (branch == "4") // MOVE
                    {
                        return "RCU";
                    }
                    if (branch == "8") // Summerland Credit Union
                    {
                        return "SCU";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Police & Nurse
                case "77":
                    if (branch == "7")
                    {
                        return "PNB";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Cuscal
                case "80":
                    return "CRU";

                // Variety of banks
                case "81":
                    if (branch == "2") // Teachers Mutual Bank
                    {
                        return "TMB";
                    }
                    if (branch == "3") // Capricornian
                    {
                        return "CAP";
                    }
                    if (branch == "4") // Credit Union Australia
                    {
                        return "CUA";
                    }
                    if (branch == "5") // Police Bank
                    {
                        return "PCU";
                    }
                    if (branch == "7") // Warwick Credit Union
                    {
                        return "WCU";
                    }
                    if (branch == "8") // Bank of Communications
                    {
                        return "COM";
                    }
                    if (branch == "9") // Industrial & Commercial Bank of China
                    {
                        return "IBK";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "82":
                    if (branch == "3") // Endeavour Mutual Bank
                    {
                        return "SEL";
                    }
                    if (branch == "4") // Sutherland Credit Union
                    {
                        return "STH";
                    }
                    if (branch == "5") // Big Sky Building Society
                    {
                        return "SKY";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "83":
                    if (branch == "3") // Defence Bank
                    {
                        return "DBL";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "88":
                    if (branch == "0") // Heritage Bank
                    {
                        return "HOM";
                    }
                    if (branch == "2") // Unity Bank
                    {
                        return "HOM";
                    }
                    if (branch == "8") // China Construction Bank
                    {
                        return "HOM";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Australia Post
                case "90":
                    return "APO";

                // Variety of banks
                case "91":
                    if (branch == "1") // Sumitomo Mitsui Banking Corporation
                    {
                        return "SMB";
                    }
                    if (branch == "3") // State Street Bank & Trust Company
                    {
                        return "SSB";
                    }
                    if (branch == "7") // Arab Bank Australia
                    {
                        return "ARA";
                    }
                    if (branch == "8") // Mizuho Bank
                    {
                        return "MCB";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "92":
                    if (branch == "2") // United Overseas Bank
                    {
                        return "UOB";
                    }
                    if (branch == "3") // ING Bank
                    {
                        return "ING";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "93":
                    if (branch == "1") // Mega International Commercial Bank
                    {
                        return "ICB";
                    }
                    if (branch == "2") // Regional Australia Bank
                    {
                        return "RAB";
                    }
                    if (branch == "6") // ING Bank
                    {
                        return "GNI";
                    }
                    if (branch == "9") // AMP Bank
                    {
                        return "AMP";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "94":
                    if (branch == "1") // Delphi Bank (division of Bendigo and Adelaide Bank)
                    {
                        return "BCY";
                    }
                    if (branch == "2") // Bank of Sydney
                    {
                        return "LBA";
                    }
                    if (branch == "3") // Taiwan Business Bank
                    {
                        return "TBB";
                    }
                    if (branch == "4") // Members Equity Bank
                    {
                        return "MEB";
                    }
                    if (branch == "6") // UBS AG
                    {
                        return "UBS";
                    }
                    throw new Exception("Unkown Financial Institution");

                // Variety of banks
                case "96":
                    if (branch == "9") // Tyro Payments
                    {
                        return "MSL";
                    }
                    throw new Exception("Unkown Financial Institution");

            }
            throw new NotImplementedException();
        }
    }
}
