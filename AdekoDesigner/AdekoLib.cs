using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraGrid;
using System.Text.RegularExpressions;
using System.Globalization;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Net.Sockets;
using DevExpress.Drawing.Internal.Fonts.Interop;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace AdekoDesigner
{
    public partial class AdekoLib : DevExpress.XtraEditors.XtraForm
    {
        public AdekoLib()
        {
            InitializeComponent();

            mainDir = "C:\\Adeko 142\\";
            libFolderName = "";

            loadDataTableDefs();
        }

        private string mainDir, libFolderName;

        private List<DefGroup> defGroupList = new List<DefGroup>();
        private List<AdekoModule> adekoModuleList = new List<AdekoModule>();
        private BindingList<AdekoModule> adekoModuleList_canRead = new BindingList<AdekoModule>();
        private BindingList<AdekoModule> adekoModuleList_cantRead = new BindingList<AdekoModule>();

        DataTable dataDefs = null;


        private void gridControl2_Load(object sender, EventArgs e)
        {
            //loadRowData(1);
        }

        private void loadDataTableDefs()
        {
            dataDefs = new DataTable();
            dataDefs.Columns.Add("Code", typeof(string)); // Modül Kodu
            dataDefs.Columns.Add("Group", typeof(string)); // Grup
            dataDefs.Columns.Add("Width", typeof(decimal)); // Genişlik
            dataDefs.Columns.Add("Height", typeof(decimal)); // Yükseklik
            dataDefs.Columns.Add("Depth", typeof(decimal)); // Derinlik
            dataDefs.Columns.Add("Description", typeof(string)); // Açıklama
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDefFiles();
        }

        private void SaveDefFiles()
        {
            foreach (DefGroup defGroup in defGroupList)
            {
                string defFilePath = Path.Combine(mainDir, libFolderName, $"{defGroup.code}.DEF");

                if (!File.Exists(defFilePath))
                {
                    MessageBox.Show($"'{defGroup.code}.DEF' dosyası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                List<AdekoModule> adekoModuleList_ToSave = combineLists(adekoModuleList_canRead,adekoModuleList_cantRead);

                // Bu .DEF dosyasına ait güncellenmiş satırları al
                List<AdekoModule> related_adekoModuleDefList = adekoModuleList_ToSave
                    .Where(row => row.FileName == defGroup.code)
                    .OrderBy(row => row.FileRowNo)
                    .ToList();

                //Liste boş ise devam etme
                if (related_adekoModuleDefList.Count == 0) continue; 
                // Güncellenmiş satırları al
                var updatedRows = GetUpdatedRows(related_adekoModuleDefList);
                if (updatedRows.Count == 0)  return; 

                try
                {
                    // Mevcut dosyayı oku
                    //var originalLines = File.ReadAllLines(defFilePath);

                    // Yeni dosya içeriğini oluştur
                    var newLines = CreateUpdatedFileContent(defGroup, related_adekoModuleDefList);

                    // Yeni içeriği dosyaya yaz
                    using (var writer = new StreamWriter(defFilePath, false, Encoding.Default))
                    {
                        for (int i = 0; i < newLines.Count; i++)
                        {
                            if (i == newLines.Count - 1) // Last line
                            {
                                writer.Write(newLines[i]); // Write without adding a new line
                            }
                            else
                            {
                                writer.WriteLine(newLines[i]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"'{defGroup.code}.DEF' dosyasını güncellerken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("Dosyalar başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private List<AdekoModule> combineLists(BindingList<AdekoModule> list1, BindingList<AdekoModule> list2)
        {
            List<AdekoModule> combinedList = new List<AdekoModule>();

            foreach (var module in list1)
            {
                combinedList.Add(module);
            }

            foreach (var module in list2)
            {
                combinedList.Add(module);
            }
            return combinedList;
        }

        // Dosya içeriğini güncelleyen yardımcı metod
        private List<string> CreateUpdatedFileContent(DefGroup defGroup, List<AdekoModule> related_adekoModuleDefList)
        {
            var newLines = new List<string>();

            foreach (var line in related_adekoModuleDefList)
            {
                // OriginalDataLine ile eşleşen satırları bul

                
                var originalRow = "";
                if (!string.IsNullOrEmpty(line.OriginalDataLine)) originalRow = "(" + line.OriginalDataLine + ")";

                if (originalRow != null)
                {
                    if (line.IsUpdated)
                    {
                        // Güncellenmişse, yeni versiyonu ekle
                        string dataLine = FormatRowData(line);
                        newLines.Add(dataLine);
                    }
                    else
                    {
                        // Güncellenmemişse, mevcut haliyle ekle
                        newLines.Add(originalRow);
                    }
                }
            }

            return newLines;
        }

        private string FormatRowData(AdekoModule row)
        {
            // Örnek: Satırdaki verileri CSV formatında yaz
            return $@"(""{row.Code}"" ""{row.Dt1}"" {row.Width} {row.Height} {row.Depth} {row.DynamicDataString} ""{row.RootCode}"" ""{row.Description}"")";
        }



        private List<AdekoModule> GetUpdatedRows(List<AdekoModule> mList)
        {
            var updatedRows = new List<AdekoModule>();
            for (int i = 0; i < mList.Count; i++)
            {
                var row = gridView2.GetRow(i) as AdekoModule;
                if (row != null && row.IsUpdated)
                {
                    updatedRows.Add(row);
                }
            }
            return updatedRows;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }



        public void RefreshData()
        {
            List<string> ignoredFolders = new List<string> { "kapak", "kulp", "cera", "adeko_render_viewer32", "adeko_render_viewer64", "dclImages", "iconengines", "imageformats", "lights", "materials", "platforms", "ADEData", "adeko_render_viewer", "Agrx", "btoolsets", "Fonts", "Help", "Imalat", "lang", "language", "lng", "logs", "Patterns", "Shaders", "tefris", "xmf_tr", ".git" };

            try
            {
                // Ana dizini kontrol et
                if (!Directory.Exists(mainDir)) { throw new DirectoryNotFoundException($"'{mainDir}' dizini bulunamadı."); }

                // Tüm klasörleri al
                string[] allDirectories = Directory.GetDirectories(mainDir);

                // DataTable oluştur
                DataTable dtExcludedFolders = new DataTable();
                dtExcludedFolders.Columns.Add("LIB_NAME", typeof(string));

                foreach (string dir in allDirectories)
                {
                    string folderName = Path.GetFileName(dir);

                    // Eğer klasör ignoredFolders içinde değilse, DataTable'a ekle
                    if (!ignoredFolders.Contains(folderName))
                    {
                        DataRow row = dtExcludedFolders.NewRow();
                        row["LIB_NAME"] = folderName;
                        dtExcludedFolders.Rows.Add(row);
                    }
                }

                // DataTable'ı gridControl1'e ata
                gridControl1.DataSource = dtExcludedFolders;
            }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message, "Dosya Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            loadRowData(e.RowHandle);
        }

        private void loadRowData(int rowHandle)
        {
            // Seçilen satırdan kodu ve yolu al
            DataRow selectedRow = gridView1.GetDataRow(rowHandle);
            if (selectedRow != null)
            {
                libFolderName = selectedRow["LIB_NAME"].ToString();

                defGroupList.Clear();
                adekoModuleList.Clear();

                LoadGroupLst(libFolderName);

                if (defGroupList.Count > 0)
                {
                    foreach (var defGroup in defGroupList)
                    {
                        addDefFile_toDataTableDefs(defGroup);
                    }

                    adekoModuleList_canRead = new BindingList<AdekoModule>(adekoModuleList.Where(module => module.canRead).ToList());
                    adekoModuleList_cantRead = new BindingList<AdekoModule>(adekoModuleList.Where(module => !module.canRead).ToList());

                    gridControl2.DataSource = adekoModuleList_canRead;
                    gridControl2.RefreshDataSource();
                }
            }
        }

        private void LoadGroupLst(string libFolder)
        {
            try
            {
                // GRUP.LST dosyasının tam yolu
                string groupLstPath = Path.Combine(mainDir, libFolder, "GRUPS.LST");

                if (!File.Exists(groupLstPath)) { throw new FileNotFoundException($"'{groupLstPath}' dosyası bulunamadı."); }

                using (StreamReader reader = new StreamReader(groupLstPath, Encoding.GetEncoding("windows-1254")))
                {
                    string[] lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string line in lines)
                    {
                        Match match = Regex.Match(line, "\\(\"(.*?)\"\\s+\"(.*?)\"\\)");
                        if (match.Success)
                        {
                            DefGroup defGroup = new DefGroup()
                            {
                                code = match.Groups[1].Value,
                                name = match.Groups[2].Value
                            };

                            defGroupList.Add(defGroup);
                        }
                    }
                }

            }
            catch (FileNotFoundException ex) { MessageBox.Show(ex.Message, "Dosya Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void addDefFile_toDataTableDefs(DefGroup defGroup)
        {

            string defFilePath = Path.Combine(mainDir, libFolderName, $"{defGroup.code}.DEF");
            if (!File.Exists(defFilePath))
            {
                MessageBox.Show($"'{defGroup.code}.DEF' dosyası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string line = "";
            int rowNo = 0;
            bool canRead  = false;

            try
            {
                int lineCount = File.ReadLines(defFilePath).Count();

                using (StreamReader reader = new StreamReader(defFilePath, Encoding.GetEncoding("windows-1254")))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Console.WriteLine($"Line {rowNo + 1}: {line}"); // Log the line before processing

                        rowNo++;

                        try
                        {
                            canRead = true;
                            // Satırın başlangıç ve bitişindeki parantezleri kaldır
                            line = line.Trim().Trim('(', ')');

                            // Tırnak işaretlerini dikkate alarak ayır
                            List<string> parts = SplitWithQuotes(line);

                            string code = "";
                            if (parts.Count < 4) canRead = false; // Geçersiz satır
                            else code = parts[0].Trim('"');
                            if (code == ".") canRead = false;// Geçersiz satır

                            if (canRead)
                            {
                                string dt1 = parts[1].Trim('"');

                                string keyCode = parts[parts.Count - 2].Trim('"');
                                string description = parts[parts.Count - 1].Trim('"');

                                // Dinamik alanlar
                                int startIndex = 5;
                                int count = Math.Max(0, parts.Count - startIndex); // Ensure count is not negative
                                List<string> dynamicData = parts.GetRange(startIndex, count);
                                //List<string> dynamicData = parts.GetRange(5, parts.Count - 4);
                                string dynamicDataString = string.Join(" ", dynamicData);

                                // Genişlik, yükseklik ve derinlik bilgilerini ayıkla
                                decimal? width = ParseDecimal(parts.ElementAtOrDefault(2));
                                decimal? height = ParseDecimal(parts.ElementAtOrDefault(3));
                                decimal? depth = ParseDecimal(parts.ElementAtOrDefault(4));

                                // Modülü oluştur ve listeye ekle
                                AdekoModule adekoModule = new AdekoModule
                                {
                                    canRead = true,
                                    SelectModule = "Modül Seç ->",
                                    Code = code,
                                    Group = defGroup.name,
                                    Dt1 = dt1,
                                    Width = width,
                                    Height = height,
                                    Depth = depth,
                                    RootCode = keyCode,
                                    Description = description,
                                    //DynamicData = dynamicData,
                                    LibFolderName = libFolderName,
                                    FileName = defGroup.code,
                                    FileRowNo = rowNo,
                                    IsUpdated = false,
                                    OriginalDataLine = line,
                                    DynamicDataString = dynamicDataString
                                };
                                adekoModuleList.Add(adekoModule);
                            }
                            else
                            {
                                // Modülü oluştur ve listeye ekle
                                AdekoModule adekoModule = new AdekoModule
                                {
                                    canRead = false,
                                    Group = defGroup.name,
                                    FileName = defGroup.code,
                                    FileRowNo = rowNo,
                                    OriginalDataLine = line
                                };
                                adekoModuleList.Add(adekoModule);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Modülü oluştur ve listeye ekle
                            AdekoModule adekoModule = new AdekoModule
                            {
                                canRead = false,
                                Group = defGroup.name,
                                FileName = defGroup.code,
                                FileRowNo = rowNo,
                                OriginalDataLine = line
                            };
                            adekoModuleList.Add(adekoModule);
                        }
                        

                    }
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
            }
        }

        // Decimal parse etmek için yardımcı fonksiyon
        decimal ParseDecimal(string input)
        {
            // Giriş değerinin beklenen kültüre uygun şekilde ayrıştırılması
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                throw new FormatException($"'{input}' değeri geçerli bir decimal formatında değil.");
            }
        }

        private static List<string> SplitWithQuotes(string input)
        {
            // Regex: Çift tırnak içindeki ifadeleri veya tırnaksız ifadeleri yakalar
            var matches = Regex.Matches(input, @"\"".*?\""|[^\s]+");
            var result = new List<string>();

            foreach (Match match in matches)
            {
                string value = match.Value;

                // Eğer çift tırnak içindeyse, tırnakları kaldır
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                result.Add(value);
            }

            return result;
        }


        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            int rowHandle = gridView2.FocusedRowHandle;
            var rowObject = gridView2.GetRow(rowHandle);
            if (rowObject != null)
            {
                var myObject = rowObject as AdekoModule; //  DataSource'ta kullanılan tür
                if (myObject != null)
                {
                    string code = myObject.Code;
                    string folderName = myObject.FileName + "_tn";
                    string libFolderName = myObject.LibFolderName;

                    if (libFolderName != null && code != null)
                    {

                        string imgFilePath = Path.Combine(mainDir, libFolderName, folderName, $"{code}.bmp");

                        // Dosyanın mevcut olup olmadığını kontrol et
                        if (File.Exists(imgFilePath))
                        {
                            // Resmi PictureEdit kontrolünde göster
                            pictureEdit1.Image = Image.FromFile(imgFilePath);
                        }
                        else
                        {
                            MessageBox.Show($"Görsel bulunamadı: {imgFilePath}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pictureEdit1.Image = null; // Resim bulunamadığında kontrolü temizle
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seçilen satır uygun türde değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir veri satırı seçin.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool isUpdatingCellValue = false; // Sonsuz döngüyü önlemek için flag
        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (isUpdatingCellValue) return; // Eğer flag true ise işlem yapmadan çık

            try
            {
                isUpdatingCellValue = true; // İşlem başladığında flag'i true yap
                // Değişen satırın "IsUpdated" sütununu true olarak işaretle
                gridView2.SetRowCellValue(e.RowHandle, "IsUpdated", true);
            }
            finally
            {
                isUpdatingCellValue = false; // İşlem tamamlandığında flag'i false yap
            }

        }
    }

    public class DefGroup
    {
        public string code {get; set;}
        public string name {get; set;}
    }
    public class AdekoModule
    {
        public bool canRead { get; set; }
        public string SelectModule { get; set; }
        public string Code { get; set; }
        public string Dt1 { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public string RootCode { get; set; }
        public string Description { get; set; }
        //public List<string> DynamicData { get; set; } = new List<string>();
        public string DynamicDataString { get; set; } // Dinamik verilerin birleşik hali
        public string LibFolderName { get; set; }
        public string Group { get; set; }
        public string FileName { get; set; }
        public int FileRowNo { get; set; }
        public bool IsUpdated { get; set; }
        public string OriginalDataLine { get; set; } // Orijinal veri (değişmeyen kısımlar)
    }


}