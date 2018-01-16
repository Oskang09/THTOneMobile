using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THTOneMobile.Classes
{
    class StockItem
    {
        string filepath = Application.StartupPath;

        public string brand { get; set; }
        public string model { get; set; }
        public string stats { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }

        public void addStockItem()
        {
            string itempath = "";
            if (this.brand != "")
            {
                if (!Directory.Exists(filepath + "/stocks/" + brand))
                {
                    Directory.CreateDirectory(filepath + "/stocks/" + brand);
                }
            }
            if (model != "")
            {
                if (stats == "")
                {
                    itempath = filepath + "/stocks/" + brand + "/" + model + ".si";
                    if (!File.Exists(itempath))
                    {
                        File.Create(itempath).Close();
                    }
                }
                else
                {
                    if (!Directory.Exists(filepath + "/stocks/" + brand + "/" + model))
                    {
                        Directory.CreateDirectory(filepath + "/stocks/" + brand + "/" + model);
                    }
                    itempath = filepath + "/stocks/" + brand + "/" + model + "/" + stats + ".si";
                    if (!File.Exists(itempath))
                    {
                        File.Create(itempath).Close();
                    }
                }
            }
            if (quantity != "")
            {
                string p = getItemPrice();
                string q = getItemQuantity();
                if (price != "")
                {
                    if (q == "" && p == "")
                    {
                        File.WriteAllText(itempath, quantity + "\n" + price);
                        string newtotal = (float.Parse(price) * float.Parse(quantity)).ToString("C");
                        File.AppendAllText(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "input.si", brand + "|" + model + "|" + stats + "|" + quantity + "|" + price + "|" + newtotal + "\n");
                    }
                    else
                    { 
                        string newquantity = (Int32.Parse(quantity) + Int32.Parse(q)).ToString();
                        string newprice = ((float.Parse(price) + float.Parse(p)) / 2).ToString();
                        string newtotal = (float.Parse(newprice) * float.Parse(quantity)).ToString("C");
                        File.WriteAllText(itempath, newquantity + "\n" + newprice);
                        if (quantity.StartsWith("-"))
                        {
                            File.AppendAllText(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "output.si", brand + "|" + model + "|" + stats + "|" + quantity.Replace("-", "") + "\n");
                        }
                        else
                        {
                            File.AppendAllText(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "input.si", brand + "|" + model + "|" + stats + "|" + quantity + "|" + price + "\n");
                        }
                    }
                }
            }
        }
        public string getIOQauntiti(string fp, string type)
        {
            Int32 quantiti = 0;
            string[] lines = File.ReadAllLines(fp);
            string b = "";string m = "";string s = "";
            foreach (string line in lines)
            {
                string[] array = line.Split('|');
                if (array[0] == brand)
                {
                    if (array[1] == model)
                    {
                        if (array[2] == stats)
                        {
                            b = brand;
                            m = model;
                            s = stats;
                            quantiti += Int32.Parse(array[3]);
                        }
                    }
                }
            }
            if (type == "output")
            {
                quantiti -= Int32.Parse(new StockItem() { brand = b, model = m, stats = s }.getSalesQuantiti(fp.Replace("output.si", "sales.si")));
            }
            return quantiti.ToString();
        }
        public string getSalesQuantiti(string fp)
        {
            Int32 quantiti = 0;
            string[] lines = File.ReadAllLines(fp);
            foreach (string line in lines)
            {
                string[] array = line.Split('|');
                if ( array[1] == brand )
                {
                    if ( array[2] == model )
                    {
                        if ( array[3] == stats )
                        {
                            quantiti += Int32.Parse(array[4]);
                        }
                    }
                }
            }
            return quantiti.ToString();
        }
        public List<string> getAllBrand()
        {
            List<string> brands = new List<string>();
            DirectoryInfo d = new DirectoryInfo(filepath + "/stocks");
            DirectoryInfo[] files = d.GetDirectories("*");
            foreach (DirectoryInfo file in files)
            {
                brands.Add(file.Name);
            }
            return brands;
        }
        public List<string> getAllModelNoHaveStats()
        {
            List<string> models = new List<string>();
            DirectoryInfo d = new DirectoryInfo(filepath + "/stocks/" + brand);
            FileInfo[] files = d.GetFiles("*");
            foreach (FileInfo file in files)
            {
                models.Add(Path.GetFileNameWithoutExtension(filepath + "/stocks/" + brand + "/" + file.Name));
            }
            return models;
        }
        public List<string> getAllModelHaveStats()
        {
            List<string> models = new List<string>();
            DirectoryInfo d = new DirectoryInfo(filepath + "/stocks/" + brand);
            DirectoryInfo[] files = d.GetDirectories("*");
            foreach (DirectoryInfo file in files)
            {
                models.Add(file.Name);
            }
            return models;
        }
        public List<string> getAllStats()
        {
            List<string> stats = new List<string>();
            DirectoryInfo d = new DirectoryInfo(filepath + "/stocks/" + brand + "/" + model + "/");
            FileInfo[] files = d.GetFiles("*");
            foreach (FileInfo file in files)
            {
                stats.Add(Path.GetFileNameWithoutExtension(filepath + "/stocks/" + brand + "/" + model + "/" + file.Name));
            }
            return stats;
        }
        public string getItemQuantity()
        {
            if (stats == "")
            {
                string[] lines = File.ReadAllLines(filepath + "/stocks/" + brand + "/" + model + ".si");
                if (lines.Count() < 1)
                {
                    return "";
                }
                return lines[0];
            }
            else
            {
                string[] lines = File.ReadAllLines(filepath + "/stocks/" + brand + "/" + model + "/" + stats + ".si");
                if (lines.Count() < 1)
                {
                    return "";
                }
                return lines[0];
            }
        }
        public string getItemPrice()
        {
            if (stats == "")
            {
                string[] lines = File.ReadAllLines(filepath + "/stocks/" + brand + "/" + model + ".si");
                if (lines.Count() < 1)
                {
                    return "";
                }
                return lines[1];
            }
            else
            {
                string[] lines = File.ReadAllLines(filepath + "/stocks/" + brand + "/" + model + "/" + stats + ".si");
                if (lines.Count() < 1)
                {
                    return "";
                }
                return lines[1];
            }
        }
    }
}
