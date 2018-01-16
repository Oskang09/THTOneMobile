using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using THTOneMobile.Classes;

namespace THTOneMobile
{
    public partial class Form1 : Form
    {
        string filepath = Application.StartupPath;
        public Form1()
        {
            InitializeComponent();
            readSales();
            readStockItem();
            readSalesStockItem();
            loadStockIn();
            loadStockOut();
            readInvoiceList();
        }
        void readSales()
        {
            if (!Directory.Exists(filepath + "/sales/" + DateTime.Now.Year))
            {
                Directory.CreateDirectory(filepath + "/sales/" + DateTime.Now.Year);
            }
            if (!Directory.Exists(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month))
            {
                Directory.CreateDirectory(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month);
            }
            if (!Directory.Exists(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day))
            {
                Directory.CreateDirectory(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day);
            }
            if (!File.Exists(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "sales.si"))
            {
                File.Create(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "sales.si").Close();
            }
            if (!File.Exists(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "input.si"))
            {
                File.Create(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "input.si").Close();
            }
            if (!File.Exists(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "output.si"))
            {
                File.Create(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "output.si").Close();
            }
            string[] array = File.ReadAllLines(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "sales.si");
            string id = "0";
            if (array.Count() == 0)
            {
                id = "1";
                sales_id.Text = id;
                sales_date.Text = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                return;
            }
            id = (Int32.Parse(array[array.Count() - 1].Split('|').First()) + 1).ToString();
            sales_id.Text = id;
            sales_date.Text = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
        }
        void readSalesStockItem()
        {
            sales_brand.Items.Clear();
            foreach (string item in new StockItem().getAllBrand())
            {
                sales_brand.Items.Add(item);
                sales_brand.SelectedIndexChanged += delegate
                {
                    sales_model.ResetText();
                    sales_stats.ResetText();
                    sales_quantiti.ResetText();
                    sales_price.ResetText();
                    if (sales_brand.Text == item)
                    {
                        sales_model.Items.Clear();
                        foreach (string item2 in new StockItem() { brand = item }.getAllModelHaveStats())
                        {
                            sales_model.Items.Add(item2);
                            sales_model.SelectedIndexChanged += delegate
                            {
                                sales_stats.ResetText();
                                sales_quantiti.ResetText();
                                sales_price.ResetText();
                                if (sales_model.Text == item2)
                                {
                                    sales_stats.Items.Clear();
                                    foreach (string item4 in new StockItem() { brand = item, model = item2 }.getAllStats())
                                    {
                                        sales_stats.Items.Add(item4);
                                        sales_stats.SelectedIndexChanged += delegate
                                        {
                                            sales_quantiti.ResetText();
                                            if (sales_stats.Text == item4)
                                            {
                                                if (new StockItem() { brand = item, model = item2, stats = item4 }.getItemPrice() != "")
                                                {
                                                    sales_price.Text = new StockItem() { brand = item, model = item2, stats = item4 }.getItemPrice();
                                                }
                                            }
                                        };
                                    }
                                }
                            };
                        }
                        foreach (string item3 in new StockItem() { brand = item }.getAllModelNoHaveStats())
                        {
                            if (sales_model.Items.Contains(item3))
                            {
                                sales_model.Items.Add(item3 + "(N)");
                                sales_model.SelectedIndexChanged += delegate
                                {
                                    sales_stats.ResetText();
                                    sales_quantiti.ResetText();
                                    if (sales_model.Text == item3 + "(N)")
                                    {
                                        sales_stats.Items.Clear();
                                        if (new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice() != "")
                                        {
                                            sales_price.Text = new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice();
                                        }
                                    }
                                };
                            }
                            else
                            {
                                sales_model.Items.Add(item3);
                                sales_model.SelectedIndexChanged += delegate
                                {
                                    sales_stats.ResetText();
                                    sales_quantiti.ResetText();
                                    if (sales_model.Text == item3)
                                    {
                                        sales_stats.Items.Clear();
                                        if (new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice() != "")
                                        {
                                            sales_price.Text = new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice();
                                        }
                                    }
                                };
                            }
                        }
                    }
                };
            }
        }

        void readStockItem()
        {
            text_brand.Items.Clear();
            search_brand.Items.Clear();
            out_brand.Items.Clear();
            foreach (string item in new StockItem().getAllBrand())
            {
                search_brand.Items.Add(item);
                out_brand.Items.Add(item);
                //Out
                out_brand.SelectedIndexChanged += delegate
                {
                    out_model.ResetText();
                    out_stat.ResetText();
                    out_quantiti.ResetText();
                    if (out_brand.Text == item)
                    {
                        out_model.Items.Clear();
                        foreach (string item2 in new StockItem() { brand = item }.getAllModelHaveStats())
                        {
                            out_model.Items.Add(item2);
                            out_model.SelectedIndexChanged += delegate
                            {
                                out_quantiti.ResetText();
                                if (out_model.Text == item2)
                                {
                                    out_stat.Items.Clear();
                                    foreach (string item4 in new StockItem() { brand = item, model = item2 }.getAllStats())
                                    {
                                        out_stat.Items.Add(item4);
                                    }
                                }
                            };
                        }
                        foreach (string item3 in new StockItem() { brand = item }.getAllModelNoHaveStats())
                        {
                            if (out_model.Items.Contains(item3))
                            {
                                out_model.Items.Add(item3 + "(N)");
                                out_model.SelectedIndexChanged += delegate
                                {
                                    out_stat.ResetText();
                                    out_quantiti.ResetText();
                                    if (out_model.Text == item3 + "(N)")
                                    {
                                        out_stat.Items.Clear();
                                    }
                                };
                            }
                            else
                            {
                                out_model.Items.Add(item3);
                                out_model.SelectedIndexChanged += delegate
                                {
                                    out_stat.ResetText();
                                    out_quantiti.ResetText();
                                    if (out_model.Text == item3)
                                    {
                                        out_stat.Items.Clear();
                                    }
                                };
                            }
                        }
                    }
                };
                // Others
                text_brand.Items.Add(item);
                text_brand.SelectedIndexChanged += delegate
                {
                    text_model.ResetText();
                    text_stat.ResetText();
                    text_quantiti.ResetText();
                    text_price.ResetText();
                    if (text_brand.Text == item)
                    {
                        text_model.Items.Clear();
                        foreach (string item2 in new StockItem() { brand = item }.getAllModelHaveStats())
                        {
                            text_model.Items.Add(item2);
                            text_model.SelectedIndexChanged += delegate
                            {
                                text_quantiti.ResetText();
                                if (text_model.Text == item2)
                                {
                                    text_stat.Items.Clear();
                                    foreach (string item4 in new StockItem() { brand = item, model = item2 }.getAllStats())
                                    {
                                        text_stat.Items.Add(item4);
                                        text_stat.SelectedIndexChanged += delegate
                                        {
                                            text_quantiti.ResetText();
                                            if (text_stat.Text == item4)
                                            {
                                                if (new StockItem() { brand = item, model = item2, stats = item4 }.getItemPrice() != "")
                                                {
                                                    text_price.Text = new StockItem() { brand = item, model = item2, stats = item4 }.getItemPrice();
                                                }
                                            }
                                        };
                                    }
                                }
                            };
                        }
                        foreach (string item3 in new StockItem() { brand = item }.getAllModelNoHaveStats())
                        {
                            if (text_model.Items.Contains(item3))
                            {
                                text_model.Items.Add(item3 + "(N)");
                                text_model.SelectedIndexChanged += delegate
                                {
                                    text_stat.ResetText();
                                    text_quantiti.ResetText();
                                    if (text_model.Text == item3 + "(N)")
                                    {
                                        text_stat.Items.Clear();
                                        if (new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice() != "")
                                        {
                                            text_price.Text = new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice();
                                        }
                                    }
                                };
                            }
                            else
                            {
                                text_model.Items.Add(item3);
                                text_model.SelectedIndexChanged += delegate
                                {
                                    text_stat.ResetText();
                                    text_quantiti.ResetText();
                                    if (text_model.Text == item3)
                                    {
                                        text_stat.Items.Clear();
                                        if (new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice() != "")
                                        {
                                            text_price.Text = new StockItem() { brand = item, model = item3, stats = "" }.getItemPrice();
                                        }
                                    }
                                };
                            }
                        }
                    }
                };
            }
        }

        private void stockitem_out_Click(object sender, EventArgs e)
        {
            if (out_brand.Text.Contains("|") || out_model.Text.Contains("|") || out_stat.Text.Contains("|"))
            {
                MessageBox.Show("物品名字内有非法字体/符号!");
                return;
            }
            if (out_quantiti.Text.Contains("."))
            {
                MessageBox.Show("请输入正确的数量!");
                return;
            }
            StockItem si = new StockItem();
            si.brand = out_brand.Text;
            si.model = out_model.Text.Replace("(N)", "");
            si.stats = out_stat.Text;
            si.quantity = "-" + out_quantiti.Text;
            si.price = si.getItemPrice();
            si.addStockItem();
            readStockItem();
            readSalesStockItem();
            loadStockOut();
            MessageBox.Show("物品出货成功!");
            out_brand.ResetText();
            out_model.ResetText();
            out_stat.ResetText();
            out_quantiti.ResetText();
            if (Int32.Parse(si.getItemQuantity()) < 1)
            {
                MessageBox.Show("物品编号 - " + si.brand + " 货存已经没了!");
            }
        }
        private void stockitem_add_Click(object sender, EventArgs e)
        {
            if (text_brand.Text.Contains("|") || text_model.Text.Contains("|") || text_price.Text.Contains("|"))
            {
                MessageBox.Show("物品名字内有非法字体/符号!");
                return;
            }
            if (text_quantiti.Text.Contains("."))
            {
                MessageBox.Show("请输入正确的数量!");
                return;
            }
            StockItem si = new StockItem();
            si.brand = text_brand.Text;
            si.model = text_model.Text.Replace("(N)", "");
            si.stats = text_stat.Text;
            si.quantity = text_quantiti.Text;
            si.price = text_price.Text;
            si.addStockItem();
            readStockItem();
            readSalesStockItem();
            loadStockIn();
            MessageBox.Show("物品进货成功!");
            text_brand.ResetText();
            text_model.ResetText();
            text_stat.ResetText();
            text_quantiti.ResetText();
            text_price.ResetText();
        }

        private void stockitem_load_Click(object sender, EventArgs e)
        {
            stockitem_dgv.Columns.Clear();
            stockitem_dgv.Rows.Clear();
            if (search_brand.Text != "")
            {
                stockitem_dgv.Columns.Add("名字 ", "名字");
                stockitem_dgv.Columns.Add("价格", "价格");
                stockitem_dgv.Columns.Add("数量", "数量");
                if (search_model.Text == "")
                {
                    foreach (string item in new StockItem() { brand = search_brand.Text }.getAllModelNoHaveStats())
                    {
                        stockitem_dgv.Rows.Add(item, float.Parse(new StockItem() { brand = search_brand.Text , model = item, stats = "" }.getItemPrice()).ToString("C"), new StockItem() { brand = search_brand.Text, model = item, stats = "" }.getItemQuantity());
                    }
                }
                else
                {
                    foreach (string item in new StockItem() { brand = search_brand.Text , model = search_model.Text }.getAllStats())
                    {
                        stockitem_dgv.Rows.Add(item , float.Parse(new StockItem() { brand = search_brand.Text,model = search_model.Text, stats = item }.getItemPrice()).ToString("C"), new StockItem() { brand = search_brand.Text, model = search_model.Text, stats = item }.getItemQuantity());
                    }
                }
            }
        }

        private void search_brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            search_model.Items.Clear();
            foreach (string item in new StockItem() {  brand=search_brand.Text }.getAllModelHaveStats())
            {
                search_model.Items.Add(item);
            }
            stockitem_load.PerformClick();
        }

        private void sales_add_Click(object sender, EventArgs e)
        {
            if (sales_brand.Text.Contains("|") || sales_model.Text.Contains("|") || sales_stats.Text.Contains("|"))
            {
                MessageBox.Show("物品名字内有非法字体/符号!");
                return;
            }
            if (sales_quantiti.Text.Contains("."))
            {
                MessageBox.Show("请输入正确的数量！");
                return;
            }
            if (sales_quantiti.Text == "")
            {
                MessageBox.Show("请输入购买的数量！");
                return;
            }
            if (sales_brand.Text == "")
            {
                MessageBox.Show("请选择购买的物品！");
                return;
            }
            if (sales_model.Text == "")
            {
                MessageBox.Show("请选择购买的物品！");
                return;
            }
            if (sales_stats.Items.Count !=  0)
            {
                if (sales_stats.Text == "")
                {
                    MessageBox.Show("请选择购买的物品！");
                    return;
                }
            }
            string[] rows =
            {
                sales_brand.Text ,
                sales_model.Text.Replace("(N)", "") ,
                sales_stats.Text , 
                sales_quantiti.Text ,
                (float.Parse(sales_price.Text) * float.Parse(sales_quantiti.Text)).ToString("C")
            };
            ListViewItem lvi = new ListViewItem(rows); 
            sales_listView.Items.Add(lvi);
        }

        private void sales_end_Click(object sender, EventArgs e)
        {
            List<string> sales = new List<string>();
            float latestprice = 0;
            StockItem si = new StockItem();
            foreach (ListViewItem lvi in sales_listView.Items)
            {
                string brand = lvi.SubItems[0].Text;
                string model = lvi.SubItems[1].Text;
                string stats = lvi.SubItems[2].Text;
                string quantiti = lvi.SubItems[3].Text;
                string price = lvi.SubItems[4].Text;
                sales.Add(sales_id.Text + "|" + brand + "|" + model + "|" + stats + "|" + quantiti + "|" + price);
                si.brand = lvi.SubItems[0].Text;
                si.model = lvi.SubItems[1].Text;
                si.stats = lvi.SubItems[2].Text;
                si.quantity = "-" + lvi.SubItems[3].Text;
                si.price = si.getItemPrice();
                si.addStockItem();
                latestprice += float.Parse(price, System.Globalization.NumberStyles.Currency);
            }
            File.AppendAllLines(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + "sales.si", sales);
            SalesEndTotal tf = new SalesEndTotal(latestprice.ToString("C"));
            tf.Show();
            tf.FormClosing += delegate
            {
                sales_brand.Items.Clear();
                sales_brand.ResetText();
                sales_model.Items.Clear();
                sales_model.ResetText();
                sales_stats.Items.Clear();
                sales_stats.ResetText();
                sales_quantiti.ResetText();
                sales_price.ResetText();
                sales_listView.Items.Clear();
                readStockItem();
                readSalesStockItem();
                readSales();
                if (Int32.Parse(si.getItemQuantity()) < 1)
                {
                    MessageBox.Show("物品编号 - " + si.brand + " 货存已经没了!");
                }
            };
        }

        private void search_model_SelectedIndexChanged(object sender, EventArgs e)
        {
            stockitem_load.PerformClick();
        }

        private void stocklist_print_Click(object sender, EventArgs e)
        {
            loadStocks();
            ClsPrint print = new ClsPrint(printing_gridview, "THT One Mobile 货物列表", stocklist_date.Value.Year + "年" + stocklist_date.Value.Month + "月" + stocklist_date.Value.Day + "日");
            print.PrintForm();
        }
        private void daysales_print_Click(object sender, EventArgs e)
        {
            loadSales();
            ClsPrint print = new ClsPrint(printing_gridview, "THT One Mobile 一日销售", daysales_date.Value.Year + "年" + daysales_date.Value.Month + "月" + daysales_date.Value.Day + "日");
            print.PrintForm();
        }
        void loadStocks()
        {
            printing_gridview.Columns.Clear();
            printing_gridview.Rows.Clear();
            printing_gridview.Columns.Add("Brand", "编号");
            printing_gridview.Columns.Add("Name", "名字");
            printing_gridview.Columns.Add("Stats", "备注");
            printing_gridview.Columns.Add("Current_stock", "当前存货");
            printing_gridview.Columns.Add("Output_Stock", "出货");
            printing_gridview.Columns.Add("Input Stock", "进货");
            printing_gridview.Columns.Add("Latest_Stock", "最后存货");
            
            string salesfile = filepath + "/sales/" + stocklist_date.Value.Year + "/" + stocklist_date.Value.Month + "/" + stocklist_date.Value.Day + "/" + "sales.si";
            string inputfile = filepath + "/sales/" + stocklist_date.Value.Year + "/" + stocklist_date.Value.Month + "/" + stocklist_date.Value.Day + "/" + "input.si";
            string outputfile = filepath + "/sales/" + stocklist_date.Value.Year + "/" + stocklist_date.Value.Month + "/" + stocklist_date.Value.Day + "/" + "output.si";
            if (!File.Exists(salesfile) || !File.Exists(inputfile) || !File.Exists(outputfile))
            {
                MessageBox.Show("日期选择错误!");
                return;
            }
            int brandcount = 1;
            int modelcount = 1;
            int statscount = 1;
            foreach (string brands in new StockItem().getAllBrand())
            {
                modelcount = 1;
                statscount = 1;
                foreach (string modelns in new StockItem() { brand = brands }.getAllModelNoHaveStats())
                {
                    StockItem si = new StockItem() { brand = brands, model = modelns, stats = "" };
                    int current = ((Int32.Parse(si.getItemQuantity()) - Int32.Parse(si.getIOQauntiti(inputfile, "input"))) + (Int32.Parse(si.getIOQauntiti(outputfile, "output"))) + Int32.Parse(si.getSalesQuantiti(salesfile)));
                    int outstock = Int32.Parse(si.getIOQauntiti(outputfile, "output")) + Int32.Parse(si.getSalesQuantiti(salesfile));
                    int instock = Int32.Parse(si.getIOQauntiti(inputfile, "input"));
                    int lateststock = Int32.Parse(si.getItemQuantity());
                    if (modelcount > 1)
                    {
                        printing_gridview.Rows.Add("", modelns, "", current, outstock, instock, lateststock);
                        modelcount++;
                        statscount++;
                        continue;
                    }
                    printing_gridview.Rows.Add(brands, modelns, "", current, outstock, instock, lateststock);
                    modelcount++;
                    statscount++;
                }
                foreach (string modelhs in new StockItem() { brand = brands }.getAllModelHaveStats())
                {
                    modelcount = 1;
                    foreach (string stats2 in new StockItem() { brand = brands, model = modelhs }.getAllStats())
                    {
                        StockItem si = new StockItem() { brand = brands, model = modelhs, stats = stats2 };
                        int current = ((Int32.Parse(si.getItemQuantity()) - Int32.Parse(si.getIOQauntiti(inputfile, "input"))) + (Int32.Parse(si.getIOQauntiti(outputfile, "output"))) + Int32.Parse(si.getSalesQuantiti(salesfile)));
                        int outstock = Int32.Parse(si.getIOQauntiti(outputfile, "output")) + Int32.Parse(si.getSalesQuantiti(salesfile));
                        int instock = Int32.Parse(si.getIOQauntiti(inputfile, "input"));
                        int lateststock = Int32.Parse(si.getItemQuantity());
                        if (modelcount > 1)
                        {
                            if (statscount > 1)
                            {
                                printing_gridview.Rows.Add("", "", stats2, current, outstock, instock, lateststock);
                                statscount++;
                                continue;
                            }
                        }
                        if (statscount > 1)
                        {
                            printing_gridview.Rows.Add("", modelhs, stats2, current, outstock, instock, lateststock);
                            statscount++;
                            continue;
                        }
                        printing_gridview.Rows.Add(brands, modelhs, stats2, current, outstock, instock, lateststock);
                        statscount++;
                    }
                    modelcount++;
                }
                brandcount++;
            }
        }
        void loadSales()
        {
            printing_gridview.Columns.Clear();
            printing_gridview.Rows.Clear();
            printing_gridview.Columns.Add("Brand", "编号");
            printing_gridview.Columns.Add("Name", "名字");
            printing_gridview.Columns.Add("Stats", "备注");
            printing_gridview.Columns.Add("Quantiti", "数量");
            printing_gridview.Columns.Add("Price2", "原价");
            printing_gridview.Columns.Add("Price", "价钱");
            printing_gridview.Columns.Add("Profit", "盈利");
            string salesfile = filepath + "/sales/" + daysales_date.Value.Year + "/" + daysales_date.Value.Month + "/" + daysales_date.Value.Day + "/" + "sales.si";
            if (!File.Exists(salesfile))
            {
                MessageBox.Show("日期选择错误!");
                return;
            }
            float totalprice = 0;
            Int32 totalquantiti = 0;
            float totalprofit = 0;
            float oriprice = 0;
            string[] array = File.ReadAllLines(salesfile);
            foreach (string ar in array)
            {
                string[] a = ar.Split('|');
                totalquantiti += Int32.Parse(a[4]);
                totalprice += float.Parse(a[5], System.Globalization.NumberStyles.Currency);
                oriprice = float.Parse(new StockItem() { brand = a[1], model = a[2], stats = a[3] }.getItemPrice());
                totalprofit += float.Parse(a[5], System.Globalization.NumberStyles.Currency) - (oriprice * int.Parse(a[4]));
                printing_gridview.Rows.Add(a[1], a[2], a[3], a[4], oriprice.ToString("C"), a[5], (float.Parse(a[5], System.Globalization.NumberStyles.Currency) - (oriprice * int.Parse(a[4]))));
            }

            printing_gridview.Rows.Add("总额", "", "", totalquantiti, "", totalprice.ToString("C"), totalprofit.ToString("C"));
        }
        void loadStockIn()
        {
            stockin_view.Columns.Clear();
            stockin_view.Rows.Clear();
            stockin_view.Columns.Add("Brand", "编号");
            stockin_view.Columns.Add("Name", "名字");
            stockin_view.Columns.Add("Stats", "备注");
            stockin_view.Columns.Add("In", "进货数量");
            stockin_view.Columns.Add("Price", "价钱");
            stockin_view.Columns.Add("total", "总数");
            string inputfile = filepath + "/sales/" + stocklist_date.Value.Year + "/" + stocklist_date.Value.Month + "/" + stocklist_date.Value.Day + "/" + "input.si";
            string[] array = File.ReadAllLines(inputfile);
            Int32 linechecker = 1;
            foreach (string ar in array)
            {
                if (ar.Contains("#INVOICED"))
                {
                    array = array.Skip(linechecker).ToArray();
                    linechecker = 0;
                }
                linechecker++;
            }
            int brandcount = 1;
            int modelcount = 1;
            int statscount = 1;
            foreach (string brands in new StockItem().getAllBrand())
            {
                modelcount = 1;
                statscount = 1;
                foreach (string modelns in new StockItem() { brand = brands }.getAllModelNoHaveStats())
                {
                    Int32 latestquantiti = 0;
                    string latestprice = "";
                    string totalprice = "";
                    foreach (string line in array)
                    {
                        string[] a = line.Split('|');
                        if (a[0] == brands)
                        {
                            if (a[1] == modelns)
                            {
                                if (a[2] == "")
                                {
                                    latestquantiti += Int32.Parse(a[3]);
                                    latestprice = a[4];
                                }
                            }
                        }
                    }
                    if (latestprice != "")
                    {
                        totalprice = (float.Parse(latestquantiti.ToString()) * float.Parse(latestprice)).ToString();
                    }
                    if (latestquantiti > 0)
                    {
                        if (modelcount > 1)
                        {
                            modelcount++;
                            statscount++;
                            stockin_view.Rows.Add("", modelns, "", latestquantiti, float.Parse(latestprice).ToString("C"), float.Parse(totalprice).ToString("c"));
                            continue;
                        }
                        stockin_view.Rows.Add(brands, modelns, "", latestquantiti, float.Parse(latestprice).ToString("C"), float.Parse(totalprice).ToString("c"));
                        modelcount++;
                        statscount++;
                    }
                }
                foreach (string modelhs in new StockItem() { brand = brands }.getAllModelHaveStats())
                {
                    modelcount = 1;
                    foreach (string stats2 in new StockItem() { brand = brands, model = modelhs }.getAllStats())
                    {
                        Int32 latestquantiti = 0;
                        string latestprice = "";
                        string totalprice = "";
                        foreach (string line in array)
                        {
                            string[] a = line.Split('|');
                            if (a[0] == brands)
                            {
                                if (a[1] == modelhs)
                                {
                                    if (a[2] == stats2)
                                    {
                                        latestquantiti += Int32.Parse(a[3]);
                                        latestprice = a[4];
                                    }
                                }
                            }
                        }
                        if (latestprice != "")
                        {
                            totalprice = (float.Parse(latestquantiti.ToString()) * float.Parse(latestprice)).ToString();
                        }
                        if (latestquantiti > 0)
                        {
                            if (modelcount > 1)
                            {
                                if (statscount > 1)
                                {
                                    stockin_view.Rows.Add("", "", stats2, latestquantiti, float.Parse(latestprice).ToString("C"), float.Parse(totalprice).ToString("c"));
                                    statscount++;
                                    continue;
                                }
                            }
                            if (statscount > 1)
                            {
                                stockin_view.Rows.Add("", modelhs, stats2, latestquantiti, float.Parse(latestprice).ToString("C"), float.Parse(totalprice).ToString("c"));
                                statscount++;
                                continue;
                            }
                            stockin_view.Rows.Add(brands, modelhs, stats2, latestquantiti, float.Parse(latestprice).ToString("C"), float.Parse(totalprice).ToString("c"));
                            statscount++;
                        }
                    }
                    modelcount++;
                }
                brandcount++;
            }
            invoice_stock.Columns.Clear();
            invoice_stock.Rows.Clear();
            invoice_stock.Columns.Add("Brand", "编号");
            invoice_stock.Columns.Add("Name", "名字");
            invoice_stock.Columns.Add("Stats", "备注");
            invoice_stock.Columns.Add("In", "进货数量");
            invoice_stock.Columns.Add("Price", "价钱");
            invoice_stock.Columns.Add("total", "总数");
            foreach (DataGridViewRow dgvr in stockin_view.Rows)
            {
                List<string> row = new List<string>();
                foreach (DataGridViewCell cell in dgvr.Cells)
                {
                    row.Add(cell.Value.ToString());
                }
                invoice_stock.Rows.Add(row.ToArray());
            }
        }
        void loadStockOut()
        {
            stockout_view.Columns.Clear();
            stockout_view.Rows.Clear();
            stockout_view.Columns.Add("Brand", "编号");
            stockout_view.Columns.Add("Name", "名字");
            stockout_view.Columns.Add("Stats", "备注");
            stockout_view.Columns.Add("Out", "出货数量");
            string outputfile = filepath + "/sales/" + stocklist_date.Value.Year + "/" + stocklist_date.Value.Month + "/" + stocklist_date.Value.Day + "/" + "output.si";
            string[] array = File.ReadAllLines(outputfile);
            int brandcount = 1;
            int modelcount = 1;
            int statscount = 1;
            foreach (string brands in new StockItem().getAllBrand())
            {
                modelcount = 1;
                statscount = 1;
                foreach (string modelns in new StockItem() { brand = brands }.getAllModelNoHaveStats())
                {
                    Int32 latestquantiti = 0;
                    foreach (string line in array)
                    {
                        string[] a = line.Split('|');
                        if (a[0] == brands)
                        {
                            if (a[1] == modelns)
                            {
                                if (a[2] == "")
                                {
                                    latestquantiti += Int32.Parse(a[3]);
                                }
                            }
                        }
                    }
                    if (latestquantiti > 0)
                    {
                        if (modelcount > 1)
                        {
                            modelcount++;
                            statscount++;
                            stockout_view.Rows.Add("", modelns, "", latestquantiti);
                            continue;
                        }
                        stockout_view.Rows.Add(brands, modelns, "", latestquantiti);
                        modelcount++;
                        statscount++;
                    }
                }
                foreach (string modelhs in new StockItem() { brand = brands }.getAllModelHaveStats())
                {
                    modelcount = 1;
                    foreach (string stats2 in new StockItem() { brand = brands, model = modelhs }.getAllStats())
                    {
                        Int32 latestquantiti = 0;
                        foreach (string line in array)
                        {
                            string[] a = line.Split('|');
                            if (a[0] == brands)
                            {
                                if (a[1] == modelhs)
                                {
                                    if (a[2] == stats2)
                                    {
                                        latestquantiti += Int32.Parse(a[3]);
                                    }
                                }
                            }
                        }
                        if (latestquantiti > 0)
                        {
                            if (modelcount > 1)
                            {
                                if (statscount > 1)
                                {
                                    stockout_view.Rows.Add("", "", stats2, latestquantiti);
                                    statscount++;
                                    continue;
                                }
                            }
                            if (statscount > 1)
                            {
                                stockout_view.Rows.Add("", modelhs, stats2, latestquantiti);
                                statscount++;
                                continue;
                            }
                            stockout_view.Rows.Add(brands, modelhs, stats2, latestquantiti);
                            statscount++;
                        }
                    }
                    modelcount++;
                }
                brandcount++;
            }
        }
        void readInvoiceList()
        {
            invoice_list.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(filepath + "/invoice/");
            FileInfo[] files = d.GetFiles("*");
            foreach (FileInfo a in files)
            {
                invoice_list.Items.Add(Path.GetFileNameWithoutExtension(filepath + "/invoice/" + a.Name));
            }
        }

        private void invoice_print_Click(object sender, EventArgs e)
        {
            if (invoice_list.Text == "")
            {
                MessageBox.Show("请输入Invoice单号后才复印!");
                return;
            }
            if (!File.Exists(filepath + "/invoice/" + invoice_list.Text))
            {
                MessageBox.Show("选择的单号不存在!");
                return;
            }
            printing_gridview.Columns.Clear();
            printing_gridview.Rows.Clear();
            printing_gridview.Columns.Add("Brand", "编号");
            printing_gridview.Columns.Add("Name", "名字");
            printing_gridview.Columns.Add("Stats", "备注");
            printing_gridview.Columns.Add("In", "进货数量");
            printing_gridview.Columns.Add("Price", "价钱");
            printing_gridview.Columns.Add("total", "总数");
            foreach (string line in File.ReadAllLines(filepath + "/invoice/" + invoice_list.Text))
            {
                if (!line.StartsWith("DATE="))
                {
                    printing_gridview.Rows.Add(line.Split('|'));
                }
                else
                {
                    string[] date = line.Split('=').Last().Split('-');
                    ClsPrint print = new ClsPrint(printing_gridview, "Invoice NO: " + invoice_list.Text, date[0] + "年" + date[1] + "月" + date[2] + "日");
                    print.PrintForm();
                }
            }
        }

        private void sales_brand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sales_brand.Items.Clear();
                foreach (string b in new StockItem().getAllBrand())
                {
                    if (b.Contains(sales_brand.Text))
                    {
                        sales_brand.Items.Add(b);
                    }
                }
            }
        }

        private void out_brand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                out_brand.Items.Clear();
                foreach (string b in new StockItem().getAllBrand())
                {
                    if (b.Contains(out_brand.Text))
                    {
                        out_brand.Items.Add(b);
                    }
                }
            }
        }

        private void search_brand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                search_brand.Items.Clear();
                foreach (string b in new StockItem().getAllBrand())
                {
                    if (b.Contains(search_brand.Text))
                    {
                        search_brand.Items.Add(b);
                    }
                }
            }
        }

        private void invoice_list_KeyDown(object sender, KeyEventArgs e)
        {
            invoice_list.Items.Clear();
            if (e.KeyCode == Keys.Enter)
            {
                foreach (string a in Directory.GetFiles(filepath + "/invoice/"))
                {
                    if (a.Contains(invoice_list.Text))
                    {
                        invoice_list.Items.Add(a);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (invoice_num.Text == "")
            {
                MessageBox.Show("请输入Invoice单号");
                return;
            }
            DirectoryInfo d = new DirectoryInfo(filepath + "/invoice/");
            FileInfo[] files = d.GetFiles("*");
            foreach (FileInfo a in files)
            {
                if (Path.GetFileNameWithoutExtension(filepath + "/invoice/" + a.Name) == invoice_num.Text)
                {
                    MessageBox.Show("此Invoice单号已经存在!");
                    return;
                }
            }
            string id = invoice_num.Text;
            File.Create(filepath + "/invoice/" + id).Close();
            List<string> invoice = new List<string>();
            foreach (DataGridViewRow dgvr in stockin_view.Rows)
            {
                string b = "";
                foreach (DataGridViewCell cell in dgvr.Cells)
                {
                    b += cell.Value + "|";
                }
                invoice.Add(b.Substring(0, b.Length - 1));
            }
            invoice.Add("DATE=" + invoice_date.Value.Year + "-" + invoice_date.Value.Month + "-" + invoice_date.Value.Day);
            File.AppendAllLines(filepath + "/invoice/" + id, invoice);
            File.AppendAllText(filepath + "/sales/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/input.si", "#INVOICED\n");
            readInvoiceList();
            loadStockIn();
            MessageBox.Show("Invoice生成完成!");
        }

        private void sales_remove_Click(object sender, EventArgs e)
        {
            sales_listView.Items.Clear();
        }
    }
}
